using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_InstantiateVFX : MonoBehaviour, IFeedbackPlayer
{
    [SerializeField] private ParticleSystem particlePrefab;

    TimerManager.Timer timer = null;

    public void Play()
    {
        particlePrefab.playOnAwake = true;
        GameObject instantiatedObject = Instantiate(particlePrefab.gameObject, transform.position, Quaternion.identity);
        timer = TimerManager.CreateGameTimer(particlePrefab.main.duration, () => DespawnVfx(instantiatedObject));
        particlePrefab.playOnAwake = false;
    }

    private void DespawnVfx(GameObject toDespawn)
    {
        Destroy(toDespawn);
    }

    void OnDisable()
    {
        timer?.Stop();
    }
}
