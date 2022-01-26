using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    [SerializeField] private InteractionHandler interaction;
    [SerializeField] private InteractionSequence sequence;
    [SerializeField] private Transform spawnPosition;

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
