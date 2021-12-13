using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Voxell.Jobx
{
  public static partial class Jobx
  {
    [BurstCompile]
    private struct ReverseArrayJob<T> : IJobParallelFor where T : struct
    {
      private NativeArray<T> na_array;
      private int _arraySize;

      public ReverseArrayJob(ref NativeArray<T> na_array, int arraySize)
      {
        this.na_array = na_array;
        _arraySize = arraySize;
      }

      public void Execute(int index)
      {
        T elem = na_array[index];
        na_array[index] = na_array[_arraySize - index];
        na_array[_arraySize - index] = elem;
      }
    }

    /// <summary>Reverse native array in parallel.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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