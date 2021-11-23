using UnityEngine;
using UnityEditor.AssetImporters;

namespace Voxell.PythonVX
{
  [ScriptedImporter(1, "py")]
  public class PythonImporter : ScriptedImporter
  {
    public override void OnImportAsset(AssetImportContext ctx)
    {
      TextAsset pythonAsset = new TextAsset(FileUtilx.ReadAssetFileText(ctx.assetPath));
      pythonAsset.name = FileUtilx.GetFilename(ctx.assetPath);
      ctx.AddObjectToAsset("pythonAsset", pythonAsset, Resources.Load<Texture2D>("PythonLogo"));
      ctx.SetMainObject(pythonAsset);
    }
  }
}