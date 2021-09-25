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
}