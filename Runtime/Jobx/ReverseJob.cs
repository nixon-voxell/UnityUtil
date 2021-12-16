using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Voxell.Jobx
{
  public partial class Jobx
  {
    [BurstCompile]
    private struct ReverseArrayJob<T> : IJobParallelFor where T : struct
    {
      public int arraySize;

      [NativeDisableParallelForRestriction] public NativeArray<T> na_array;

      public ReverseArrayJob(ref NativeArray<T> na_array, int arraySize)
      {
        this.na_array = na_array;
        this.arraySize = arraySize;
      }

      public void Execute(int index)
      {
        T elem = na_array[index];
        na_array[index] = na_array[arraySize - index];
        na_array[arraySize - index] = elem;
      }
    }

    /// <summary>Reverse native array in parallel.</summary>
    public static void ReverseArray<T>(NativeArray<T> na_array) where T : struct
    {
      int arraySize = na_array.Length;
      int jobSize = na_array.Length/2;

      // parallel array reversal
      ReverseArrayJob<T> reverseArrayJob = new ReverseArrayJob<T>(ref na_array, arraySize);
      JobHandle jobHandle = reverseArrayJob.Schedule(jobSize, 128);
      jobHandle.Complete();
    }
  }
}