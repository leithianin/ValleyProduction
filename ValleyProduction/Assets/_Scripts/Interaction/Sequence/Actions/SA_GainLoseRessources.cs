using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SA_GainLoseRessources : InteractionActions
{
    [SerializeField] private float amountToAdd;
    [SerializeField] private bool gainRessource = true;
    public UnityEvent OnGainMoney;
    public UnityEvent OnLoseMoney;


    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        if (gainRessource)
        {
            VLY_RessourceManager.GainRessource(amountToAdd);
            OnGainMoney?.Invoke();
        }
        else
        {
            VLY_RessourceManager.LoseRessource(amountToAdd);
            OnLoseMoney?.Invoke();
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
