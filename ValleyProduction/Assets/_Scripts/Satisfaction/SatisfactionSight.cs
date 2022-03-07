using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatisfactionSight : MonoBehaviour
{
    [SerializeField] private CPN_SatisfactionHandler satisfactionHandler;

    private void OnTriggerEnter(Collider other)
    {
        CPN_SatisfactionGiver otherGiver = other.GetComponent<CPN_SatisfactionGiver>();
        if (otherGiver != null)
        {
            satisfactionHandler.AddSatisfaction(otherGiver.SatisfactionGiven * satisfactionHandler.GetAppreciationLevel(otherGiver.SatisfactionGiverType));
        }
    }
}
