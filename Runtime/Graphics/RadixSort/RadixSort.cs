using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Voxell.Graphics
{
  using Mathx;

  public sealed class RadixSort : System.IDisposable
  {
    private const int MAX_BLOCK_SZ = 64;
    private const int BLOCK_SZ = 32;

    private static ComputeShader cs_radixSort;
    private BlellochSumScan _blellochSumScan;
    private static int kn_radixSortLocal, kn_globalShuffle;

    private readonly int _sortGridSize, _blockSumsSize;
    private readonly int _dataSize;

    private ComputeBuffer cb_sortTemp, cb_indexTemp, cb_prefixSums;
    private ComputeBuffer cb_blockSums, cb_scanBlockSums;

    public RadixSort(int dataSize)
    {
      this._dataSize = dataSize;

      this._sortGridSize = MathUtil.CalculateGrids(_dataSize, MAX_BLOCK_SZ);
      this._blockSumsSize = 4 * _sortGridSize;
      this._blellochSumScan = new BlellochSumScan(_blockSumsSize);

      this.cb_sortTemp = new ComputeBuffer(_dataSize, StrideSize.s_uint);
      this.cb_indexTemp = new ComputeBuffer(_dataSize, StrideSize.s_uint);
      this.cb_prefixSums = new ComputeBuffer(_dataSize, StrideSize.s_uint);
      this.cb_blockSums = new ComputeBuffer(_blockSumsSize, StrideSize.s_uint);
      this.cb_scanBlockSums = new ComputeBuffer(_blockSumsSize, StrideSize.s_uint);
    }

    public static void Init()
    {
      if (cs_radixSort != null) return;
      cs_radixSort = Resources.Load<ComputeShader>("RadixSort");
      kn_radixSortLocal = cs_radixSort.FindKernel("RadixSortLocal");
      kn_globalShuffle = cs_radixSort.FindKernel("GlobalShuffle");

      BlellochSumScan.Init();
    }

    /// <summary>Sorts an array of unsigned integers in parallel.</summary>
    public void Sort(int maxShiftWidth = 32)
    {
      Profiler.BeginSample("RadixSort");
      ComputeShaderUtil.ZeroOut(ref cb_sortTemp, _dataSize);
      ComputeShaderUtil.ZeroOut(ref cb_indexTemp, _dataSize);
      ComputeShaderUtil.ZeroOut(ref cb_prefixSums, _dataSize);
      ComputeShaderUtil.ZeroOut(ref cb_blockSums, _blockSumsSize);
      ComputeShaderUtil.ZeroOut(ref cb_scanBlockSums, _blockSumsSize);

      // for every 2 bits from LSB to MSB:
      // block-wise radix sort (write blocks back to global memory)
      for (int shiftWidth=0; shiftWidth < maxShiftWidth; shiftWidth+=2)
      {
        cs_radixSort.SetInt(RadixSortPropertyId.shiftWidth, shiftWidth);
        cs_radixSort.Dispatch(kn_radixSortLocal, _sortGridSize, 1, 1);

        // scan global block sum array
        ComputeShaderUtil.ZeroOut(ref cb_scanBlockSums, _blockSumsSize);
        _blellochSumScan.Scan(ref cb_blockSums, ref cb_scanBlockSums, _blockSumsSize);

        // scatter/shuffle block-wise sorted array to final positions
        cs_radixSort.Dispatch(kn_globalShuffle, _sortGridSize, 1, 1);
      }
      Profiler.EndSample();
    }

    /// <summary>
    /// Sets up radix sort algorithm (call this function when you want to change the target buffers and/or data size)
    /// </summary>
    /// <param name="cb_sort">buffer of unsigned integers to be sorted</param>
    /// <param name="cb_indices">buffer of indices for each unsigned integers</param>
    /// <param name="_dataSize">total number of unsigned integers to sort</param>
    /// <param name="_maxShiftWidth">number of bits to be sorted</param>
    public void Setup(ref ComputeBuffer cb_sort, ref ComputeBuffer cb_indices)
    {
      cs_radixSort.SetInt(RadixSortPropertyId.gridSize, _sortGridSize);
      cs_radixSort.SetInt(RadixSortPropertyId.len, _dataSize);

      cs_radixSort.SetBuffer(kn_radixSortLocal, RadixSortBufferId.cb_in, cb_sort);
      cs_radixSort.SetBuffer(kn_radixSortLocal, RadixSortBufferId.cb_indices, cb_indices);

      cs_radixSort.SetBuffer(kn_globalShuffle, RadixSortBufferId.cb_out, cb_sort);
      cs_radixSort.SetBuffer(kn_globalShuffle, RadixSortBufferId.cb_indices, cb_indices);

      cs_radixSort.SetBuffer(kn_radixSortLocal, RadixSortBufferId.cb_outSorted, cb_sortTemp);
      cs_radixSort.SetBuffer(kn_radixSortLocal, RadixSortBufferId.cb_outIndex, cb_indexTemp);
      cs_radixSort.SetBuffer(kn_radixSortLocal, RadixSortBufferId.cb_prefixSums, cb_prefixSums);
      cs_radixSort.SetBuffer(kn_radixSortLocal, RadixSortBufferId.cb_blockSums, cb_blockSums);

      cs_radixSort.SetBuffer(kn_globalShuffle, RadixSortBufferId.cb_outSorted, cb_sortTemp);
      cs_radixSort.SetBuffer(kn_globalShuffle, RadixSortBufferId.cb_outIndex, cb_indexTemp);
      cs_radixSort.SetBuffer(kn_globalShuffle, RadixSortBufferId.cb_prefixSums, cb_prefixSums);
      cs_radixSort.SetBuffer(kn_globalShuffle, RadixSortBufferId.cb_scanBlockSums, cb_scanBlockSums);
    }

    public void Dispose()
    {
      cb_sortTemp?.Dispose();
      cb_indexTemp?.Dispose();
      cb_prefixSums?.Dispose();
      cb_blockSums?.Dispose();
      cb_scanBlockSums?.Dispose();
      _blellochSumScan?.Dispose();
    }
  }
}