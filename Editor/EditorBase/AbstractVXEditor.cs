using UnityEngine;
using UnityEditor;

namespace Voxell.Inspector
{
  public abstract class AbstractVXEditor : Editor
  {
    public Color defaultBGColor;
    public Color defaultContentColor;
    public float defaultLabelWidth;
    public float defaultFieldWidth;

    public virtual void OnEnable()
    {
      defaultBGColor = GUI.backgroundColor;
      defaultContentColor = GUI.contentColor;
      defaultLabelWidth = EditorGUIUtility.labelWidth;
      defaultFieldWidth = EditorGUIUtility.fieldWidth;
    }
    public virtual void OnRender() => base.OnInspectorGUI();

    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      EditorGUI.BeginChangeCheck();

      OnRender();

      if (EditorGUI.EndChangeCheck()) OnChange();
      serializedObject.ApplyModifiedProperties();
    }

    public virtual void OnChange() {}
  }
}