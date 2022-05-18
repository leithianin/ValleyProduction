using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_NoiseMaker : VLY_Component<CPN_Data_Noise>
{
    public UnityEvent<CPN_Data_Noise> OnChangeScore;
    [SerializeField] private ECO_AGT_MakeSound noiseSystem;

    public override void SetData(CPN_Data_Noise dataToSet)
    {
        OnChangeScore?.Invoke(dataToSet);
    }

    public void ChangeNoise(float newNoiseValue)
    {
        noiseSystem.SetScore(newNoiseValue);
    }

    public void AddNoise(float toAdd)
    {
        noiseSystem.AddNoide(toAdd);
    }

    public void RemoveNoise(float toRemove)
    {
        noiseSystem.RemoveNoide(toRemove);
    }
}
