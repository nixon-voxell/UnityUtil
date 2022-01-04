using UnityEngine;
using UnityEditor;

namespace Voxell.Inspector
{
  public abstract class AbstractVXEditor : Editor
  {
    public Color defaultBGColor;

    public virtual void OnEnable() => defaultBGColor = GUI.backgroundColor;
    public virtual void OnRender() => base.OnInspectorGUI();

    public override void OnInspectorGUI()
    {
      EditorGUI.BeginChangeCheck();

      OnRender();

      EditorGUI.EndChangeCheck();
      serializedObject.ApplyModifiedProperties();
    }
  }
}