using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private CPN_Movement movement;

    public List<InterestPointType> possibleInteractions;

    public CPN_Movement Movement => movement;

    public void SetProfile(IInteractionProfile profile)
    {
        possibleInteractions = profile.GetInterests();
    }
}
