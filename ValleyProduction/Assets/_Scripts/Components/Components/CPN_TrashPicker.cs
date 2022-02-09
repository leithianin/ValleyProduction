using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_TrashPicker : VLY_Component<CPN_Data_TrashPicker>
{
    [SerializeField] private SphereCollider pickingDetector;

    private float pickupChance;

    public override void SetData(CPN_Data_TrashPicker dataToSet)
    {
        if (dataToSet.PickupChance() <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            pickingDetector.radius = dataToSet.PickingRadius();
            pickupChance = dataToSet.PickupChance();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PollutionTrash trash = other.GetComponent<PollutionTrash>();
        if(trash != null && DoesPickUp())
        {
            trash.PickUp();
        }
    }

    private bool DoesPickUp()
    {
        return Random.Range(0f, 1f) <= pickupChance;
    }
}
