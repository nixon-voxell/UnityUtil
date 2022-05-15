using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Voxell.Inspector.List
{
  public static class EditorListUtil
  {
    public static ReorderableList CreateReorderableList(
      SerializedObject serializedObject, SerializedProperty property, EditorListConfig listConfig
    )
    {
      bool hasEmptyMsg = !string.IsNullOrEmpty(listConfig.emptyMsg);
      string _emptyMsg = hasEmptyMsg ? listConfig.emptyMsg : "List is Empty";

      bool showPrefix = !string.IsNullOrEmpty(listConfig.prefix);
      if (string.IsNullOrEmpty(listConfig.header)) listConfig.header = property.displayName;

      ReorderableList list = new ReorderableList(
        serializedObject, property,
        listConfig.draggable, listConfig.displayHeader,
        listConfig.displayAddButton, listConfig.displayRemoveButton
      );

      InitializeReorderableList(list, listConfig);

      return list;
    }

    public static void InitializeReorderableList(ReorderableList list, EditorListConfig listConfig)
    {
      bool hasEmptyMsg = !string.IsNullOrEmpty(listConfig.emptyMsg);
      string _emptyMsg = hasEmptyMsg ? listConfig.emptyMsg : "List is Empty";

      bool showPrefix = !string.IsNullOrEmpty(listConfig.prefix);
      if (string.IsNullOrEmpty(listConfig.header)) listConfig.header = list.serializedProperty.displayName;

      list.multiSelect = listConfig.multiSelect;

      list.drawHeaderCallback = (Rect rect) =>
      {
        SerializedProperty property = list.serializedProperty;
        rect.height += 3.0f;
        EditorGUI.indentLevel += 1;

        property.isExpanded = EditorGUI.Foldout(
          rect, property.isExpanded, listConfig.header, true, VXEditorStyles.ReordableFoldoutStyle
        );
        list.draggable = property.isExpanded && listConfig.draggable;
        list.displayAdd = property.isExpanded && listConfig.displayAddButton;
        list.displayRemove = property.isExpanded && listConfig.displayRemoveButton;

        EditorGUI.indentLevel -= 1;
      };

      list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
      {
        SerializedProperty property = list.serializedProperty;
        if (!property.isExpanded) return;

        SerializedProperty elemProperty = property.GetArrayElementAtIndex(index);
        bool isGeneric = elemProperty.propertyType == SerializedPropertyType.Generic;
        if (isGeneric)
        {
          rect.x += 10.0f;
          rect.width -= 10.0f;
        }

        EditorGUI.PropertyField(
          rect, elemProperty,
          new GUIContent(showPrefix ? $"{listConfig.prefix}{index}" : ""), true
        );
      };

      list.elementHeightCallback = (int indexer) =>
      {
        SerializedProperty property = list.serializedProperty;
        if (!property.isExpanded) return 0.0f;
        else
        {
          if (indexer < property.arraySize)
            return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(indexer));

          return 0.0f;
        }
      };

      list.drawNoneElementCallback = (Rect rect) =>
      {
        SerializedProperty property = list.serializedProperty;
        if (property.isExpanded)
        {
          list.elementHeight = 22.0f;
          EditorGUI.LabelField(rect, _emptyMsg);
        } else list.elementHeight = 0.0f;
      };

      list.drawFooterCallback = (Rect rect) =>
      {
        SerializedProperty property = list.serializedProperty;
        if (property.isExpanded)
        {
          list.footerHeight = 22.0f;
          ReorderableList.defaultBehaviours.DrawFooter(rect, list);
        } else list.footerHeight = 0.0f;
      };
    }
  }
}