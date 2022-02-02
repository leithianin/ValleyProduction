using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AnimalBehavior : MonoBehaviour
{
    [SerializeField] private InteractionHandler interaction;
    private InteractionSequence sequence;
    [SerializeField] private Transform spawnPosition;

    [SerializeField] private UnityEvent OnSet;
    [SerializeField] private UnityEvent OnUnset;

    public void SetAnimal(InteractionSequence nSequence)
    {
        NavMeshHit hit;
        NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas);

        transform.position = hit.position;

        sequence = nSequence;
        gameObject.SetActive(true);
        OnSet?.Invoke();
    }

    public void UnsetAnimal()
    {
        sequence.InteruptAction(interaction);
        gameObject.SetActive(false);
        OnUnset?.Invoke();
    }

    private void OnEnable()
    {
        transform.position = spawnPosition.position;
        DoBehavior();
    }

    private void DoBehavior()
    {
        sequence.PlayAction(interaction, DoBehavior);
        //sequence.PlayAction(interaction, () => StartCoroutine(StartBehavior()));
    }

    IEnumerator StartBehavior()
    {
        yield return new WaitForSeconds(.1f);
        DoBehavior();
    }
}
