using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Visitor", menuName = "Visitor/Create Visitor")]
public class VisitorScriptable : ScriptableObject, IInteractableData, CPN_Data_Stamina, CPN_Data_Movement
{
    [SerializeField, Tooltip("A list of all available skin for the visitor. We take a random one for each visitor.")] private List<AnimationHandler> display;
    [SerializeField, Tooltip("The speed of the visitor.")] private float speed;
    [SerializeField, Tooltip("The list of types of Interaction the visitor is interested in.")] private List<InteractionType> interactionTypes;

    [SerializeField] private float maxStamina;
    [SerializeField] private float slopeCoef;
    [SerializeField] private float staminaRegenCoef;

    public float noiseMade;

    /// <summary>
    /// Get a random skin for the visitor.
    /// </summary>
    public AnimationHandler Display => display[Random.Range(0, display.Count)];
    public float Speed => speed;

    public List<InteractionType> GetInteractionTypes()
    {
        return interactionTypes;
    }

    public float MaxStamina()
    {
        return maxStamina;
    }

    public float SlopeCoef()
    {
        return slopeCoef;
    }

    public float RegenCoef()
    {
        return staminaRegenCoef;
    }

    float CPN_Data_Movement.Speed()
    {
        return speed;
    }
}
