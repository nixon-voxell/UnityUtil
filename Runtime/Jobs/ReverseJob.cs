using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Voxell.Jobs
{
  [BurstCompile]
  public struct ReverseArrayJob<T> : IJobParallelFor
  where T : struct
  {
    private NativeArray<T> na_array;
    private int _arraySize;

    public ReverseArrayJob(ref NativeArray<T> na_array, int arraySize)
    {
      this.na_array = na_array;
      _arraySize = arraySize;
    }

    public void Execute(int index)
    {
      T elem = na_array[index];
      na_array[index] = na_array[_arraySize - index];
      na_array[_arraySize - index] = elem;
    }
  }

  [BurstCompile(CompileSynchronously = true)]
  public struct ReverseListJob<T> : IJobParallelFor
  where T : unmanaged
  {
    private NativeList<T> na_list;
    private int _arraySize;

    public ReverseListJob(ref NativeList<T> na_list, int arraySize)
    {
      this.na_list = na_list;
      _arraySize = arraySize;
    }

    public void Execute(int index)
    {
      T elem = na_list[index];
      na_list[index] = na_list[_arraySize - index];
      na_list[_arraySize - index] = elem;
    }
  }
}