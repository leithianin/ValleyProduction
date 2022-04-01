using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckColliderName : MonoBehaviour
{
    public string name;

    public void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(name))
        {
            OnBoardingManager.OnClickZoneTrashCan?.Invoke(true);
        }
    }
}
