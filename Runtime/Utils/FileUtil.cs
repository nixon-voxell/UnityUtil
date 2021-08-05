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

using System.IO;
using UnityEngine;

namespace Voxell
{
  public static class FileUtil
  {
    public static string projectPath
    { get => Application.dataPath.Substring(0, Application.dataPath.Length-6); }

    /// <summary>
    /// Get the path of the folder given a file path by excluding the filename
    /// </summary>
    /// <param name="assetPath">full file path</param>
    /// <returns>Folder path of the file</returns>
    public static string GetFolderPath(string assetPath)
    {
      string folder = "";
      string[] paths = assetPath.Split(new char[]{'/', '\\'});
      for (int p=0; p < paths.Length-1; p++) folder += paths[p] + '/';
      return folder;
    }

    /// <summary>
    /// Get the path of the folder given a file path by excluding the filename
    /// </summary>
    /// <param name="assetPath">full file path</param>
    /// <param name="separator">separator of each folder in the assetPath</param>
    /// <returns>Folder path of the file</returns>
    public static string GetFolderPath(string assetPath, char[] separator)
    {
      string folder = "";
      string[] paths = assetPath.Split(separator);
      for (int p=0; p < paths.Length-1; p++) folder += paths[p] + '/';
      return folder;
    }

    /// <summary>
    /// Get the name of the file given the file path by excluding all of its folder paths
    /// </summary>
    /// <param name="assetPath">full file path</param>
    /// <returns></returns>
    public static string GetFilename(string assetPath)
    {
      string[] paths = assetPath.Split(new char[]{'/', '\\'});
      return paths[paths.Length-1];
    }

    /// <summary>
    /// Get the name of the file given the file path by excluding all of its folder paths
    /// </summary>
    /// <param name="assetPath">full file path</param>
    /// <param name="separator">separator of each folder in the assetPath</param>
    /// <returns></returns>
    public static string GetFilename(string assetPath, char[] separator)
    {
      string[] paths = assetPath.Split(separator);
      return paths[paths.Length-1];
    }

    /// <summary>
    /// Read all the bytes of a given VoxellAsset
    /// </summary>
    /// <returns>Raw bytes from the file</returns>
    public static byte[] ReadAssetFileByte(string assetPath)
      => File.ReadAllBytes(GetAssetFilePath(assetPath));

    /// <summary>
    /// Read all the string text of a given VoxellAsset
    /// </summary>
    /// <returns>Raw string from the file</returns>
    public static string ReadAssetFileText(string assetPath)
      => File.ReadAllText(GetAssetFilePath(assetPath));

    /// <summary>
    /// Return the full file path given an asset path
    /// </summary>
    /// <param name="assetPath">Path that starts from the 'Assets/' folder</param>
    public static string GetAssetFilePath(string assetPath)
      => Path.Combine(projectPath, assetPath);

    /// <summary>
    /// Return the full file path given an streaming asset path
    /// </summary>
    /// <param name="streamingAssetPath">Path that is in the StreamingAssets/ folder</param>
    public static string GetStreamingAssetFilePath(string streamingAssetPath)
      => Path.Combine(Application.streamingAssetsPath, streamingAssetPath);

    /// <summary>
    /// Read the file and return raw bytes from the file
    /// </summary>
    /// <param name="path">file path starting from and excluding Application.streamingAssetsPath</param>
    /// <returns>Raw bytes from the file</returns>
    public static byte[] ReadStreamingAssetFileByte(string path)
      => File.ReadAllBytes(GetStreamingAssetFilePath(path));

    /// <summary>
    /// Read the file and return raw string from the file
    /// </summary>
    /// <param name="path">file path starting excluding Application.streamingAssetsPath</param>
    /// <returns>Raw string from the file</returns>
    public static string ReadStreamingAssetFileText(string path)
      => File.ReadAllText(GetStreamingAssetFilePath(path));

  }
}
