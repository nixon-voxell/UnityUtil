using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Voxell.Jobs
{
  /// <summary>Inclusive Hillis Steele sum scan.</summary>
  [BurstCompile]
  public struct HillisSteeleSumScanJob : IJobParallelFor
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
}