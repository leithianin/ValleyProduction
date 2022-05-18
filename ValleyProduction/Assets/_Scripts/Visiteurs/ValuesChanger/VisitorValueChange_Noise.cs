using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitorValueChange_Noise : MonoBehaviour
{
    [SerializeField] private float noiseToAdd;
    [SerializeField] private float delayForReset;

    public void ChangeNoiseFromHandler(VLY_ComponentHandler handler)
    {
        CPN_NoiseMaker noiseMaker = handler.GetComponentOfType<CPN_NoiseMaker>();

        if (noiseMaker != null)
        {
            noiseMaker.AddNoise(noiseToAdd);
            if(delayForReset > 0)
            {
                TimerManager.CreateGameTimer(delayForReset, () => noiseMaker.RemoveNoise(noiseToAdd));
            }
        }
    }
}
