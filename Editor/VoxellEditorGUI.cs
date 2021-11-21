using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Voxell.Inspector
{
  public static class CustomEditorGUI
  {
    public static void MethodButton(UnityEngine.Object target, MethodInfo methodInfo)
    {
      ButtonAttribute buttonAttribute = (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
      string buttonName = string.IsNullOrEmpty(buttonAttribute.buttonName) ? ObjectNames.NicifyVariableName(methodInfo.Name) : buttonAttribute.buttonName;

      if (GUILayout.Button(buttonName))
      {
        object[] defaultParams = methodInfo.GetParameters().Select(p => p.DefaultValue).ToArray();
        IEnumerator methodResult = methodInfo.Invoke(target, defaultParams) as IEnumerator;
        if (!Application.isPlaying)
        {
          // Set target object and scene dirty to serialize changes to disk
          EditorUtility.SetDirty(target);

          PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
          // Prefab mode
          if (stage != null) EditorSceneManager.MarkSceneDirty(stage.scene);
          // Normal scene
          else EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        } else if (methodResult != null && target is MonoBehaviour behaviour)
          behaviour.StartCoroutine(methodResult);
      }
    }
  }
}