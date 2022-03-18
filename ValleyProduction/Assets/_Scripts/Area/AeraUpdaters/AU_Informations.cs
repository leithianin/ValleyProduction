using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AU_Informations : MonoBehaviour
{
    public AreaUpdater AU_Sound;
    public AreaUpdater AU_Plant;
    public AreaUpdater AU_Polluter;

    public float GetNoiseScore => AU_Sound.GetScore();
    public float GetPlantScore => AU_Plant.GetScore();
    public float GetPolluterScore => AU_Polluter.GetScore();
}
