using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_SatisfactionHandler_FloraEffect : CPN_SatisfactionHandler
{
    [SerializeField] private float updateTime;
    [SerializeField] private float floraLevelWanted;
    [SerializeField] private float toLoseOnBadFlora;

    private TimerManager.Timer satisfactionUpdateTimer;

    private void OnEnable()
    {
        satisfactionUpdateTimer = TimerManager.CreateGameTimer(updateTime, CheckFloraLevel);
    }

    private void OnDisable()
    {
        if(satisfactionUpdateTimer != null)
        {
            satisfactionUpdateTimer.Stop();
        }
    }

    private void CheckFloraLevel()
    {
        if (VLY_EcosystemManager.GetScoreAtPosition(new Vector2(transform.position.x, transform.position.z), EcosystemDataType.Flora) < floraLevelWanted)
        {
            AddSatisfaction(-toLoseOnBadFlora);
        }

        satisfactionUpdateTimer = TimerManager.CreateGameTimer(updateTime, CheckFloraLevel);
    }
}
