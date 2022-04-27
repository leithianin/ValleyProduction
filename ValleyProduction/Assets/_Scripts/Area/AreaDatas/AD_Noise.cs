using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class AD_Noise : AreaData<ECO_AGT_MakeSound>
{
    /// <summary>
    /// NOTES :
    /// T est de type NoiseMaker.
    /// NoiseMaker est un script contenant la data relative au score du son.
    /// Lors de la création de nouvelles data, il faudra créer de nouveau type.
    /// Exemple : Si on a un script GroundValue qui gère toute la donné d'un élément du terrain (Exemple: un rocher)
    ///     On peut avoir dedans : Niveau d'humidité, niveau d'érosion, ...
    ///     On récupère alors toute ces données et on en calcul le score ICI.
    /// </summary>
    
    private List<ECO_AGT_MakeSound> noiseInArea = new List<ECO_AGT_MakeSound>();

    public override EcosystemDataType GetDataType()
    {
        return EcosystemDataType.Noise;
    }

    protected override void OnAddData(ECO_AGT_MakeSound data)
    {
        noiseInArea.Add(data);
    }

    protected override void OnRemoveData(ECO_AGT_MakeSound data)
    {
        noiseInArea.Remove(data);
    }

    protected override int ScoreCalculation()
    {
        float toReturn = 0;
        for(int i = 0; i < noiseInArea.Count; i++)
        {
            toReturn += noiseInArea[i].GetScore();
        }
        return Mathf.RoundToInt(toReturn);
    }
}
