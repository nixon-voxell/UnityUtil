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
using UnityEditorInternal;

namespace Voxell.Inspector
{
  public static class VXEditorStyles
  {
    public const int spaceA = 20, spaceB = 10;
    public static readonly GUIStyleState foldoutNormal = new GUIStyleState { textColor = Color.gray };
    public static readonly GUIStyleState foldoutOnNormal= new GUIStyleState { textColor = new Color(0.7f, 1f, 1f, 1f) };

    public static readonly GUIStyleState subFoldoutNormal = new GUIStyleState { textColor = Color.gray * Color.cyan };
    public static readonly GUIStyleState subFoldoutOnNormal= new GUIStyleState { textColor = Color.cyan };

    public static GUIStyle CenteredLabelStyle => new GUIStyle(GUI.skin.label)
    {
      alignment = TextAnchor.UpperCenter,
      fontStyle = FontStyle.Bold,
      fontSize = 12
    };

    public static GUIStyle FoldoutStyle => new GUIStyle(EditorStyles.foldout)
    {
      fontStyle = FontStyle.Bold,
      fontSize = 14,
      normal = foldoutNormal,
      onNormal = foldoutOnNormal,
      hover = foldoutOnNormal
    };

    public static GUIStyle SubFoldoutStyle => new GUIStyle(EditorStyles.foldout)
    {
      fontStyle = FontStyle.Bold,
      fontSize = 12,
      normal = subFoldoutNormal,
      onNormal = subFoldoutOnNormal
    };

    public static GUIStyle ReordableFoldoutStyle => new GUIStyle(GUI.skin.label)
    {
      fontStyle = FontStyle.Bold,
      fontSize = 12,
      normal = subFoldoutNormal,
      onNormal = subFoldoutOnNormal
    };

    public static GUIStyle NotesLabel => new GUIStyle(GUI.skin.label)
    {
      fontStyle = FontStyle.Italic,
      fontSize = 10,
      alignment = TextAnchor.MiddleRight
    };

    public static GUIStyle box => new GUIStyle(GUI.skin.box)
    { padding = new RectOffset(10, 10, 10, 10) };

    public static ReorderableList FoldableReorderableList(
      SerializedObject serializedObject,
      SerializedProperty property,
      bool draggable, bool displayHeader,
      bool displayAddButton, bool displayRemoveButton,
      string prefix = ""
    )
    {
      bool showPrefix = !string.IsNullOrEmpty(prefix);

      ReorderableList list = new ReorderableList(
        serializedObject, property,
        draggable, displayHeader,
        displayAddButton, displayRemoveButton
      );

      list.drawHeaderCallback = (Rect rect) =>
      {
        EditorGUI.indentLevel += 1;
        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, property.displayName, true, ReordableFoldoutStyle);
        list.draggable = property.isExpanded && draggable;
        list.displayAdd = property.isExpanded && displayAddButton;
        list.displayRemove = property.isExpanded && displayRemoveButton;
        EditorGUI.indentLevel -= 1;
      };

      list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
      {
        if (!property.isExpanded)
        {
          GUI.enabled = index == property.arraySize-1;
          return;
        }
        EditorGUI.PropertyField(
          rect, property.GetArrayElementAtIndex(index),
          new GUIContent(showPrefix ? $"{prefix}{index}" : "")
        );
      };

      list.elementHeightCallback = (int indexer) =>
      {
        if (!property.isExpanded) return 0;
        else return list.elementHeight;
      };

      return list;
    }
  }
}