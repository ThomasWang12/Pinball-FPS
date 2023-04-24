using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Methods
{
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
