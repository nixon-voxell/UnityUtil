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
using Unity.Mathematics;

namespace Voxell.Mathx
{
  public static class MathUtil
  {
    /// <summary>
    /// Calculate the least amount of split from the total size given a split size
    /// </summary>
    /// <param name="totalSize">total amount of size available</param>
    /// <param name="splitSize">maximum size of each divisions after the split</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CalculateGrids(int totalSize, int splitSize) => (totalSize + splitSize - 1) / splitSize;
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CalculateGrids(uint totalThreads, uint grpSize) => (int)((totalThreads + grpSize - 1) / grpSize);

    /// <summary>
    /// Set all values in that array to the given value
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetArray<T>(ref T[] array, T value)
    { for (int i=0; i < array.Length; i++) array[i] = value; }

    /// <summary>
    /// Generate an int array with sequence id as it's value
    /// (ex: [0, 1, 2, 3, 4] will be generated if `length` of 5 is being given)
    /// </summary>
    /// <param name="length">number of elements</param>
    public static int[] GenerateSeqArray(int length)
    {
      int[] array = new int[length];
      for (int i=0; i < length; i++) array[i] = i;
      return array;
    }

    /// <summary>
    /// Shuffles an array
    /// </summary>
    public static void ShuffleArray<T>(ref T[] decklist, uint seed=1)
    {
      // make sure seed is not 0
      Random rand = new Random(math.max(seed, 1));
      for (int i = 0; i < decklist.Length; i++)
      {
        int randomIdx = rand.NextInt(0, decklist.Length);
        T tempItem = decklist[randomIdx];
        decklist[randomIdx] = decklist[i];
        decklist[i] = tempItem;
      }
    }
  }
}