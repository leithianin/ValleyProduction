using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_Time
{
    private static float savedTimeScale = 1f;

    public static void SetTimeScale(float value)
    {
        savedTimeScale = value;
        ResetTime();
    }

    public static void PauseTime()
    {
        Time.timeScale = 0f;
    }

    public static void ResetTime()
    {
        Time.timeScale = savedTimeScale;
    }
}
