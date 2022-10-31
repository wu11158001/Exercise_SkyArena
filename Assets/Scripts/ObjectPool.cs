using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物件池
/// </summary>
public class ObjectPool
{
    static ObjectPool objectPool;
    public static ObjectPool Instance => objectPool;

    List<List<TemporaryObject>> allObjectPool_List = new List<List<TemporaryObject>>();//所有物件池物件
    List<GameObject> objectType_List = new List<GameObject>();//物件種類        

    /// <summary>
    /// 建構子
    /// </summary>    
    public ObjectPool()
    {
        objectPool = this;
    }

    /// <summary>
    /// 創建與紀錄物件
    /// </summary>
    /// <param name="obj">物件</param>
    /// <param name="parent">物件池容器</param>
    public int OnCreateAndRecordObject(GameObject obj, Transform parent)
    {
        if (obj == null)
        {
            Debug.LogError("沒有物件");
            return default;
        }

        //創建物件
        TemporaryObject temporaryObject = new TemporaryObject();
        temporaryObject.obj = GameObject.Instantiate(obj);
        temporaryObject.obj.SetActive(false);
        temporaryObject.obj.transform.SetParent(parent);

        //紀錄
        objectType_List.Add(temporaryObject.obj);
        List<TemporaryObject> temporary = new List<TemporaryObject>();
        temporary.Add(temporaryObject);
        allObjectPool_List.Add(temporary);

        return allObjectPool_List.Count - 1;//回傳物件編號
    }

    /// <summary>
    /// 激活物件
    /// </summary>
    /// <param name="number">物件編號</param>
    /// <returns></returns>
    public GameObject OnActiveObject(int number)
    {
        //防呆
        if (number < 0 || number > allObjectPool_List.Count)
        {
            Debug.LogError("編號錯誤!");
            return null;
        }

        List<TemporaryObject> temporary = allObjectPool_List[number];//取出物件List

        //激活物件
        for (int i = 0; i < temporary.Count; i++)
        {
            if (!temporary[i].obj.activeSelf)
            {
                temporary[i].obj.SetActive(true);
                return temporary[i].obj;
            }
        }

        //超過數量複製物件
        TemporaryObject copy = new TemporaryObject();
        copy.obj = GameObject.Instantiate(objectType_List[number]);
        copy.obj.SetActive(true);
        allObjectPool_List[number].Add(copy);
        return copy.obj;
    }

    /// <summary>
    /// 清除物件
    /// </summary>
    /// <param name="number"></param>
    public void OnCleanObject(int number)
    {
        //防呆
        if (number < 0 || number > allObjectPool_List.Count)
        {
            Debug.LogError("編號錯誤!");
            return;
        }

        List<TemporaryObject> temporary_List = allObjectPool_List[number];//取出物件List
        
        //移除物件
        temporary_List[0].obj.SetActive(false);
        for (int i = 1; i < temporary_List.Count; i++)
        {
            GameObject.Destroy(temporary_List[i].obj);                        
        }

        //重新紀錄物件
        TemporaryObject temporary = new TemporaryObject();
        temporary.obj = temporary_List[0].obj;
        List<TemporaryObject> record_Lists = new List<TemporaryObject>();
        record_Lists.Add(temporary);

        allObjectPool_List[number] = record_Lists;
    }
}

/// <summary>
/// 暫存物件Class
/// </summary>
class TemporaryObject
{
    public GameObject obj;
}
