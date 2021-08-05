using UnityEngine;
using Unity.Mathematics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Voxell.Graphics
{
  public static class GraphicsUtil
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeBufferArray(ref ComputeBuffer[] bufferArray)
    {
      for (int b=0; b < bufferArray?.Length; b++)
        if (bufferArray[b] != null) bufferArray[b].Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReleaseBufferArray(ref ComputeBuffer[] bufferArray)
    {
      for (int b=0; b < bufferArray?.Length; b++)
        if (bufferArray[b] != null) bufferArray[b].Release();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeTextureArray(ref RenderTexture[] textureArray)
    {
      for (int b=0; b < textureArray?.Length; b++)
        if (textureArray[b] != null) textureArray[b] = null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void DisposeArray<T>(ref T[] disposableArray) where T : System.IDisposable
    {
      for (int d=0; d < disposableArray?.Length; d++)
        if (disposableArray[d] != null) disposableArray[d].Dispose();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ReleaseTextureArray(ref RenderTexture[] textureArray)
    {
      for (int b=0; b < textureArray?.Length; b++)
        if (textureArray[b] != null) textureArray[b].Release();
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