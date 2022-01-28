using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private void OnDisable()
    {
        sequence.InteruptAction(interaction);
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
