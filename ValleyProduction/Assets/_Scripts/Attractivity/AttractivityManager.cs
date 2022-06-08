using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttractivityManager : VLY_Singleton<AttractivityManager>
{
    [SerializeField] private float timeBetweenUpdates;

    [SerializeField] private VLY_GlobalData attractivityScore;

    [SerializeField] private UnityEvent<float> OnUpdateAttractivity;

    [SerializeField] private float attractivityCheat;

    private TimerManager.Timer currentTimer;

    public static float AttractivityScore => instance.attractivityScore.Value;

    private void Start()
    {
        CalculateAttractivity();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            attractivityCheat = 5;
        }
        
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

        float attractivityCalcul = 0;
        int visitorNumber = 0;

        for(int i = 0; i < visitorsInValley.Count; i++)
        {
            CPN_SatisfactionHandler satisfactionHandler = visitorsInValley[i].GetComponent<VLY_ComponentHandler>().GetComponentOfType<CPN_SatisfactionHandler>();

            if(satisfactionHandler != null)
            {
                attractivityCalcul += satisfactionHandler.CurrentSatisfaction;
                visitorNumber++;
            }
        }

        if (visitorNumber > 0)
        {
            attractivityCalcul /= visitorNumber;
        }

        attractivityCalcul += attractivityCheat;

        attractivityScore.SetValue(attractivityCalcul);

        OnUpdateAttractivity?.Invoke(attractivityScore.Value);

        currentTimer = TimerManager.CreateGameTimer(timeBetweenUpdates, CalculateAttractivity);
    }
}
