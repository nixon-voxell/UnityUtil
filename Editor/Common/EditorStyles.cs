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
using UnityEditor;

namespace Voxell.Inspector
{
  public static class VoxellEditorStyles
  {
    public const int spaceA = 20, spaceB = 10;
    public static readonly GUIStyleState foldoutNormal = new GUIStyleState { textColor = Color.gray };
    public static readonly GUIStyleState foldoutOnNormal= new GUIStyleState { textColor = new Color(0.7f, 1f, 1f, 1f) };

    public static readonly GUIStyleState subFoldoutNormal = new GUIStyleState { textColor = Color.gray * Color.cyan };
    public static readonly GUIStyleState subFoldoutOnNormal= new GUIStyleState { textColor = Color.cyan };

    public static readonly GUIStyle centeredLabelStyle = new GUIStyle(GUI.skin.label)
    {
      alignment = TextAnchor.UpperCenter,
      fontStyle = FontStyle.Bold,
      fontSize = 12
    };

    public static readonly GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout)
    {
      fontStyle = FontStyle.Bold,
      fontSize = 14,
      normal = foldoutNormal,
      onNormal = foldoutOnNormal,
      hover = foldoutOnNormal
    };

    public static readonly GUIStyle subFoldoutStyle = new GUIStyle(EditorStyles.foldout)
    {
      fontStyle = FontStyle.Bold,
      fontSize = 12,
      normal = subFoldoutNormal,
      onNormal = subFoldoutOnNormal
    };

    public static readonly GUIStyle notes = new GUIStyle(GUI.skin.label)
    {
      fontStyle = FontStyle.Italic,
      fontSize = 10,
      alignment = TextAnchor.MiddleRight
    };

    public static readonly GUIStyle box = new GUIStyle(GUI.skin.box)
    { padding = new RectOffset(10, 10, 10, 10) };
  }
}