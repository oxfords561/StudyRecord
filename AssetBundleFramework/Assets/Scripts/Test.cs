using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //同步加载
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject obj = AssetBundleMgr.Instance.Load("Download/Prefab/RolePrefab/Player/Player.assetbundle", "Player");
            Instantiate(obj);
        }

        //异步加载
        if (Input.GetKeyDown(KeyCode.B))
        {
            AssetBundleLoaderAsync async = AssetBundleMgr.Instance.LoadAsync("Download/Prefab/RolePrefab/Player/Player.assetbundle", "Player");
            async.OnLoadComplete = (UnityEngine.Object obj) =>
            {
                Instantiate(obj);
            };
        }
    }
}
