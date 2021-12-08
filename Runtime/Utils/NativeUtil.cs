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
    public static void DisposeArray<T>(ref NativeArray<T> na_array) where T : struct
    { if (na_array.IsCreated) na_array.Dispose(); }

    /// <summary>Check if native list has been created or not before disposing it.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeList<T>(ref NativeList<T> na_array) where T : unmanaged
    { if (na_array.IsCreated) na_array.Dispose(); }

    /// <summary>Reverse native array in serial.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerialReverseArray<T>(ref NativeArray<T> na_array) where T : struct
    {
      int arraySize = na_array.Length;
      int jobSize = na_array.Length/2;

      for (int i=0; i < jobSize; i++)
      {
        T elem = na_array[i];
        na_array[i] = na_array[arraySize - i];
        na_array[arraySize - i] = elem;
      }
    }

    /// <summary>Reverse native list in serial.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SerialReverseList<T>(ref NativeList<T> na_list) where T : unmanaged
    {
      int arraySize = na_list.Length;
      int jobSize = na_list.Length/2;

      for (int i=0; i < jobSize; i++)
      {
        T elem = na_list[i];
        na_list[i] = na_list[arraySize - i];
        na_list[arraySize - i] = elem;
      }
    }

    /// <summary>Reverse native array in parallel.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ParallelReverseArray<T>(ref NativeArray<T> na_array) where T : struct
    {
      int arraySize = na_array.Length;
      int jobSize = na_array.Length/2;

      // parallel array reversal
      ReverseArrayJob<T> reverseArrayJob = new ReverseArrayJob<T>(ref na_array, arraySize);
      JobHandle jobHandle = reverseArrayJob.Schedule(jobSize, 128);
      jobHandle.Complete();
    }

    /// <summary>Reverse native list in parallel.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ParallelReverseList<T>(ref NativeList<T> na_list) where T : unmanaged
    {
      int arraySize = na_list.Length;
      int jobSize = na_list.Length/2;

      // parallel list reversal
      ReverseListJob<T> reverseArrayJob = new ReverseListJob<T>(ref na_list, arraySize);
      JobHandle jobHandle = reverseArrayJob.Schedule(jobSize, 128);
      jobHandle.Complete();
    }

    /// <summary>Perform a Hillis Steele inclusive sum scan.</summary>
    /// <param name="na_array"></param>
    public static void InclusiveSumScan(ref NativeArray<int> na_array)
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