using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_Informations : MonoBehaviour
{
    [SerializeField] private string structureName;
    [SerializeField] private string structureDescription;

    public AreaUpdater AU_Sound;
    public AreaUpdater AU_Plant;
    public AreaUpdater AU_Polluter;

    public string GetName => structureName;
    public string GetDescription => structureDescription;

    public float GetNoiseScore()
    {
        if(AU_Sound != null)
        {
            return AU_Sound.GetScore();
        }
        return 0;
    }

    public float GetPlantScore()
    {
        if (AU_Plant != null)
        {
            return AU_Plant.GetScore();
        }
        return 0;
    }


    public float GetPolluterScore()
    {
        if (AU_Polluter != null)
        {
            return AU_Polluter.GetScore();
        }
        return 0;
    }
}
