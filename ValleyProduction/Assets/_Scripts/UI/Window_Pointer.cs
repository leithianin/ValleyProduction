using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Pointer : MonoBehaviour
{
    public GameObject target;
    public GameObject objectToInstantiate;

    RectTransform rt;

    void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    void Update()
    {
        
    }
}
