using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_InstantiateVFX : MonoBehaviour, IFeedbackPlayer
{
    [SerializeField] private ParticleSystem particlePrefab;

    public void Play()
    {
        particlePrefab.playOnAwake = true;
        GameObject instantiatedObject = Instantiate(particlePrefab.gameObject, transform.position, Quaternion.identity);
        TimerManager.CreateTimer(particlePrefab.main.duration, () => DespawnVfx(instantiatedObject));
        particlePrefab.playOnAwake = false;
    }

    private void DespawnVfx(GameObject toDespawn)
    {
        Destroy(toDespawn);
    }
}
