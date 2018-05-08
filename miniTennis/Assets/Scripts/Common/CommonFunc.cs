using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonFunc
{
    public static T AddSingleComponent<T>(GameObject go)
            where T : Component
    {
        if (go == null)
        {
            return null;
        }

        T component = go.GetComponent<T>();
        if (component == null)
        {
            component = go.AddComponent<T>();
        }
        return component;
    }

    public static GameObject GetChild(GameObject gameObj, string name)
    {
        if (gameObj == null)
        {
            return null;
        }

        Transform transform = gameObj.transform;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                if (child.name == name)
                {
                    return child.gameObject;
                }


                GameObject go = GetChild(child.gameObject, name);
                if (go != null)
                {
                    return go;
                }
            }
        }
        return null;
    }

    public static void SetTransformZero(this GameObject gameObj)
    {
        if (gameObj != null)
        {
            gameObj.transform.position = Vector3.zero;
            gameObj.transform.rotation = Quaternion.identity;
            gameObj.transform.localScale = Vector3.one;
        }
    }

    public static GameObject Instantiate(GameObject temp)
    {
        GameObject go = GameObject.Instantiate(temp);
        go.name = temp.name;
        go.SetTransformZero();
        return go;
    }
}
