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
            Destroy(this);
        }
        else
        {
            instance = this as T;
            Debug.Log(instance);
        }

        OnAwake();
    }

    protected virtual void OnAwake()
    {

    }
}
