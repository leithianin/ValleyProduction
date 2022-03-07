using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VLY_RessourceManager : VLY_Singleton<VLY_RessourceManager>
{
    [SerializeField] private float ressourceProductionSpeed = 10f;
    [SerializeField] private float ressourceByVisitor = 0.2f;
    [SerializeField] private float ressourceGainedBase = 1f;

    [SerializeField] private float currentRessources;

    [Header("Feedbacks")]
    [SerializeField] private UnityEvent<int> OnGainRessource;
    [SerializeField] private UnityEvent<int> OnLoseRessource;

    private TimerManager.Timer ressourceTimer = null;

    public static int GetRessource => Mathf.FloorToInt(instance.currentRessources);

    public static void GainRessource(float amount)
    {
        instance.currentRessources += amount * Mathf.Ceil(AttractivityManager.AttractivityScore);
        instance.OnGainRessource?.Invoke(GetRessource);
    }

    public static void LoseRessource(float amount)
    {
        instance.currentRessources -= amount;
        instance.OnLoseRessource?.Invoke(GetRessource);
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
