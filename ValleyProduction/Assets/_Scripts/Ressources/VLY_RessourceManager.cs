using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VLY_RessourceManager : VLY_Singleton<VLY_RessourceManager>
{
    [SerializeField] private float ressourceProductionSpeed = 10f;
    [SerializeField] private float ressourceByVisitor = 0.2f;
    [SerializeField] private float ressourceGainedBase = 1f;

    [SerializeField] private float currentRessources;

    private TimerManager.Timer ressourceTimer = null;

    public static int GetRessource => Mathf.FloorToInt(instance.currentRessources);

    public static void GainRessource(float amount)
    {
        instance.currentRessources += amount;
    }

    public static void LoseRessource(float amount)
    {
        instance.currentRessources -= amount;
    }

    public static bool HasEnoughRessources(int wantedAmount)
    {
        return GetRessource >= wantedAmount;
    }

    private void Start()
    {
        ressourceTimer = TimerManager.CreateGameTimer(ressourceProductionSpeed, GainRessourceOnTime);
    }

    private void GainRessourceOnTime()
    {
        ressourceTimer?.Stop();

        GainRessource(ressourceGainedBase + VisitorManager.UsedVisitorNumber() * ressourceByVisitor);

        ressourceTimer = TimerManager.CreateGameTimer(ressourceProductionSpeed, GainRessourceOnTime);
    }
}
