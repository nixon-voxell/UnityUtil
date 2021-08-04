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

using UnityEditor;
using UnityEngine;
using System.IO;

namespace Voxell.PythonVX
{
  public static class CustomMenuItem
  {
    [MenuItem("Assets/Create/Python Script", false, 80)]
    public static void CreatePlainTextFile()
    {
      string template = Resources.Load<TextAsset>("python_script").text;
      string projectWindowPath = AssetDatabase.GetAssetPath(Selection.activeObject);
      string targetPath = $"{projectWindowPath}/python_script.py";

      int count = 1;
      while (File.Exists(targetPath))
      {
        targetPath = $"{projectWindowPath}/python_script_{count}.py";
        count += 1;
      }

      StreamWriter streamWriter = File.CreateText(targetPath);
      streamWriter.Write(template);
      streamWriter.Close();
      AssetDatabase.Refresh();
    }
  }
}