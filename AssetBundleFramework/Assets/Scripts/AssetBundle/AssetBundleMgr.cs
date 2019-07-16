using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
AssetBundle 资源管理器
 */
public class AssetBundleMgr : Singleton<AssetBundleMgr>
{

    private AssetBundleManifest manifest;

    private AssetBundle bundle;

	private List<AssetBundle> lst = new List<AssetBundle>();

    #region 同步加载

    public GameObject Load(string path, string name)
    {
        bundle = AssetBundle.LoadFromFile(LocalFileMgr.Instance.LocalFilePath + "Windows");

        manifest = bundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");

        string[] dps = manifest.GetAllDependencies(name);

        if (dps.Length > 0)
        {
            foreach (string str in dps)
            {
				Debug.Log("依赖 "+str);
				//AssetBundle.LoadFromMemory(LocalFileMgr.Instance.GetBuffer(str))
            }
        }

        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            return loader.LoadAsset<GameObject>(name);
        }
    }

    #endregion

    public GameObject LoadClone(string path, string name)
    {

        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            GameObject obj = loader.LoadAsset<GameObject>(name);
            return Object.Instantiate(obj);
        }
    }

    #region 异步加载

    public AssetBundleLoaderAsync LoadAsync(string path, string name)
    {
        GameObject obj = new GameObject("AssetBundleLoadAsync");
        AssetBundleLoaderAsync async = obj.AddComponent<AssetBundleLoaderAsync>();
        async.Init(path, name);
        return async;
    }

    #endregion
}
