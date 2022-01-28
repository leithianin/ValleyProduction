using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private CPN_Movement movement;

    private IInteractableData interactableDatas;

    public CPN_Movement Movement => movement;

    public bool IsInterested(InteractionType wantedType)
    {
        return interactableDatas.GetInteractionTypes().Contains(wantedType);
    }

    public void SetInteractableData(IInteractableData newDatas)
    {
        interactableDatas = newDatas;
    }
}
