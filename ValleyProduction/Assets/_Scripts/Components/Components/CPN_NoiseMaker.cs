using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_NoiseMaker : VLY_Component<CPN_Data_Noise>
{
    public UnityEvent<CPN_Data_Noise> OnChangeScore;

    public override void SetData(CPN_Data_Noise dataToSet)
    {
        OnChangeScore?.Invoke(dataToSet);
    }
}
