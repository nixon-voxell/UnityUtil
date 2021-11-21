using System.Runtime.CompilerServices;
using Unity.Mathematics;

namespace Voxell.Mathx
{
  public static class float3_extension
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int long_axis(this float3 v)
    {
      int i = 0;
      if (math.abs(v.y) > math.abs(v.x)) i = 1;
      if (math.abs(v.z) > math.abs(i == 0 ? v.x : v.y)) i = 2;
      return i;
    }
  }
}