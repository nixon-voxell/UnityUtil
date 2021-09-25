using UnityEngine;
using Unity.Mathematics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Voxell.Graphics
{
  public static class GraphicsUtil
  {
    /// <summary>
    /// Checks if compute buffers in the array has been created or not before releasing it
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReleaseBufferArray(ref ComputeBuffer[] bufferArray)
    {
      for (int b=0; b < bufferArray?.Length; b++)
        bufferArray[b]?.Release();
    }

    /// <summary>
    /// Checks if textures in the array has been created or not before releasing it
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReleaseTextureArray<T>(ref RenderTexture[] textureArray)
    {
      for (int b=0; b < textureArray?.Length; b++)
        textureArray[b]?.Release();
    }

    /// <summary>
    /// Checks if objects in the array has been created or not before destroying it
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DestroyArray<T>(ref T[] array) where T : Object
    {
      for (int b=0; b < array?.Length; b++)
        if (array[b] != null) GameObject.Destroy(array[b]);
    }

    /// <summary>
    /// Checks if objects in the array has been created or not before disposing it
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeArray<T>(ref T[] array) where T : System.IDisposable
    {
      for (int d=0; d < array?.Length; d++)
        array[d]?.Dispose();
    }
  }

  public static class StrideSize
  {
    public static readonly int s_float = Marshal.SizeOf(typeof(float));
    public static readonly int s_int = Marshal.SizeOf(typeof(int));
    public static readonly int s_uint = Marshal.SizeOf(typeof(uint));
    public static readonly int s_float2 =  Marshal.SizeOf(typeof(float2));
    public static readonly int s_float3 =  Marshal.SizeOf(typeof(float3));
    public static readonly int s_float4 =  Marshal.SizeOf(typeof(float4));
    public static readonly int s_float2x2 = Marshal.SizeOf(typeof(float2x2));
    public static readonly int s_float2x3 = Marshal.SizeOf(typeof(float2x3));
    public static readonly int s_float2x4 = Marshal.SizeOf(typeof(float2x4));
    public static readonly int s_float3x2 = Marshal.SizeOf(typeof(float3x2));
    public static readonly int s_float3x3 = Marshal.SizeOf(typeof(float3x3));
    public static readonly int s_float3x4 = Marshal.SizeOf(typeof(float3x4));
    public static readonly int s_float4x2 = Marshal.SizeOf(typeof(float4x2));
    public static readonly int s_float4x3 = Marshal.SizeOf(typeof(float4x3));
    public static readonly int s_float4x4 = Marshal.SizeOf(typeof(float4x4));
  }
}