using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVisitor", menuName = "Valley/Visitor")]
public class VisitorScriptable : ScriptableObject, CPN_Data_Stamina, CPN_Data_Movement, CPN_Data_Interaction, CPN_Data_SatisfactionHandler, CPN_Data_Noise, CPN_Data_TrashThrower, CPN_Data_TrashPicker
{
    [SerializeField, Tooltip("A list of all available skin for the visitor. We take a random one for each visitor.")] private List<AnimationHandler> display;
    [SerializeField, Tooltip("The speed of the visitor.")] private Vector2 speed;
    [SerializeField, Tooltip("List of all the Landmark by interest.")] private List<LandmarkType> landmarksWanted;
    [SerializeField, Tooltip("The list of types of Interaction the visitor is interested in.")] private List<BuildTypes> interactionTypes;
    [SerializeField, Tooltip("The list of types of Interaction the visitor is interested in.")] private List<BuildTypes> likedInterestType;
    [SerializeField, Tooltip("The list of types of Interaction the visitor is interested in.")] private List<BuildTypes> hatedInterestType;

    [Header("Stamina")]
    [SerializeField, Tooltip("Limite de stamina.")] private float maxStamina;
    [SerializeField, Tooltip("Degré d'influence d'une pente.")] private float slopeCoef;
    [SerializeField, Tooltip("Degré de récupération de stamina.")] private float staminaRegenCoef;

    [Header("Noise")]
    [Tooltip("Bruit émit par le visiteur.")] public float noiseMade;

    [Header("Pollution")]
    [SerializeField, Tooltip("Rayon dans lequel le visiteur va lancer son déchet.")] private float throwRadius;
    [SerializeField, Tooltip("Délai minimum/maximum entre chaque jeté de déchet.")] private Vector2 throwTimeRange;
    [SerializeField, Tooltip("Chance de ramasser un déchet qu'il croise.")] private float pickupChance;
    [SerializeField, Tooltip("Rayon dans lequel le visiteur va ramasser un déchet.")] private float pickupRadius;

    /// <summary>
    /// Get a random skin for the visitor.
    /// </summary>
    public AnimationHandler Display => display[Random.Range(0, display.Count)];
    public Vector2 Speed => speed;

    public List<LandmarkType> LandmarksWanted => landmarksWanted;

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

    Vector2 CPN_Data_Movement.Speed()
    {
        return speed;
    }

    public List<BuildTypes> InteractionInterest()
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

    public List<BuildTypes> LikedInteractions()
    {
        return likedInterestType;
    }

    public List<BuildTypes> HatedInteractions()
    {
        return hatedInterestType;
    }
}
