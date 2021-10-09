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
  [BurstCompile]
  public struct SumScanJob : IJobParallelFor
  {
    [NativeDisableParallelForRestriction]
    private NativeArray<int> na_array;

    [NativeDisableParallelForRestriction, ReadOnly]
    private NativeArray<int> na_prevArray;
    private int _offset;

    public SumScanJob(ref NativeArray<int> na_array, ref NativeArray<int> na_prevArray, int offset)
    {
      this.na_array = na_array;
      this.na_prevArray = na_prevArray;
      _offset = offset;
    }

    public void Execute(int index)
    {
      int sumIdx = index - _offset;
      if (sumIdx > 0) na_array[index] += na_prevArray[sumIdx];
    }
  }
}