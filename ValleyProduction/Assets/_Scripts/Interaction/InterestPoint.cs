using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPoint : MonoBehaviour
{
    [SerializeField] private VisitorInteraction[] interactions;

    public void Interact(VisitorBehavior visitor)
    {
        List<VisitorInteraction> usableInteractions = GetUsableInteractions();

        if(usableInteractions.Count > 0)
        {
            interactions[Random.Range(0, usableInteractions.Count)].Interact(visitor);
        }
    }

    private List<VisitorInteraction> GetUsableInteractions()
    {
        List<VisitorInteraction> usableInteractions = new List<VisitorInteraction>();

        for(int i = 0; i < interactions.Length; i++)
        {
            if(interactions[i].IsUsable())
            {
                usableInteractions.Add(interactions[i]);
            }
        }

        return usableInteractions;
    }
}
