using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AnimalBehavior : MonoBehaviour
{
    [SerializeField] private CPN_InteractionHandler interaction;
    private InteractionSequence sequence;
    [SerializeField] private Transform spawnPosition;

    [SerializeField] private UnityEvent OnSet;
    [SerializeField] private UnityEvent OnUnset;

    public void SetAnimal(InteractionSequence nSequence)
    {
        float yPosition = VisitorManager.GetMainTerrain.SampleHeight(transform.position) + VisitorManager.GetMainTerrain.transform.position.y;

        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);

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
        if (sequence != null)
        {
            transform.position = spawnPosition.position;
            DoBehavior();
        }
        else
        {
            gameObject.SetActive(false);
        }
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
