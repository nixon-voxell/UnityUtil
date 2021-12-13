using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Voxell.Mathx
{
  public static class AdvMathUtil
  {
    /// <summary>
    /// Calculates a 30-bit Morton code for the given 3D point located within the unit cube [0,1]
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="z">z coordinate</param>
    /// <returns>morton code</returns>
    public static uint Morton3D(float x, float y, float z) => Morton3D(new float3(x, y, z));

    /// <summary>
    /// Calculates a 30-bit Morton code for the given 3D point located within the unit cube [0,1]
    /// </summary>
    /// <param name="point">3D coordinate</param>
    /// <returns>morton code</returns>
    public static uint Morton3D(float3 point)
    {
      point *= 1024.0f;
      point = math.clamp(point, 0.0f, 1023.0f);
      uint xx = ExpandBits((uint)point.x);
      uint yy = ExpandBits((uint)point.y);
      uint zz = ExpandBits((uint)point.z);
      return xx * 4 + yy * 2 + zz;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint ExpandBits(uint v)
    {
      v = (v * 0x00010001u) & 0xFF0000FFu;
      v = (v * 0x00000101u) & 0x0F00F00Fu;
      v = (v * 0x00000011u) & 0xC30C30C3u;
      v = (v * 0x00000005u) & 0x49249249u;
      return v;
    }

    /// <summary>
    /// Count leading zeros of a 32 bit unsigned integer
    /// </summary>
    /// <param name="x">unsigned integer input</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint __clz(uint x)
    {
      // do the smearing
      x |= x >> 1; 
      x |= x >> 2;
      x |= x >> 4;
      x |= x >> 8;
      x |= x >> 16;
      // count the ones
      x -= x >> 1 & 0x55555555;
      x = (x >> 2 & 0x33333333) + (x & 0x33333333);
      x = (x >> 4) + x & 0x0f0f0f0f;
      x += x >> 8;
      x += x >> 16;
      // subtract # of 1s from 32
      return 32 - (x & 0x0000003f);
    }
  }
}