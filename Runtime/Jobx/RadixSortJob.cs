using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Voxell.Jobx
{
  public static partial class Jobx
  {
    [BurstCompile(CompileSynchronously = true)]
    private struct RadixBitCheckJob : IJobParallelFor
    {
      public uint mask;

      public NativeArray<uint> na_values;
      // true if current bit is 1
      public NativeArray<bool> na_bit;
      public NativeArray<int> na_truePrefixSum;
      public NativeArray<int> na_falsePrefixSum;

      public RadixBitCheckJob(
        ref NativeArray<uint> na_values,
        ref NativeArray<bool> na_bit,
        ref NativeArray<int> na_truePrefixSum,
        ref NativeArray<int> na_falsePrefixSum
      )
      {
        this.na_values = na_values;
        this.na_bit = na_bit;
        this.na_truePrefixSum = na_truePrefixSum;
        this.na_falsePrefixSum = na_falsePrefixSum;
        this.mask = 1;
      }

      public void Execute(int index)
      {
        uint value = na_values[index];
        bool bit = (value & mask) > 0;

        na_bit[index] = bit;
        if (bit)
        {
          na_truePrefixSum[index] = 1;
          na_falsePrefixSum[index] = 0;
        } else
        {
          na_truePrefixSum[index] = 0;
          na_falsePrefixSum[index] = 1;
        }
      }
    }

    [BurstCompile(CompileSynchronously = true)]
    private struct RadixSortShuffleJob : IJobParallelFor
    {
      public int lastFalseIdx;

      public NativeArray<uint> na_values;
      public NativeArray<uint> na_sortedValues;
      public NativeArray<int> na_indices;
      public NativeArray<int> na_sortedIndices;
      // true if current bit is 1
      public NativeArray<bool> na_bit;
      public NativeArray<int> na_truePrefixSum;
      public NativeArray<int> na_falsePrefixSum;

      public RadixSortShuffleJob(
        ref NativeArray<uint> na_values,
        ref NativeArray<uint> na_sortedValues,
        ref NativeArray<int> na_indices,
        ref NativeArray<int> na_sortedIndices,
        ref NativeArray<bool> na_bit,
        ref NativeArray<int> na_truePrefixSum,
        ref NativeArray<int> na_falsePrefixSum
      )
      {
        this.na_values = na_values;
        this.na_sortedValues = na_sortedValues;
        this.na_indices = na_indices;
        this.na_sortedIndices = na_sortedIndices;
        this.na_bit = na_bit;
        this.na_truePrefixSum = na_truePrefixSum;
        this.na_falsePrefixSum = na_falsePrefixSum;

        lastFalseIdx = 0;
      }

      public void Execute(int index)
      {
        int sortIdx;
        if (na_bit[index])
        {
          // minus one because we use inclusive sum scan
          sortIdx = na_truePrefixSum[index] - 1 + lastFalseIdx;
          na_sortedValues[sortIdx] = na_values[index];
          na_sortedIndices[sortIdx] = na_indices[index];
        } else
        {
          sortIdx = na_falsePrefixSum[index] - 1;
          na_sortedValues[sortIdx] = na_values[index];
          na_sortedIndices[sortIdx] = na_indices[index];
        }
      }
    }

    /// <summary>Sort an array of unsigned integers.</summary>
    /// <param name="na_values">na_values to sort</param>
    /// <param name="na_indices">resultant index position after the sorting process</param>
    public static void RadixSort(
      NativeArray<uint> na_values,
      NativeArray<int> na_indices,
      int maxShiftWidth = 32
    )
    {
      uint mask = 1;
      int valueCount = na_values.Length;
      JobHandle jobHandle;

      NativeArray<uint> na_sortedValues = new NativeArray<uint>(na_values, Allocator.TempJob);
      NativeArray<int> na_sortedIndices = new NativeArray<int>(na_indices, Allocator.TempJob);
      NativeArray<bool> na_bit = new NativeArray<bool>(valueCount, Allocator.TempJob);
      NativeArray<int> na_truePrefixSum = new NativeArray<int>(valueCount, Allocator.TempJob);
      NativeArray<int> na_falsePrefixSum = new NativeArray<int>(valueCount, Allocator.TempJob);

      RadixBitCheckJob radixBitCheckJob = new RadixBitCheckJob(
        ref na_sortedValues, ref na_bit, ref na_truePrefixSum, ref na_falsePrefixSum
      );
      RadixSortShuffleJob radixSortShuffleJob = new RadixSortShuffleJob(
        ref na_values, ref na_sortedValues, ref na_indices, ref na_sortedIndices,
        ref na_bit, ref na_truePrefixSum, ref na_falsePrefixSum
      );

      for (int m=0; m < maxShiftWidth; m++)
      {
        radixBitCheckJob.mask = mask;
        jobHandle = radixBitCheckJob.Schedule(valueCount, 64);
        jobHandle.Complete();

        InclusiveSumScan(na_truePrefixSum);
        InclusiveSumScan(na_falsePrefixSum);
        radixSortShuffleJob.lastFalseIdx = na_falsePrefixSum[valueCount-1];

        jobHandle = radixSortShuffleJob.Schedule(valueCount, 64);
        jobHandle.Complete();
        na_values.CopyFrom(na_sortedValues);
        na_indices.CopyFrom(na_sortedIndices);

        mask <<= 1;
      }

      na_sortedValues.Dispose();
      na_sortedIndices.Dispose();
      na_bit.Dispose();
      na_truePrefixSum.Dispose();
      na_falsePrefixSum.Dispose();
    }
  }
}