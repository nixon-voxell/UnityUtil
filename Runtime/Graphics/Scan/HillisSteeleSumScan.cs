using UnityEngine;
using UnityEngine.Profiling;

namespace Voxell.Graphics
{
  using Mathx;

  public sealed class HillisSteeleSumScan
  {
    private const int MAX_BLOCK_SZ = 64;

    private ComputeShader cs_hillisSteeleSumScan;
    private readonly int kn_hillisSteeleSumScan, kn_addBlockSums;

    private readonly int _dataSize;
    private readonly int _gridSize;

    public HillisSteeleSumScan(ref ComputeShader cs_hillisSteeleSumScan, int dataSize)
    {
      this.cs_hillisSteeleSumScan = cs_hillisSteeleSumScan;
      kn_hillisSteeleSumScan = cs_hillisSteeleSumScan.FindKernel("HillisSteeleSumScan");

      _dataSize = dataSize;
      _gridSize = MathUtil.CalculateGrids(_dataSize, MAX_BLOCK_SZ);
    }

    public void Scan(ref ComputeBuffer cb_in)
    {
      Profiler.BeginSample("HillisSteeleSumScan");
      cs_hillisSteeleSumScan.SetInt(ScanPropertyId.len, _dataSize);
      cs_hillisSteeleSumScan.SetBuffer(kn_hillisSteeleSumScan, ScanBufferId.cb_in, cb_in);

      for (int offset=1; offset < _dataSize; offset <<= 1)
      {
        cs_hillisSteeleSumScan.SetInt(ScanPropertyId.offset, offset);
        cs_hillisSteeleSumScan.Dispatch(kn_hillisSteeleSumScan, _gridSize, 1, 1);
      }
      Profiler.EndSample();
    }
  }
}