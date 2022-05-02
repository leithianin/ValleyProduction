using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVisitor", menuName = "Valley/Visitor")]
public class VisitorScriptable : ScriptableObject, CPN_Data_Stamina, CPN_Data_Movement, CPN_Data_Interaction, CPN_Data_SatisfactionHandler, CPN_Data_Noise, CPN_Data_TrashThrower, CPN_Data_TrashPicker
{
    [Header("General")]
    [SerializeField] private Sprite icon;
    [SerializeField] private string nom;
    [SerializeField, Tooltip("A list of all available skin for the visitor. We take a random one for each visitor.")] private List<AnimationHandler> display;

    [Header("Movement")]
    [SerializeField, Tooltip("The speed of the visitor.")] private Vector2 speed;

    [Header("Interest")]
    [SerializeField, Tooltip("List of all the Landmark by interest.")] private List<LandmarkType> landmarksWanted;
    [SerializeField, Tooltip("The list of types of Interaction the visitor is interested in.")] private List<SatisfactorType> interactionTypes;
    [SerializeField, Tooltip("The list of types of Interaction the visitor is interested in.")] private List<SatisfactorType> likedInterestType;
    [SerializeField, Tooltip("The list of types of Interaction the visitor is interested in.")] private List<SatisfactorType> hatedInterestType;

    [Header("Stamina")]
    [SerializeField, Tooltip("Limite de stamina.")] private float maxStamina;
    [SerializeField, Tooltip("Degr� d'influence d'une pente.")] private float slopeCoef;
    [SerializeField, Tooltip("Degr� de r�cup�ration de stamina.")] private float staminaRegenCoef;

    [Header("Noise")]
    [Tooltip("Bruit �mit par le visiteur.")] public float noiseMade;

    [Header("Pollution")]
    [SerializeField, Tooltip("Rayon dans lequel le visiteur va lancer son d�chet.")] private float throwRadius;
    [SerializeField, Tooltip("D�lai minimum/maximum entre chaque jet� de d�chet.")] private Vector2 throwTimeRange;
    [SerializeField, Tooltip("Chance de ramasser un d�chet qu'il croise.")] private float pickupChance;
    [SerializeField, Tooltip("Rayon dans lequel le visiteur va ramasser un d�chet.")] private float pickupRadius;

    public Sprite Icon => icon;

    public string Nom => nom;

    /// <summary>
    /// Get a random skin for the visitor.
    /// </summary>
    public AnimationHandler Display => display[Random.Range(0, display.Count)];
    public Vector2 Speed => speed;

    public float GetThrowRadius => throwRadius;
    public float GetMaxStamina => maxStamina;

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

    public List<SatisfactorType> InteractionInterest()
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

    public List<SatisfactorType> LikedInteractions()
    {
        return likedInterestType;
    }

    public List<SatisfactorType> HatedInteractions()
    {
        return hatedInterestType;
    }
}
