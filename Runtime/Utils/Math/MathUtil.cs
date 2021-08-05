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
    /// Shuffles an array
    /// </summary>
    public static void ShuffleArray<T>(ref T[] decklist)
    {
      for (int i = 0; i < decklist.Length; i++)
      {
        int randomIdx = UnityEngine.Random.Range(0, decklist.Length);
        T tempItem = decklist[randomIdx];
        decklist[randomIdx] = decklist[i];
        decklist[i] = tempItem;
      }
    }
  }
}