using Unity.Mathematics;
using Unity.Collections;

namespace Voxell.Mathx
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

    public static float2[] Vector2ArrayToFloat2Array(UnityEngine.Vector2[] v)
    {
      float2[] target = new float2[v.Length];
      for (int i=0; i < v.Length; i++) target[i] = v[i];
      return target;
    }

    public static float3[] Vector3ArrayToFloat3Array(UnityEngine.Vector3[] v)
    {
      float3[] target = new float3[v.Length];
      for (int i=0; i < v.Length; i++) target[i] = v[i];
      return target;
    }

    public static float4[] Vector4ArrayToFloat4Array(UnityEngine.Vector4[] v)
    {
      float4[] target = new float4[v.Length];
      for (int i=0; i < v.Length; i++) target[i] = v[i];
      return target;
    }

    /// <summary>
    /// Check if native array has been created or not before disposing it
    /// </summary>
    public static void DisposeArray<T>(ref NativeArray<T> array) where T : struct
    { if (array.IsCreated) array.Dispose(); }

    /// <summary>
    /// Check if native list has been created or not before disposing it
    /// </summary>
    public static void DisposeList<T>(ref NativeList<T> array) where T : unmanaged
    { if (array.IsCreated) array.Dispose(); }
  }
}