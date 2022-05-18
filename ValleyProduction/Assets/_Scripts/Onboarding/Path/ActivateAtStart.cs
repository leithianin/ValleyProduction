using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAtStart : MonoBehaviour
{
    public GameObject go;

    public void Start()
    {
        go.SetActive(true);
    }
}
