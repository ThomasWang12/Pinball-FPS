using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Methods
{
    public static GameObject GetChildContainsName(GameObject go, string name)
    {
        foreach (Transform tran in go.transform)
        {
            if (tran.gameObject.name.Contains(name))
                return tran.gameObject;
        }
        return null;
    }

    public static void GetChildRecursive(GameObject obj, List<GameObject> listOfChildren, string findName)
    {
        if (null == obj) return;

        foreach (Transform child in obj.transform)
        {
            if (null == child) continue;
            if (findName == "") listOfChildren.Add(child.gameObject);
            else if (child.gameObject.name.Contains(findName)) listOfChildren.Add(child.gameObject);
            GetChildRecursive(child.gameObject, listOfChildren, findName);
        }
    }
}
