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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Voxell.Inspector
{
  [CanEditMultipleObjects]
  [CustomEditor(typeof(UnityEngine.Object), true)]
  public class VoxellEditor : UnityEditor.Editor
  {
    private IEnumerable<MethodInfo> _methods;

    protected virtual void OnEnable()
    {
      _methods = EditorUtil.GetAllMethods(
        target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);
    }

    public override void OnInspectorGUI()
    {
      base.DrawDefaultInspector();
      DrawButtons();
    }

    protected void DrawButtons()
    {
      if (_methods.Any())
      {
        EditorGUILayout.Space();

        foreach (var method in _methods)
          CustomEditorGUI.MethodButton(serializedObject.targetObject, method);
      }
    }
  }
}
