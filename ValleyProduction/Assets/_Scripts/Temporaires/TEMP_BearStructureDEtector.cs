using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEMP_BearStructureDEtector : MonoBehaviour
{
    [SerializeField] private UnityEvent ToPlayOnValidate;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IST_BaseStructure>() != null)
        {
            ToPlayOnValidate?.Invoke();
        }
    }
}
