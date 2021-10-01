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

using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;

namespace Voxell.Jobs
{
  [BurstCompile(CompileSynchronously = true)]
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