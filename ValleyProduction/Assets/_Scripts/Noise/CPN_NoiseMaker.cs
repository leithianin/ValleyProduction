using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_NoiseMaker : VLY_Component<CPN_Data_Noise>
{
    [SerializeField] private float noiseMade;

    public float NoiseMade => noiseMade;

    public override void SetData(CPN_Data_Noise dataToSet)
    {
        noiseMade = dataToSet.NoiseMade();
    }
}
