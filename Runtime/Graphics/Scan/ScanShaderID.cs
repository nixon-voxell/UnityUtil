using UnityEngine;

namespace Voxell.Graphics
{
  internal static class ScanPropertyId
  {
    public static readonly int len = Shader.PropertyToID("_len");
    public static readonly int offset = Shader.PropertyToID("_offset");
  }

  internal static class ScanBufferId
  {
    public static readonly int cb_in = Shader.PropertyToID("cb_in");
    public static readonly int cb_out = Shader.PropertyToID("cb_out");
    public static readonly int cb_prev = Shader.PropertyToID("cb_prev");
    public static readonly int cb_blockSums = Shader.PropertyToID("cb_blockSums");

    public static readonly int cb_outIndex = Shader.PropertyToID("cb_outIndex");
    public static readonly int cb_indices = Shader.PropertyToID("cb_indices");
  }
}