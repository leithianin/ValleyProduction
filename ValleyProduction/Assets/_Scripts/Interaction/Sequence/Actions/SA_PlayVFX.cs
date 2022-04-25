using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_PlayVFX : InteractionActions
{
    [SerializeField] private ParticleSystem particlePrefab;

    TimerManager.Timer timer = null;

    public void Play()
    {
        
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        timer?.Stop();
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        timer.Stop();
    }

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        particlePrefab.playOnAwake = true;
        GameObject instantiatedObject = Instantiate(particlePrefab.gameObject, transform.position, Quaternion.identity);
        timer = TimerManager.CreateGameTimer(particlePrefab.main.duration, () => DespawnVfx(instantiatedObject, caller));
        particlePrefab.playOnAwake = false;
    }

    private void DespawnVfx(GameObject toDespawn, CPN_InteractionHandler caller)
    {
        EndAction(caller);

        Destroy(toDespawn);
    }
}
