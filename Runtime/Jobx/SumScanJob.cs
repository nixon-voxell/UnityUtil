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
      public int offset;

      [NativeDisableParallelForRestriction]
      public NativeArray<int> na_array;
      [NativeDisableParallelForRestriction, ReadOnly]
      public NativeArray<int> na_prevArray;

      public HillisSteeleSumScanJob(ref NativeArray<int> na_array, ref NativeArray<int> na_prevArray)
      {
        this.na_array = na_array;
        this.na_prevArray = na_prevArray;
        this.offset = 1;
      }

      public void Execute(int index)
      {
        int sumIdx = index - offset;
        if (sumIdx >= 0) na_array[index] += na_prevArray[sumIdx];
      }
    }

    /// <summary>Perform a Hillis Steele inclusive sum scan.</summary>
    public static void InclusiveSumScan(NativeArray<int> na_array)
    {
      int arrayLength = na_array.Length;
      JobHandle jobHandle;

      NativeArray<int> na_prevArray = new NativeArray<int>(na_array, Allocator.TempJob);
      HillisSteeleSumScanJob sumScanJob = new HillisSteeleSumScanJob(ref na_array, ref na_prevArray);

      for (int offset=1; offset < arrayLength; offset <<= 1)
      {
        sumScanJob.offset = offset;
        jobHandle = sumScanJob.Schedule(arrayLength, 64);
        jobHandle.Complete();
        na_prevArray.CopyFrom(na_array);
      }

      na_prevArray.Dispose();
    }
  }
}