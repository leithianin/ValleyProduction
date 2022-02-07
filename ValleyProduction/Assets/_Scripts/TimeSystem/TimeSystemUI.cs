using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystemUI : MonoBehaviour
{
    public void SetTimeScale(float value)
    {
        VLY_Time.SetTimeScale(value);
    }

    public void ResetTime()
    {
        VLY_Time.ResetTime();
    }

    public void PauseTime()
    {
        VLY_Time.PauseTime();
    }
}
