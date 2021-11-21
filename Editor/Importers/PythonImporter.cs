using UnityEngine;
using UnityEditor.AssetImporters;

namespace Voxell.PythonVX
{
  [ScriptedImporter(1, "py")]
  public class PythonImporter : ScriptedImporter
  {
    public override void OnImportAsset(AssetImportContext ctx)
    {
      TextAsset pythonAsset = new TextAsset(FileUtil.ReadAssetFileText(ctx.assetPath));
      pythonAsset.name = FileUtil.GetFilename(ctx.assetPath);
      ctx.AddObjectToAsset("pythonAsset", pythonAsset, Resources.Load<Texture2D>("PythonLogo"));
      ctx.SetMainObject(pythonAsset);
    }
  }
}