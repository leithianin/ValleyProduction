using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_HideCaller : InteractionActions
{
    [SerializeField, Tooltip("If true, will disable the mesh renderer. If false, will activate the mesh renderer.")] private bool showCaller = true;

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        caller.Handler.gameObject.SetActive(!showCaller);
    }

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        caller.Handler.gameObject.SetActive(showCaller);
        TimerManager.CreateGameTimer(Time.deltaTime, () => EndAction(caller));
    }
}
