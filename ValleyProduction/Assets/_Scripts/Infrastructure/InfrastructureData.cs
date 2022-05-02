using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Structure", menuName ="Infrastructure/Create New Structure Data")]
public class InfrastructureData : ScriptableObject, CPN_Data_Purchasable
{
    [SerializeField] private string structureName;
    [SerializeField] private string structureDescription;
    [SerializeField] private Sprite icon;

    [SerializeField] private float price;
    [SerializeField] private InfrastructureType structureType;

    [SerializeField] private Infrastructure structure;
    [SerializeField] private InfrastructurePreview preview;

    [SerializeField] private GameObject logo;
    

    public string Name => structureName;

    public Sprite Icon => icon;

    public InfrastructureType StructureType => structureType;

    public Infrastructure Structure => structure;

    public InfrastructurePreview Preview => preview;

    public GameObject Logo => logo;

    public float Cost()
    {
        return price;
    }
}
