using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFind : MonoBehaviour
{
    private List<Point> lst = new List<Point>();
    private bool isOver = false;
    private string targetPointName;
    private Dictionary<string, Point> dic = new Dictionary<string, Point>();

    void Start()
    {
        dic.Add("1", new Point() { name = "1", nearPoints = "3" });
        dic.Add("2", new Point() { name = "2", nearPoints = "3" });
        dic.Add("3", new Point() { name = "3", nearPoints = "1_2_5_7" });
        dic.Add("4", new Point() { name = "4", nearPoints = "5" });
        dic.Add("5", new Point() { name = "5", nearPoints = "3_4_6" });
        dic.Add("6", new Point() { name = "6", nearPoints = "5" });
        dic.Add("7", new Point() { name = "7", nearPoints = "3_8" });
        dic.Add("8", new Point() { name = "8", nearPoints = "7" });

        targetPointName = "6";
        Calculate("7");
        GetParent(targetPointName);

        for (int i = lst.Count-1; i >= 0; i--)
        {
            Debug.Log(lst[i].name);
        }
    }

    private void Calculate(string currPointName)
    {
        if (!dic.ContainsKey(currPointName)) return;
        Point p = dic[currPointName];
        string[] arr = p.nearPoints.Split('_');
        for (int i = 0; i < arr.Length; i++)
        {
            if (isOver) continue;
            string pointName = arr[i];

            Point nearPoint = dic[pointName];
            if (nearPoint.isVisit) continue;

            nearPoint.isVisit = true;
            nearPoint.Parent = p;

            if (nearPoint.name.Equals(targetPointName))
            {
                isOver = true;
				Debug.Log("查找完毕");
                break;
            }
            else
            {
                Calculate(nearPoint.name);
            }
        }

    }

    private void GetParent(string name)
    {
        Point p = dic[name];
        lst.Add(p);
        if (p.Parent != null)
        {
            GetParent(p.Parent.name);
        }
    }
}

public class Point{
    //当前点的名称
    public string name;
    //附近的点
    public string nearPoints;
    //是否被访问
    public bool isVisit;
    //点的父亲
    public Point Parent;
}
