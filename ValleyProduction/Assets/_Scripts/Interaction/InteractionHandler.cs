using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private CPN_Movement movement;

    public CPN_Movement Movement => movement;

    // Dégueulasse ? CODE REVIEW
    public Action<InteractionHandler> onMovementEnd;

    public void OnEndMovement()
    {
        onMovementEnd?.Invoke(this);
    }

    /*[SerializeField] private List<MonoBehaviour> interactionsTypes;

    public void GetInteractionElement<T>(ref T element) where T : MonoBehaviour
    {
        Debug.Log(element);
        for (int i = 0; i < interactionsTypes.Count; i++)
        {
            if (interactionsTypes[i] as T != null)
            {
                element = interactionsTypes[i] as T;
            }
        }
    }*/

    // Voir si on peut pas stocker une liste d'action directement au niveau des personnages

    /*  Sur les interractions
     *  - Liste d'action à faire par le personnage
     *  - Quand un personnage rentre dans la zone, il appel la fonction d'interaction avec la liste des actions à faire
     *  Sur les personnages
     *  - Fonction d'interraction : Récupère la liste d'action à faire
     * 
     * 
     */
}
