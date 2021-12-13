using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Voxell.Jobx
{
  public static partial class Jobx
  {
    /// <summary>Inclusive Hillis Steele sum scan.</summary>
    [BurstCompile(CompileSynchronously = true)]
    private struct HillisSteeleSumScanJob : IJobParallelFor
    {
      [NativeDisableParallelForRestriction]
      private NativeArray<int> na_array;

      [NativeDisableParallelForRestriction, ReadOnly]
      private NativeArray<int> na_prevArray;
      private int _offset;

      public HillisSteeleSumScanJob(ref NativeArray<int> na_array, ref NativeArray<int> na_prevArray, int offset)
      {
        this.na_array = na_array;
        this.na_prevArray = na_prevArray;
        _offset = offset;
      }

      public void Execute(int index)
      {
        int sumIdx = index - _offset;
        if (sumIdx >= 0) na_array[index] += na_prevArray[sumIdx];
      }
    }

    /// <summary>Perform a Hillis Steele inclusive sum scan.</summary>
    public static void InclusiveSumScan(NativeArray<int> na_array)
    {
      int arrayLength = na_array.Length;
      NativeArray<int> na_prevArray = new NativeArray<int>(na_array, Allocator.TempJob);
      for (int offset=1; offset < arrayLength; offset <<= 1)
      {
        HillisSteeleSumScanJob sumScanJob = new HillisSteeleSumScanJob(ref na_array, ref na_prevArray, offset);
        JobHandle jobHandle = sumScanJob.Schedule(arrayLength, 16);
        jobHandle.Complete();
        na_prevArray.CopyFrom(na_array);
      }
      na_prevArray.Dispose();
    }
  }
}