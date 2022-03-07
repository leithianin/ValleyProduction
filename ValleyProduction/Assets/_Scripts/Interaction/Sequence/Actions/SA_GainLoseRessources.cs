using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_GainLoseRessources : InteractionActions
{
    [SerializeField] private float amountToAdd;
    [SerializeField] private bool gainRessource = true;


    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        if (gainRessource)
        {
            VLY_RessourceManager.GainRessource(amountToAdd);
        }
        else
        {
            VLY_RessourceManager.LoseRessource(amountToAdd);
        }
        EndAction(caller);
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        
    }
}
