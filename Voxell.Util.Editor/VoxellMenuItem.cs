using UnityEditor;
using System.Reflection;

namespace Voxell.Util.Editor
{
    public static class VoxellMenuItem
    {
        [MenuItem("Shortcuts/Clear Console %#d")] // CTRL + SHIFT + D
        public static void ClearConsole()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
            System.Type type = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}
