using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_Purchasable : VLY_Component<CPN_Data_Purchasable>
{
    [SerializeField] private float cost;

    public float Cost => cost;

    public override void SetData(CPN_Data_Purchasable dataToSet)
    {
        cost = dataToSet.Cost();
    }

    public bool IsPurchasable(float currentRessourceAmount)
    {
        return currentRessourceAmount >= cost;
    }

    public void Buy()
    {
        VLY_RessourceManager.LoseRessource(cost);
    }

    public void Sell()
    {
        VLY_RessourceManager.GainRessource(cost);
    }
}
