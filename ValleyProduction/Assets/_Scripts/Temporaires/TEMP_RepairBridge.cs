using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEMP_RepairBridge : MonoBehaviour
{
    [SerializeField] private float cost;
    [SerializeField] private UnityEvent OnRepairBridge;

    private void OnMouseDown()
    {
        if(VLY_RessourceManager.GetRessource >= cost)
        {
            VLY_RessourceManager.LoseRessource(cost);
            OnRepairBridge?.Invoke();
        }
    }
}
