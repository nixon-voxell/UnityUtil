using System.Runtime.CompilerServices;
using UnityEngine;
using Unity.Mathematics;

namespace Voxell.Mathx
{
  public static class float2_extension
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float2 perpendicular(this float2 vector) => new float2(-vector.y, vector.x);

    // Returns the angle in degrees between /from/ and /to/.
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float angle(this float2 from, float2 to)
    {
      // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
      float denominator = (float)math.sqrt(math.lengthsq(from) * math.lengthsq(to));
      if (denominator < Vector2.kEpsilonNormalSqrt)
        return 0F;

      float dot = math.clamp(math.dot(from, to) / denominator, -1F, 1F);
      return math.acos(dot) * Mathf.Rad2Deg;
    }
  }
}