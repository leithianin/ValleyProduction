using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_HideMesh : InteractionActions
{
    [SerializeField, Tooltip("If true, will disable the mesh renderer. If false, will activate the mesh renderer.")] private bool hideMesh = true;

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        caller.Handler.MeshRnd.enabled = !hideMesh;
    }

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        caller.Handler.MeshRnd.enabled = hideMesh;
        EndAction(caller);
    }
}
