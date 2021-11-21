using UnityEditor;
using UnityEngine;
using System.IO;

namespace Voxell.PythonVX
{
  public static class CustomMenuItem
  {
    [MenuItem("Assets/Create/Python Script", false, 80)]
    public static void CreatePlainTextFile()
    {
      string template = Resources.Load<TextAsset>("python_script").text;
      string projectWindowPath = AssetDatabase.GetAssetPath(Selection.activeObject);
      string targetPath = $"{projectWindowPath}/python_script.py";

      int count = 1;
      while (File.Exists(targetPath))
      {
        targetPath = $"{projectWindowPath}/python_script_{count}.py";
        count += 1;
      }

      StreamWriter streamWriter = File.CreateText(targetPath);
      streamWriter.Write(template);
      streamWriter.Close();
      AssetDatabase.Refresh();
    }
  }
}