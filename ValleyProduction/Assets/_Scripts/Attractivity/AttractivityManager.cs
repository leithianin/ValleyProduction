using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttractivityManager : VLY_Singleton<AttractivityManager>
{
    [SerializeField] private float timeBetweenUpdates;

    [SerializeField] private float attractivityScore;

    public static float AttractivityScore => instance.attractivityScore;

    private void Start()
    {
        TimerManager.CreateGameTimer(timeBetweenUpdates, CalculateAttractivity);
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

        TimerManager.CreateGameTimer(timeBetweenUpdates, CalculateAttractivity);
    }
}
