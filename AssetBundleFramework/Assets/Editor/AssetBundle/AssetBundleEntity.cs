using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
AssetBundle 实体类
 */
public class AssetBundleEntity
{

    /**
	用于打包的时候选定的 唯一key
	 */
    public string Key;

    /**
	名称
	 */
    public string Name;

    /*
	标签
	 */
    public string Tag;

    /**
	是否是文件夹
	 */
    public bool IsFolder;

    /**
	是否是初始资源
	 */
    public bool IsFirstData;

	/**
	是否被选中
	 */
	public bool IsChecked;

    private List<string> m_PathList = new List<string>();

	/**
	路径集合
	 */
    public List<string> PathList
    {
        get { return m_PathList; }
    }

}
