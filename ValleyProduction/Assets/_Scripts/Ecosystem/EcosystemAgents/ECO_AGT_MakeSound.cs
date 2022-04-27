using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECO_AGT_MakeSound : EcosystemAgent<ECO_AGT_MakeSound>
{
    /// <summary>
    /// Ajoute NewData à la data actuelle.
    /// </summary>
    /// <param name="newData">Le Data à ajouter.</param>
    public override void SetData(ECO_AGT_MakeSound newData)
    {
        data = newData;
    }

    public override void SetData()
    {
        data = this;
    }

    public void SetNoiseScore(CPN_Data_Noise dataToSet)
    {
        SetScore(dataToSet.NoiseMade());
    }

    public void AddNoide(float toAdd)
    {
        SetScore(GetScore() + toAdd);
    }

    public void RemoveNoide(float toRemove)
    {
        SetScore(GetScore() - toRemove);
    }
}