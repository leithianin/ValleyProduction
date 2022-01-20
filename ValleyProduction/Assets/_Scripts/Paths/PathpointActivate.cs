using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathpointActivate : MonoBehaviour
{
    public float timer;

    private void OnEnable()
    {
        StartCoroutine(Activate());
    }

    IEnumerator Activate()
    {
        yield return new WaitForSeconds(timer);
        gameObject.layer = 0;
    }
}
