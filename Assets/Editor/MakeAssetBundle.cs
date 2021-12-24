//Create a folder (right click in the Assets folder and go to Create>Folder), and name it “Editor” if it doesn’t already exist
//Place this script in the Editor folder

//This script creates a new Menu named “Build Asset” and new options within the menu named “Normal” and “Strict Mode”. Click these menu items to build an AssetBundle into a folder with either no extra build options, or a strict build.

using UnityEngine;
using UnityEditor;

public class MakeAssetBundle : MonoBehaviour
{
    //Creates a new menu (Build Asset Bundles) and item (Normal) in the Editor
    [MenuItem("Build Asset Bundles/Normal")]
    static void BuildItemAssetBundlesNone()
    {
        //Create a folder to put the Asset Bundle in.
        // This puts the bundles in your custom folder (this case it's "MyAssetBuilds") within the Assets folder.
        //Build AssetBundles with no special options
        BuildPipeline.BuildAssetBundles("Assets/Bundles/Items", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
    }

    //Creates a new item (Strict Mode) in the new Build Asset Bundles menu
    [MenuItem("Build Asset Bundles/Strict Mode ")]
    static void BuildItemAssetBundlesStrict()
    {
        //Build the AssetBundles in strict mode (build fails if any errors are detected)
        BuildPipeline.BuildAssetBundles("Assets/Bundles/Items", BuildAssetBundleOptions.StrictMode, BuildTarget.StandaloneWindows);
    }
}