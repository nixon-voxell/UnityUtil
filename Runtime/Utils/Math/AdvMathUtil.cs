/*
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software Foundation,
Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

The Original Code is Copyright (C) 2020 Voxell Technologies.
All rights reserved.
*/

using UnityEngine;
using System.Runtime.CompilerServices;

namespace Voxell.Mathx
{
  public static class AdvMathUtil
  {
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
    /// Calculates a 30-bit Morton code for the given 3D point located within the unit cube [0,1]
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="z">z coordinate</param>
    /// <returns>morton code</returns>
    public static uint Morton3D(float x, float y, float z)
    {
      x = Mathf.Min(Mathf.Max(x * 1024.0f, 0.0f), 1023.0f);
      y = Mathf.Min(Mathf.Max(y * 1024.0f, 0.0f), 1023.0f);
      z = Mathf.Min(Mathf.Max(z * 1024.0f, 0.0f), 1023.0f);
      uint xx = ExpandBits((uint)x);
      uint yy = ExpandBits((uint)y);
      uint zz = ExpandBits((uint)z);
      return xx * 4 + yy * 2 + zz;
    }

    /// <summary>
    /// Calculates a 30-bit Morton code for the given 3D point located within the unit cube [0,1]
    /// </summary>
    /// <param name="point">3D coordinate</param>
    /// <returns>morton code</returns>
    public static uint Morton3D(Vector3 point) => Morton3D(point.x, point.y, point.z);

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