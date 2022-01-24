using UnityEngine;

namespace Voxell.Graphics
{
  internal static class RadixSortPropertyId
  {
    public static readonly int len = Shader.PropertyToID("_len");
    public static readonly int gridSize = Shader.PropertyToID("_gridSize");
    public static readonly int shiftWidth = Shader.PropertyToID("_shiftWidth");
  }

  internal static class RadixSortBufferId
  {
    public static readonly int cb_in = Shader.PropertyToID("cb_in");
    public static readonly int cb_out = Shader.PropertyToID("cb_out");
    public static readonly int cb_outSorted = Shader.PropertyToID("cb_outSorted");
    public static readonly int cb_prefixSums = Shader.PropertyToID("cb_prefixSums");
    public static readonly int cb_blockSums = Shader.PropertyToID("cb_blockSums");
    public static readonly int cb_scanBlockSums = Shader.PropertyToID("cb_scanBlockSums");

    public static readonly int cb_outIndex = Shader.PropertyToID("cb_outIndex");
    public static readonly int cb_indices = Shader.PropertyToID("cb_indices");
  }
}