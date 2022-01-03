using UnityEngine;
using UnityEditor;

namespace Voxell.Inspector
{
  public abstract class AbstractVXScriptableEditor : AbstractVXEditor
  {
    public SerializedObject serializedScriptableObject;

    public virtual void FindProperties() => serializedScriptableObject = new SerializedObject(target);

    public override void OnInspectorGUI()
    {
      EditorGUI.BeginChangeCheck();

      FindProperties();
      OnRender();

      EditorGUI.EndChangeCheck();
      serializedScriptableObject.ApplyModifiedProperties();
    }
  }
}