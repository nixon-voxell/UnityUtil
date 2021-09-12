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

using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.Mathematics;

namespace Voxell.Mathx
{
  public static class float2_extension
  {
    public const float kEpsilon = 0.00001F;
    public const float kEpsilonNormalSqrt = 1e-15f;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2 perpendicular(this float2 vector) => new float2(-vector.y, vector.x);

    // Returns the angle in degrees between /from/ and /to/.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float angle(this float2 from, float2 to)
    {
      // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
      float denominator = (float)math.sqrt(math.lengthsq(from) * math.lengthsq(to));
      if (denominator < kEpsilonNormalSqrt)
        return 0F;

      float dot = math.clamp(math.dot(from, to) / denominator, -1F, 1F);
      return math.acos(dot) * Mathf.Rad2Deg;
    }
  }
}