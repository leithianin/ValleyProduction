using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New global data", menuName ="Create Global Data")]
public class VLY_GlobalData : ScriptableObject
{
    [SerializeField] private float value;

    public Action<float> OnValueChange;

    public float Value => value;

    public void ChangeValue(float toAdd)
    {
        value += toAdd;
        OnValueChange?.Invoke(value);
    }

    public void ResetData()
    {
        value = 0;
    }
}
