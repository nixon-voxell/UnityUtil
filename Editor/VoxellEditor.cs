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
      _methods = VXEditorUtil.GetAllMethods(
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
