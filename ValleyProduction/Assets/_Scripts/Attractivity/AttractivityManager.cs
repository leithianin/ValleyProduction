using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttractivityManager : VLY_Singleton<AttractivityManager>
{
    [SerializeField] private float timeBetweenUpdates;

    [SerializeField] private float attractivityScore;

    [SerializeField] private UnityEvent<float> OnUpdateAttractivity;

    [SerializeField] private float attractivityCheat;

    private TimerManager.Timer currentTimer;

    public static float AttractivityScore => instance.attractivityScore;

    private void Start()
    {
        CalculateAttractivity();
    }


    public static void EnableFeature(bool isEnable)
    {
        if (instance.enabled != isEnable)
        {
            instance.enabled = isEnable;

            if (!isEnable)
            {
                instance.currentTimer.Stop();
            }
            else
            {
                instance.CalculateAttractivity();
            }
        }
    }

    private void CalculateAttractivity()
    {
        List<VisitorBehavior> visitorsInValley = VisitorManager.GetUsedVisitors();

        attractivityScore = 0;
        int visitorNumber = 0;

        for(int i = 0; i < visitorsInValley.Count; i++)
        {
            CPN_SatisfactionHandler satisfactionHandler = visitorsInValley[i].GetComponent<VLY_ComponentHandler>().GetComponentOfType<CPN_SatisfactionHandler>();

            if(satisfactionHandler != null)
            {
                attractivityScore += satisfactionHandler.CurrentSatisfaction;
                visitorNumber++;
            }
        }

        if (visitorNumber > 0)
        {
            attractivityScore /= visitorNumber;
        }

        attractivityScore += attractivityCheat;

        OnUpdateAttractivity?.Invoke(attractivityScore);

        currentTimer = TimerManager.CreateGameTimer(timeBetweenUpdates, CalculateAttractivity);
    }
}
