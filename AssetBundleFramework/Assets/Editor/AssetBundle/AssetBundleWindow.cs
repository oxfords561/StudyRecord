using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text;

public class AssetBundleWindow : EditorWindow
{

    private AssetBundleDAL dal;
    private List<AssetBundleEntity> m_List;
    private Dictionary<string, bool> m_Dic;
    private int tagIndex = 0;//标记的索引
    private int selectTagIndex = -1;//选择的标记索引
    private string[] arrTag = { "All", "Scene", "Role", "Effect", "Audio", "UI", "None" };

    private string[] arrBuildTarget = { "Windows", "Android", "iOS" };
    private int selectBuildTargetIndex = -1; //选择的打包平台索引
#if UNITY_STANDALONE_WIN
    private BuildTarget target = BuildTarget.StandaloneWindows;
    private int buildTargetIndex = 0; //打包的平台索引
#elif UNITY_ANDROID
    private BuildTarget target = BuildTarget.Android;
    private int buildTargetIndex = 1;
#elif UNITY_IPHONE
    private BuildTarget target = BuildTarget.iOS;
    private int buildTargetIndex = 2;
#endif

    private Vector2 pos;

    void OnEnable()
    {
        //初始化的地方
        string xmlPath = Application.dataPath + @"\Editor\AssetBundle\AssetBundleConfig.xml";
        dal = new AssetBundleDAL(xmlPath);
        m_List = dal.GetList();

        m_Dic = new Dictionary<string, bool>();
        for (int i = 0; i < m_List.Count; i++)
        {
            m_Dic[m_List[i].Key] = true;
        }
    }

