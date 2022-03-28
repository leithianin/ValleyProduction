using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AnimalBehavior : MonoBehaviour
{
    [SerializeField] private CPN_InteractionHandler interaction;
    [SerializeField] private InteractionSequence sequence;
    [SerializeField] private Transform spawnPosition;

    [SerializeField] public GameObject display;

    [SerializeField] private UnityEvent OnSet;
    [SerializeField] private UnityEvent OnUnset;

    public void SetAnimal(InteractionSequence nSequence)
    {
        float yPosition = VisitorManager.GetMainTerrain.SampleHeight(transform.position) + VisitorManager.GetMainTerrain.transform.position.y;

        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);

        display.SetActive(true);
        OnSet?.Invoke();

        if (sequence == null)
        {
            sequence = nSequence;

            DoBehavior();
        }
    }

    public void UnsetAnimal()
    {
        display.SetActive(false);
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
            display.SetActive(false);
        }
    }

    private void DoBehavior()
    {
        sequence.PlayAction(interaction, DoBehavior, null);
    }

}
