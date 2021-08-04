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

The Original Code is Copyright (C) 2020 Voxell Technologies and Contributors.
All rights reserved.
*/

using UnityEngine;
using UnityEditor.AssetImporters;

namespace Voxell.PythonVX
{
  [ScriptedImporter(1, "py")]
  public class PythonImporter : ScriptedImporter
  {
    public override void OnImportAsset(AssetImportContext ctx)
    {
      TextAsset pythonAsset = new TextAsset(FileUtil.ReadAssetFileText(ctx.assetPath));
      pythonAsset.name = FileUtil.GetFilename(ctx.assetPath);
      ctx.AddObjectToAsset("pythonAsset", pythonAsset, Resources.Load<Texture2D>("PythonLogo"));
      ctx.SetMainObject(pythonAsset);
    }
  }
}