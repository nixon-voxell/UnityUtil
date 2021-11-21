using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;

namespace Voxell
{
  [ScriptedImporter(1, "yml")]
  public class YMLImporter : ScriptedImporter
  {
    public override void OnImportAsset(AssetImportContext ctx)
    {
      TextAsset ymlAsset = new TextAsset(File.ReadAllText(ctx.assetPath));
      ctx.AddObjectToAsset("ymlAsset", ymlAsset, Resources.Load<Texture2D>("YMLLogo"));
      ctx.SetMainObject(ymlAsset);
    }
  }
}