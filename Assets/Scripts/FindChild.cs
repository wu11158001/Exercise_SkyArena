using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FindChild
{
    public static T OnFindChild<T>(this Transform searchObject, string searchName) where T: Component
    {
        for (int i = 0; i < searchObject.childCount; i++)
        {
            if (searchObject.GetChild(i).childCount > 0)
            {
                var obj = searchObject.GetChild(i).OnFindChild<Transform>(searchName);
                if (obj != null) return obj.GetComponent<T>();
            }

            if (searchObject.GetChild(i).name == searchName)
            {
                return searchObject.GetChild(i).GetComponent<T>();
            }
        }

        return default;
    }
}
