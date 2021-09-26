using System.Runtime.CompilerServices;
using Unity.Collections;

namespace Voxell
{
  public static class NativeUtil
  {
    /// <summary>
    /// Generate native array with all similar values in it
    /// </summary>
    /// <param name="value">value to populate the array</param>
    /// <param name="length">length of array</param>
    /// <param name="allocator">type of allocation</param>
    public static NativeArray<T> GenerateArray<T>(
      T value, int length, Allocator allocator) where T : struct
    {
      NativeArray<T> array = new NativeArray<T>(length, allocator);
      for (int i=0; i < length; i++) array[i] = value;
      return array;
    }

    /// <summary>
    /// Check if native array has been created or not before disposing it
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeArray<T>(ref NativeArray<T> array) where T : struct
    { if (array.IsCreated) array.Dispose(); }

    /// <summary>
    /// Check if native list has been created or not before disposing it
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeList<T>(ref NativeList<T> array) where T : unmanaged
    { if (array.IsCreated) array.Dispose(); }
  }
}