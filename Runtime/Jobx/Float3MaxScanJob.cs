using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Voxell.Jobx
{
  public static partial class Jobx
  {
    [BurstCompile(CompileSynchronously = true)]
    private struct HillisSteeleFloat3MaxScanJob : IJobParallelFor
    {
      public int offset;

      [NativeDisableParallelForRestriction]
      public NativeArray<float3> na_values;
      [NativeDisableParallelForRestriction, ReadOnly]
      public NativeArray<float3> na_prevValues;

      public HillisSteeleFloat3MaxScanJob(
        ref NativeArray<float3> na_values, ref NativeArray<float3> na_prevValues
      )
      {
        this.na_values = na_values;
        this.na_prevValues = na_prevValues;
        this.offset = 1;
      }

      public void Execute(int index)
      {
        int sumIdx = index - offset;
        if (sumIdx >= 0) na_values[index] = math.max(na_values[index], na_prevValues[sumIdx]);
      }
    }

    /// <summary>Perform a Hillis Steele inclusive max scan.</summary>
    public static void InclusiveFloat3MaxScan(NativeArray<float3> na_values)
    {
      int valueCount = na_values.Length;
      JobHandle jobHandle;

      NativeArray<float3> na_prevValues = new NativeArray<float3>(na_values, Allocator.TempJob);
      HillisSteeleFloat3MaxScanJob maxScanJob = new HillisSteeleFloat3MaxScanJob(
        ref na_values, ref na_prevValues
      );

      for (int offset=1; offset < valueCount; offset <<= 1)
      {
        maxScanJob.offset = offset;
        jobHandle = maxScanJob.Schedule(valueCount, M_BATCH_SIZE);
        jobHandle.Complete();
        na_prevValues.CopyFrom(na_values);
      }

      na_prevValues.Dispose();
    }
  }
}