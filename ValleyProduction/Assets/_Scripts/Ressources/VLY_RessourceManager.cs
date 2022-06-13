using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VLY_RessourceManager : VLY_Singleton<VLY_RessourceManager>
{
    [SerializeField] private float ressourceStart = 500f;
    [SerializeField] private float ressourceProductionSpeed = 10f;
    [SerializeField] private float ressourceByVisitor = 0.2f;
    [SerializeField] private float ressourceGainedBase = 1f;

    [SerializeField] private VLY_GlobalData currentRessources;

    [Header("Feedbacks")]
    [SerializeField] private UnityEvent<int> OnGainRessource;
    [SerializeField] private UnityEvent<int> OnLoseRessource;

    private TimerManager.Timer ressourceTimer = null;

    private float ressourceToGain;

    public static int GetRessource => Mathf.FloorToInt(instance.currentRessources.Value);

    private void Start()
    {
        currentRessources.ResetData();

        currentRessources.SetValue(ressourceStart);

        currentRessources.OnAskChangeValue += GainRessource;

        GainRessource(0);

        ressourceTimer = TimerManager.CreateGameTimer(ressourceProductionSpeed, GainRessourceOnTime);
    }

    private void OnDestroy()
    {
        currentRessources.OnAskChangeValue -= GainRessource;
    }

    public static void EnableFeature(bool isEnable)
    {
        if (instance.enabled != isEnable)
        {
            instance.enabled = isEnable;

            if (!isEnable)
            {
                instance.ressourceTimer.Stop();
            }
            else
            {
                instance.ressourceTimer = TimerManager.CreateGameTimer(instance.ressourceProductionSpeed, instance.GainRessourceOnTime);
            }
        }
    }

    public static void GainRessource(float amount)
    {
        instance.currentRessources.AddValue(amount);
        instance.OnGainRessource?.Invoke(GetRessource);
    }

    public static void LoseRessource(float amount)
    {
        instance.currentRessources.AddValue(-amount);
        instance.OnLoseRessource?.Invoke(GetRessource);
    }

    public static bool HasEnoughRessources(int wantedAmount)
    {
        return GetRessource >= wantedAmount;
    }

    private void GainRessourceOnTime()
    {
        ressourceTimer?.Stop();

        GainRessource((ressourceGainedBase * Mathf.Ceil(AttractivityManager.AttractivityScore)) + (VisitorManager.UsedVisitorNumber() * ressourceByVisitor));

        ressourceTimer = TimerManager.CreateGameTimer(ressourceProductionSpeed, GainRessourceOnTime);
    }
}
