using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_Purchasable : VLY_Component<CPN_Data_Purchasable>
{
    [SerializeField] private float cost;

    [SerializeField] private UnityEvent OnBuy;
    [SerializeField] private UnityEvent OnSell;

    public float Cost => cost;

    public override void SetData(CPN_Data_Purchasable dataToSet)
    {
        cost = dataToSet.Cost();
    }

    public bool IsPurchasable()
    {
        return VLY_RessourceManager.GetRessource >= cost;
    }

    public void Buy()
    {
        VLY_RessourceManager.LoseRessource(cost);
    }

    public void Sell()
    {
        VLY_RessourceManager.GainRessource(cost);
    }

    public void TryToBuy()
    {
        if(IsPurchasable())
        {
            Buy();
        }
    }
}
