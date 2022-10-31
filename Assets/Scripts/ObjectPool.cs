using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����
/// </summary>
public class ObjectPool
{
    static ObjectPool objectPool;
    public static ObjectPool Instance => objectPool;

    List<List<TemporaryObject>> allObjectPool_List = new List<List<TemporaryObject>>();//�Ҧ����������
    List<GameObject> objectType_List = new List<GameObject>();//�������        

    /// <summary>
    /// �غc�l
    /// </summary>    
    public ObjectPool()
    {
        objectPool = this;
    }

    /// <summary>
    /// �ЫػP��������
    /// </summary>
    /// <param name="obj">����</param>
    /// <param name="parent">������e��</param>
    public int OnCreateAndRecordObject(GameObject obj, Transform parent)
    {
        if (obj == null)
        {
            Debug.LogError("�S������");
            return default;
        }

        //�Ыت���
        TemporaryObject temporaryObject = new TemporaryObject();
        temporaryObject.obj = GameObject.Instantiate(obj);
        temporaryObject.obj.SetActive(false);
        temporaryObject.obj.transform.SetParent(parent);

        //����
        objectType_List.Add(temporaryObject.obj);
        List<TemporaryObject> temporary = new List<TemporaryObject>();
        temporary.Add(temporaryObject);
        allObjectPool_List.Add(temporary);

        return allObjectPool_List.Count - 1;//�^�Ǫ���s��
    }

    /// <summary>
    /// �E������
    /// </summary>
    /// <param name="number">����s��</param>
    /// <returns></returns>
    public GameObject OnActiveObject(int number)
    {
        //���b
        if (number < 0 || number > allObjectPool_List.Count)
        {
            Debug.LogError("�s�����~!");
            return null;
        }

        List<TemporaryObject> temporary = allObjectPool_List[number];//���X����List

        //�E������
        for (int i = 0; i < temporary.Count; i++)
        {
            if (!temporary[i].obj.activeSelf)
            {
                temporary[i].obj.SetActive(true);
                return temporary[i].obj;
            }
        }

        //�W�L�ƶq�ƻs����
        TemporaryObject copy = new TemporaryObject();
        copy.obj = GameObject.Instantiate(objectType_List[number]);
        copy.obj.SetActive(true);
        allObjectPool_List[number].Add(copy);
        return copy.obj;
    }

    /// <summary>
    /// �M������
    /// </summary>
    /// <param name="number"></param>
    public void OnCleanObject(int number)
    {
        //���b
        if (number < 0 || number > allObjectPool_List.Count)
        {
            Debug.LogError("�s�����~!");
            return;
        }

        List<TemporaryObject> temporary_List = allObjectPool_List[number];//���X����List
        
        //��������
        temporary_List[0].obj.SetActive(false);
        for (int i = 1; i < temporary_List.Count; i++)
        {
            GameObject.Destroy(temporary_List[i].obj);                        
        }

        //���s��������
        TemporaryObject temporary = new TemporaryObject();
        temporary.obj = temporary_List[0].obj;
        List<TemporaryObject> record_Lists = new List<TemporaryObject>();
        record_Lists.Add(temporary);

        allObjectPool_List[number] = record_Lists;
    }
}

/// <summary>
/// �Ȧs����Class
/// </summary>
class TemporaryObject
{
    public GameObject obj;
}
