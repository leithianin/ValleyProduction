using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PollutionTrash trash = other.GetComponent<PollutionTrash>();
        if (trash != null)
        {
            Debug.Log("Trash remove");
            trash.PickUp();
        }
    }
}
