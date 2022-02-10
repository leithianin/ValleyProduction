using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Visitor", menuName = "Visitor/Create Visitor")]
public class VisitorScriptable : ScriptableObject, CPN_Data_Stamina, CPN_Data_Movement, CPN_Data_Interaction, CPN_Data_Noise, CPN_Data_TrashThrower, CPN_Data_TrashPicker
{
    [SerializeField, Tooltip("A list of all available skin for the visitor. We take a random one for each visitor.")] private List<AnimationHandler> display;
    [SerializeField, Tooltip("The speed of the visitor.")] private float speed;
    [SerializeField, Tooltip("The list of types of Interaction the visitor is interested in.")] private List<InteractionType> interactionTypes;

    [Header("Stamina")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float slopeCoef;
    [SerializeField] private float staminaRegenCoef;

    [Header("Noise")]
    public float noiseMade;

    [Header("Pollution")]
    [SerializeField] private float throwRadius;
    [SerializeField] private Vector2 throwTimeRange;
    [SerializeField] private float pickupChance;
    [SerializeField] private float pickupRadius;

    /// <summary>
    /// Get a random skin for the visitor.
    /// </summary>
    public AnimationHandler Display => display[Random.Range(0, display.Count)];
    public float Speed => speed;

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

    public List<InteractionType> InteractionInterest()
    {
        return interactionTypes;
    }

    public float NoiseMade()
    {
        return noiseMade;
    }

    public float ThrowRadius()
    {
        return throwRadius;
    }

    public Vector2 ThrowTimeRange()
    {
        return throwTimeRange;
    }

    public float PickupChance()
    {
        return pickupChance;
    }

    public float PickingRadius()
    {
        return pickupRadius;
    }
}
