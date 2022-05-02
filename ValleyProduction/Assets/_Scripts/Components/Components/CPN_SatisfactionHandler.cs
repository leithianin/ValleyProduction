using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CPN_SatisfactionHandler : VLY_Component<CPN_Data_SatisfactionHandler>
{
    [SerializeField] private UnityEvent OnGainSatisfaction;
    [SerializeField] private UnityEvent OnLoseSatisfaction;

    [SerializeField] private List<SatisfactorType> likedInteractions;
    [SerializeField] private List<SatisfactorType> hatedInteractions;

    [SerializeField] private float currentSatisfaction;

    [SerializeField] private float maxSatisfaction = 5f;
    [SerializeField] private float maxTiredSatisfaction;

    private float currentPossibleMax;

    public float CurrentSatisfaction => currentSatisfaction;

    private void Start()
    {
        currentPossibleMax = maxSatisfaction;
    }

    public override void SetData(CPN_Data_SatisfactionHandler dataToSet)
    {
        likedInteractions = new List<SatisfactorType>(dataToSet.LikedInteractions());
        hatedInteractions = new List<SatisfactorType>(dataToSet.HatedInteractions());
    }

    public int GetAppreciationLevel(SatisfactorType satisfactionPointType)
    {
        if(likedInteractions.Contains(satisfactionPointType))
        {
            return 1;
        }
        else if (hatedInteractions.Contains(satisfactionPointType))
        {
            return -1;
        }

        return 0;
    }

    public void AddSatisfaction(float toAdd)
    {
        if(toAdd > 0)
        {
            if (currentSatisfaction < currentPossibleMax)
            {
                OnGainSatisfaction?.Invoke();
            }
        }
        else if (toAdd < 0)
        {
            if (currentSatisfaction > 0)
            {
                OnLoseSatisfaction?.Invoke();
            }
        }

        currentSatisfaction += toAdd;

        if(currentSatisfaction > currentPossibleMax)
        {
            currentSatisfaction = currentPossibleMax;
        }
        else if(currentSatisfaction < 0)
        {
            currentSatisfaction = 0;
        }
    }

    public void SetMaxOrTired(bool isTired)
    {
        if(isTired)
        {
            currentPossibleMax = maxTiredSatisfaction;
        }
        else
        {
            currentPossibleMax = maxSatisfaction;
        }
    }
}
