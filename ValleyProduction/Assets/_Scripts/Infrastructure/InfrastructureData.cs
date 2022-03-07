using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Structure", menuName ="Infrastructure/Create New Structure Data")]
public class InfrastructureData : ScriptableObject, CPN_Data_Purchasable
{
    [SerializeField] private string structureName;
    [SerializeField] private string structureDescription;

    [SerializeField] private float price;
    [SerializeField] private InfrastructureType structureType;

    public string Name => structureName;

    public InfrastructureType StructureType => structureType;

    public float Cost()
    {
        return price;
    }
}