    /// <summary>
    /// 绘制窗口
    /// </summary>
    void OnGUI()
    {
        if (m_List == null) return;

        #region 按钮行

        GUILayout.BeginHorizontal("box");

        //下拉列表 选择需要打包的分类
        selectTagIndex = EditorGUILayout.Popup(tagIndex, arrTag, GUILayout.Width(100));
        if (selectTagIndex != tagIndex)
        {
            tagIndex = selectTagIndex;
            EditorApplication.delayCall = OnSelectTagCallBack;
        }

        //下拉列表 选择打包的平台
        selectBuildTargetIndex = EditorGUILayout.Popup(buildTargetIndex, arrBuildTarget, GUILayout.Width(100));
        if (selectBuildTargetIndex != buildTargetIndex)
        {
            buildTargetIndex = selectBuildTargetIndex;
            EditorApplication.delayCall = OnSelectBuildTargetCallBack;
        }

        if (GUILayout.Button("保存设置", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnSaveAssetBundleCallBack;
        }

        if (GUILayout.Button("清空AssetBundle包", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnClearAssetBundleCallBack;
        }

        if (GUILayout.Button("打AssetBundle包", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnAssetBundleCallBack;
        }

        if (GUILayout.Button("拷贝数据表", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnCopyDataTableCallBack;
        }

        if (GUILayout.Button("生成版本文件", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnCreateVersionFileCallBack;
        }

        EditorGUILayout.Space();

        GUILayout.EndHorizontal();
        #endregion

        GUILayout.BeginHorizontal("box");

        GUILayout.Label("包名");
        GUILayout.Label("标记");
        GUILayout.Label("文件夹");
        GUILayout.Label("初始资源");

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        pos = EditorGUILayout.BeginScrollView(pos);

        for (int i = 0; i < m_List.Count; i++)
        {
            AssetBundleEntity entity = m_List[i];

            GUILayout.BeginHorizontal("box");

            m_Dic[entity.Key] = GUILayout.Toggle(m_Dic[entity.Key], "", GUILayout.Width(20));
            GUILayout.Label(entity.Name, GUILayout.Width(270));
            GUILayout.Label(entity.Tag, GUILayout.Width(300));
            GUILayout.Label(entity.IsFolder.ToString(), GUILayout.Width(310));
            GUILayout.Label(entity.IsFirstData.ToString());

            GUILayout.EndHorizontal();

            foreach (string path in entity.PathList)
            {
                GUILayout.BeginHorizontal("box");

                GUILayout.Space(40);
                GUILayout.Label(path);

                GUILayout.EndHorizontal();
            }
        }

        EditorGUILayout.EndScrollView();


        GUILayout.EndVertical();
    }

    /**
	选择打包的资源分类
	 */
    private void OnSelectTagCallBack()
    {
        switch (tagIndex)
        {
            case 0://All

                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = true;
                }

                break;
            case 1://Scene

                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = entity.Tag.Equals("Scene", StringComparison.CurrentCultureIgnoreCase);
                }

                break;
            case 2://Role

                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = entity.Tag.Equals("Role", StringComparison.CurrentCultureIgnoreCase);
                }

                break;
            case 3://Effect

                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = entity.Tag.Equals("Effect", StringComparison.CurrentCultureIgnoreCase);
                }

                break;
            case 4://Audio

                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = entity.Tag.Equals("Audio", StringComparison.CurrentCultureIgnoreCase);
                }

                break;
            case 5: //UI
                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = entity.Tag.Equals("UI", StringComparison.CurrentCultureIgnoreCase);
                }
                break;
            case 6: //None
                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = false;
                }
                break;
        }
        Debug.LogFormat("当前选择的Tag：{0}", arrTag[tagIndex]);
    }

    /**
	选择打包的平台
	 */
    private void OnSelectBuildTargetCallBack()
    {
        switch (buildTargetIndex)
        {
            case 0: //Windows
                target = BuildTarget.StandaloneWindows;
                break;
            case 1: //Android
                target = BuildTarget.Android;
                break;
            case 2: //iOS
                target = BuildTarget.iOS;
                break;
        }
        Debug.LogFormat("当前选择的BuildTarget：{0}", arrBuildTarget[buildTargetIndex]);
    }

    /**
	保存设置按钮点击
	 */
    private void OnSaveAssetBundleCallBack()
    {
        //挑选出需要打包的对象
        List<AssetBundleEntity> lst = new List<AssetBundleEntity>();

        //筛选面板上的资源是否被选中
        foreach (AssetBundleEntity entity in m_List)
        {
            if (m_Dic[entity.Key])
            {
                entity.IsChecked = true;
                lst.Add(entity);
            }
            else
            {
                entity.IsChecked = false;
                lst.Add(entity);
            }
        }

        //循环设置文件夹包括文件里面的 assetbundle 路径名称
        for (int i = 0; i < lst.Count; i++)
        {
            AssetBundleEntity entity = lst[i];
            if (entity.IsFolder)
            {
                //如果当前节点配置是一个文件夹的时候 循环遍历文件夹
                //需要将相对路径变成绝对路径(因为 xml 文件中配置的是相对路径)
                string[] folderArr = new string[entity.PathList.Count];
                for (int j = 0; j < entity.PathList.Count; j++)
                {
                    folderArr[j] = Application.dataPath + "/" + entity.PathList[j];
                }
                SaveFolderSettings(folderArr, !entity.IsChecked);
            }
            else
            {
                //如果不是文件夹 则只需要设置 Assetbundle 路径命名
                string[] folderArr = new string[entity.PathList.Count];
                for (int j = 0; j < entity.PathList.Count; j++)
                {
                    folderArr[j] = Application.dataPath + "/" + entity.PathList[j];
                    SaveFileSetting(folderArr[j], !entity.IsChecked);
                }
            }
        }

        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.Refresh();
        Debug.Log("保存设置完毕");
    }


    private void SaveFolderSettings(string[] folderArr, bool isSetNull)
    {
        foreach (string folderPath in folderArr)
        {
            //1、先看这个文件夹下的文件
            string[] arrFile = Directory.GetFiles(folderPath);//文件夹下的文件

            //2、对文件进行设置
            foreach (string filePath in arrFile)
            {
                //进行文件设置
                SaveFileSetting(filePath, isSetNull);
            }

            //3、看这个文件夹下的子文件夹
            string[] arrFolder = Directory.GetDirectories(folderPath);
            SaveFolderSettings(arrFolder, isSetNull);
        }
    }

    private void SaveFileSetting(string filePath, bool isSetNull)
    {
        FileInfo file = new FileInfo(filePath);
        //过滤 .meta 文件
        if (!file.Extension.Equals(".meta", StringComparison.InvariantCultureIgnoreCase))
        {
            int index = filePath.IndexOf("Assets/", StringComparison.CurrentCultureIgnoreCase);

            //获取文件的短路径
            string newPath = filePath.Substring(index);
            // Debug.Log("newPath" + newPath);

            //文件名 去掉扩展名
            string fileName = newPath.Replace("Assets/", "").Replace(file.Extension, "");

            //后缀
            string variant = file.Extension.Equals(".unity", StringComparison.CurrentCultureIgnoreCase) ? "unity3d" : "assetbundle";

            AssetImporter import = AssetImporter.GetAtPath(newPath);

            if (isSetNull)
            {
                import.SetAssetBundleNameAndVariant(null, null);
            }
            else
            {
                import.SetAssetBundleNameAndVariant(fileName, variant);
            }

            import.SaveAndReimport();

        }
    }

    /**
	清空AssetBundle 按钮点击
	 */
    private void OnClearAssetBundleCallBack()
    {
        string path = Application.dataPath + "/../AssetBundles/" + arrBuildTarget[buildTargetIndex];
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        Debug.Log("清空完毕");
    }

    /**
	打包AssetBundle 按钮点击
 	*/
    private void OnAssetBundleCallBack()
    {
        //设置打包的路径
        string toPath = Application.dataPath + "/../AssetBundles/" + arrBuildTarget[buildTargetIndex];
        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }

        //打包的方法就一句话
        BuildPipeline.BuildAssetBundles(toPath, BuildAssetBundleOptions.None, target);
        Debug.Log("打包完毕");
    }

    /**
	拷贝数据表按钮点击
	 */
    private void OnCopyDataTableCallBack()
    {

    }

    /**
	生成版本文件按钮点击
	 */
    private void OnCreateVersionFileCallBack()
    {
        string path = Application.dataPath + "/../AssetBundles/" + arrBuildTarget[buildTargetIndex];
        if (!Directory.Exists(path)) return;

        string strVersionFilePath = path + "/VersionFile.txt";//版本文件路径

        IOUtil.DeleteFile(strVersionFilePath);//如果版本文件存在则删除 重新创建

        StringBuilder sbContent = new StringBuilder();

        DirectoryInfo directory = new DirectoryInfo(path);

        //拿到文件夹下的所有文件
        FileInfo[] arrFiles = directory.GetFiles("*", SearchOption.AllDirectories);

        for (int i = 0; i < arrFiles.Length; i++)
        {
            FileInfo file = arrFiles[i];
            string fullName = file.FullName;//文件的全名 包含路径扩展名

            //相对路径
            string name = fullName.Substring(fullName.IndexOf(arrBuildTarget[buildTargetIndex]) + arrBuildTarget[buildTargetIndex].Length + 1);

            if (name.Equals(arrBuildTarget[buildTargetIndex], StringComparison.CurrentCultureIgnoreCase))
            {
                continue;
            }

            string md5 = EncryptUtil.GetFileMD5(fullName);//获取文件的 MD5
            if (md5 == null) continue;

            string size = Math.Ceiling(file.Length / 1024f).ToString();//文件大小

            bool isFirstData = true;//是否是初始资源
            bool isBreak = false;

            for (int j = 0; j < m_List.Count; j++)
            {
                foreach (string xmlPath in m_List[j].PathList)
                {
                    string tempPath = xmlPath;
                    //是个文件
                    if (xmlPath.IndexOf(".") != -1)
                    {
                        tempPath = xmlPath.Substring(0, xmlPath.IndexOf("."));
                    }
                    if (name.IndexOf(tempPath, StringComparison.CurrentCultureIgnoreCase) != -1)
                    {
                        isFirstData = m_List[j].IsFirstData;
                        isBreak = true;
                        break;
                    }
                }
                if (isBreak) break;
            }

            if (name.IndexOf("DataTable") != -1)
            {
                isFirstData = true;
            }

            string strLine = string.Format("{0} {1} {2} {3}", name, md5, size, isFirstData ? 1 : 0);
            sbContent.AppendLine(strLine);
        }

		IOUtil.CreateTextFile(strVersionFilePath,sbContent.ToString());
		Debug.Log("创建版本文件成功");
    }

}
