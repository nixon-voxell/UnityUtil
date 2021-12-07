using UnityEditor;
using UnityEngine;

namespace Voxell.Inspector
{
  [CustomPropertyDrawer(typeof(InspectOnlyAttribute))]
  public class InspectOnlyDrawer : PropertyDrawer
  {
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
      GUI.enabled = false;
      EditorGUI.PropertyField(rect, property, label, true);
      GUI.enabled = true;
    }
  }

  [CustomPropertyDrawer(typeof(SceneAttribute))]
  public class SceneDrawer : PropertyDrawer
  {
    public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label)
    {
      if (property.propertyType == SerializedPropertyType.String)
      {
        SceneAsset sceneObject = GetSceneObject(property.stringValue);
        SceneAsset scene = EditorGUI.ObjectField(rect, label, sceneObject, typeof(SceneAsset), true) as SceneAsset;
        if (scene == null)
        {
          property.stringValue = "";
        } else if (scene.name != property.stringValue)
        {
          SceneAsset sceneObj = GetSceneObject(scene.name);
          if (sceneObj == null)
            Debug.LogWarning($"The scene {scene.name} cannot be used. To use this scene add it to the build settings for the project");
          else property.stringValue = scene.name;
        }
      }
      else EditorGUI.LabelField(rect, label.text, "Use [Scene] with strings.");
    }

    protected SceneAsset GetSceneObject(string sceneObjectName)
    {
      if (string.IsNullOrEmpty(sceneObjectName)) return null;

      foreach (EditorBuildSettingsScene editorScene in EditorBuildSettings.scenes)
      {
        if (editorScene.path.IndexOf(sceneObjectName) != -1)
          return AssetDatabase.LoadAssetAtPath<SceneAsset>(editorScene.path);
      }
      Debug.LogWarning($"Scene [{sceneObjectName}] cannot be used. Add this scene to the 'Scenes in the Build' in build settings.");
      return null;
    }
  }

  [CustomPropertyDrawer(typeof(StreamingAssetFilePathAttribute))]
  public class StreamingAssetFilePathDrawer : PropertyDrawer
  {
    public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label)
    {
      if (property.propertyType == SerializedPropertyType.String)
      {
        rect.size = new Vector2(rect.size.x, 20.0f);
        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true, VXEditorStyles.SubFoldoutStyle);
        if (property.isExpanded)
        {
          rect.y += 20.0f;
          if (GUI.Button(new Rect(rect.position, new Vector2(20.0f, 20.0f)), EditorGUIUtility.IconContent("Folder Icon").image))
          {
            string filePath = EditorUtility.OpenFilePanel("Asset File", Application.streamingAssetsPath, "");
            if (filePath != "") property.stringValue = filePath.Substring(Application.streamingAssetsPath.Length+1);
            property.serializedObject.ApplyModifiedProperties();
            GUIUtility.ExitGUI();
          }
          Rect assetLabelRect = new Rect(new Vector2(rect.x + 22.0f, rect.y), new Vector2(116.0f, rect.size.y));
          EditorGUI.LabelField(assetLabelRect, "StreamingAssets/");
          Rect filePathRect = new Rect(new Vector2(assetLabelRect.x + assetLabelRect.size.x, rect.y), new Vector2(rect.size.x - assetLabelRect.size.x - 22.0f, rect.size.y));
          GUI.enabled = false;
          property.stringValue = EditorGUI.TextField(filePathRect, property.stringValue);
          GUI.enabled = true;
        }
      }
      else EditorGUI.LabelField(rect, label.text, "Use [StreamingAssetFilePath] with strings.");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
      => property.isExpanded ? 40.0f : 20.0f;
  }

  [CustomPropertyDrawer(typeof(StreamingAssetFolderPathAttribute))]
  public class StreamingAssetFolderPathPathDrawer : PropertyDrawer
  {
    public override void OnGUI (Rect rect, SerializedProperty property, GUIContent label)
    {
      if (property.propertyType == SerializedPropertyType.String)
      {
        rect.size = new Vector2(rect.size.x, 20.0f);
        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true, VXEditorStyles.SubFoldoutStyle);
        if (property.isExpanded)
        {
          rect.y += 20.0f;
          if (GUI.Button(new Rect(rect.position, new Vector2(20.0f, 20.0f)), EditorGUIUtility.IconContent("Folder Icon").image))
          {
            string filePath = EditorUtility.OpenFolderPanel("Asset Folder", Application.streamingAssetsPath, "");
            if (filePath != "") property.stringValue = filePath.Substring(Application.streamingAssetsPath.Length+1);
            property.serializedObject.ApplyModifiedProperties();
            GUIUtility.ExitGUI();
          }
          Rect assetLabelRect = new Rect(new Vector2(rect.x + 22.0f, rect.y), new Vector2(116.0f, rect.size.y));
          EditorGUI.LabelField(assetLabelRect, "StreamingAssets/");
          Rect filePathRect = new Rect(new Vector2(assetLabelRect.x + assetLabelRect.size.x, rect.y), new Vector2(rect.size.x - assetLabelRect.size.x - 22.0f, rect.size.y));
          GUI.enabled = false;
          property.stringValue = EditorGUI.TextField(filePathRect, property.stringValue);
          GUI.enabled = true;
        }
      }
      else EditorGUI.LabelField(rect, label.text, "Use [StreamingAssetFilePath] with strings.");
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
      => property.isExpanded ? 40.0f : 20.0f;
  }
}