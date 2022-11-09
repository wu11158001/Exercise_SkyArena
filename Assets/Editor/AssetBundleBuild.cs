using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/// <summary>
/// AssetBundle«Ø¸m
/// </summary>
public class AssetBundleBuild
{
    [MenuItem("MyTool/AssetBundleBuild")]
    static void OnAssetBundleBuild()
    {
        string folder = Application.streamingAssetsPath + "/MyAssetBundle";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        BuildPipeline.BuildAssetBundles(folder, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }
}
