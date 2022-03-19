using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_Singleton<T> : MonoBehaviour where T : VLY_Singleton<T>
{
    protected static T instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log(gameObject);
            Destroy(this);
        }
        else
        {
            instance = this as T;
        }
    }
}
