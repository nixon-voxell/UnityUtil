using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Voxell.Jobs;

namespace Voxell
{
  public static class NativeUtil
  {
    /// <summary>Generate native array with all similar values in it.</summary>
    /// <param name="value">value to populate the array</param>
    /// <param name="length">length of array</param>
    /// <param name="allocator">type of allocation</param>
    public static NativeArray<T> GenerateArray<T>(
      T value, int length, Allocator allocator) where T : struct
    {
      NativeArray<T> na_array = new NativeArray<T>(length, allocator);
      for (int i=0; i < length; i++) na_array[i] = value;
      return na_array;
    }

    /// <summary>Check if native array has been created or not before disposing it.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeArray<T>(ref NativeArray<T> na_array) where T : unmanaged
    { if (na_array.IsCreated) na_array.Dispose(); }

    /// <summary>Check if native list has been created or not before disposing it.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeList<T>(ref NativeList<T> na_array) where T : unmanaged
    { if (na_array.IsCreated) na_array.Dispose(); }

    /// <summary>Reverse native array in parallel.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReverseArray<T>(ref NativeArray<T> na_array) where T : unmanaged
    {
      int arraySize = na_array.Length;
      int jobSize = na_array.Length/2;

      // do it sequentially if job size is too small
      if (jobSize < 256)
      {
        for (int i=0; i < jobSize; i++)
        {
          T elem = na_array[i];
          na_array[i] = na_array[arraySize - i];
          na_array[arraySize - i] = elem;
        }
        return;
      }

      // parallel array reversal
      ReverseArrayJob<T> reverseArrayJob = new ReverseArrayJob<T>(ref na_array, arraySize);
      JobHandle jobHandle = reverseArrayJob.Schedule(jobSize, 128);
      jobHandle.Complete();
    }
  }
}