using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Structure", menuName ="Valley/Infrastructure/Create New Structure Data")]
public class InfrastructureData : ScriptableObject, CPN_Data_Purchasable
{
    [SerializeField] private string structureName;
    [SerializeField, TextArea(2,2)] private string structureDescription;
    [SerializeField] private Sprite rewardIcon;
    [SerializeField] private Sprite buttonIcon;
    [SerializeField] private Sprite notInteractibleIcon;
    [SerializeField] private Sprite exempleImage;

    [SerializeField] private float price;
    [SerializeField] private InfrastructureType structureType;

    [SerializeField] private Infrastructure structure;
    [SerializeField] private InfrastructurePreview preview;
    

    public string Name => structureName;

    public string Description => structureDescription;
    public Sprite RewardIcon => rewardIcon;
    public Sprite ButtonIcon => buttonIcon;
    public Sprite NotInteractibleIcon => notInteractibleIcon;
    public Sprite ExempleImage => exempleImage;

    public InfrastructureType StructureType => structureType;

    public Infrastructure Structure => structure;

    public InfrastructurePreview Preview => preview;

    public float Cost()
    {
        return price;
    }
}
