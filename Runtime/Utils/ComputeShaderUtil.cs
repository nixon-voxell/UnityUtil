using UnityEngine;
using Voxell.Mathx;

namespace Voxell.Graphics
{
  public static class ComputeShaderUtil
  {
    public static ComputeShader util;

    private const int MAX_BLOCK_SZ = 128;
    private static int kn_CopyBuffer, kn_ZeroOut, kn_SetBufferAsThreadIdx;

    /// <summary>
    /// copy a buffer to another buffer
    /// </summary>
    /// <param name="cb_in">source buffer</param>
    /// <param name="cb_out">destination buffer</param>
    /// <param name="dataSize">buffer size</param>
    public static void CopyBuffer(ref ComputeBuffer cb_in, ref ComputeBuffer cb_out, int dataSize)
    {
      int gridSize = MathUtil.CalculateGrids(dataSize, MAX_BLOCK_SZ);
      util.SetBuffer(kn_CopyBuffer, ShaderBufferId.cb_in, cb_in);
      util.SetBuffer(kn_CopyBuffer, ShaderBufferId.cb_out, cb_out);
      util.Dispatch(kn_CopyBuffer, gridSize, 1, 1);
    }

    public static void ZeroOut(ref ComputeBuffer cb_out, int dataSize)
    {
      int gridSize = MathUtil.CalculateGrids(dataSize, MAX_BLOCK_SZ);
      util.SetBuffer(kn_ZeroOut, ShaderBufferId.cb_out, cb_out);
      util.Dispatch(kn_ZeroOut, gridSize, 1, 1);
    }

    public static void SetBufferAsThreadIdx(ref ComputeBuffer cb_out, int dataSize)
    {
      int gridSize = MathUtil.CalculateGrids(dataSize, MAX_BLOCK_SZ);
      util.SetBuffer(kn_SetBufferAsThreadIdx, ShaderBufferId.cb_out, cb_out);
      util.Dispatch(kn_SetBufferAsThreadIdx, gridSize, 1, 1);
    }

    public static void Init()
    {
      if (util == null)
      {
        util = Resources.Load<ComputeShader>("Util");
        kn_CopyBuffer = util.FindKernel("CopyBuffer");
        kn_ZeroOut = util.FindKernel("ZeroOut");
        kn_SetBufferAsThreadIdx = util.FindKernel("SetBufferAsThreadIdx");
      }
    }

    internal static class ShaderBufferId
    {
      public static readonly int cb_in = Shader.PropertyToID("cb_in");
      public static readonly int cb_out = Shader.PropertyToID("cb_out");
    }
  }
}