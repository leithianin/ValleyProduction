using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontLeaveThisCollider : MonoBehaviour
{
    public Collider collider;

    private Vector3 lastPos;

    private void Update()
    {
        if(collider.bounds.Contains(transform.position))
        {
            lastPos = transform.position;
        }
        else
        {
            GetComponent<InfrastructurePreviewHandler>().snaping = true;
            transform.position = lastPos;
        }
    }
}
