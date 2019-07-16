using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Menu
{

    [MenuItem("Tools/AssetBundleCreate")]
    public static void AssetBundleCreate()
    {
        AssetBundleWindow win = EditorWindow.GetWindow<AssetBundleWindow>();
        win.titleContent = new GUIContent("资源打包");
        win.Show();
    }

}
