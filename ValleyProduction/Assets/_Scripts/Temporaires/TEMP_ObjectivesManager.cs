using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TEMP_ObjectivesManager : VLY_Singleton<TEMP_ObjectivesManager>
{
    private TimerManager.Timer objectiveCheckTimer = null;

    [SerializeField] private int wantedAnimalCount;
    [SerializeField] private int wantedLandmarkCount;

    [SerializeField] private bool animalObjective = false;
    [SerializeField] private bool landmarkObjective = false;
    [SerializeField] private bool bearObjective = false;
    [SerializeField] private bool isPhaseOneGood = false;

    [SerializeField] private UnityEvent OnSetAnimalObjective;
    [SerializeField] private UnityEvent OnResolveAnimalObjective;
    [SerializeField] private UnityEvent OnFailAnimalObjective;

    [SerializeField] private UnityEvent OnSetLandmarkObjective;
    [SerializeField] private UnityEvent OnResolveLandmarkObjective;
    [SerializeField] private UnityEvent OnFailLandmarkObjective;

    [SerializeField] private UnityEvent OnValidatePhaseOne;

    [SerializeField] private UnityEvent OnSetBearObjective;
    [SerializeField] private UnityEvent OnResolveBearObjective;

    [SerializeField] private List<LandmarkType> currentValidLandmark = new List<LandmarkType>();

    private void Start()
    {
        SetPhaseOne();

        objectiveCheckTimer = TimerManager.CreateGameTimer(1f, CheckObjectives);
    }

    private void CheckObjectives()
    {
        if (!isPhaseOneGood)
        {
            int animalCount = AreaManager.GetAnimalInValley();

            if (animalCount >= wantedAnimalCount && !animalObjective)
            {
                animalObjective = true;

                if (landmarkObjective)
                {
                    isPhaseOneGood = true;

                    SetPhaseTwo();
                }

                OnResolveAnimalObjective?.Invoke();

            }
            else if (animalCount < wantedAnimalCount && animalObjective)
            {
                animalObjective = false;

                OnFailAnimalObjective?.Invoke();
            }

            objectiveCheckTimer = TimerManager.CreateGameTimer(1f, CheckObjectives);
        }
    }

    public void CheckLandmarkObjective(bool landmarkResult, LandmarkType landmarkType)
    {
        if (!isPhaseOneGood)
        {
            Debug.Log(landmarkType);
            if (landmarkType != LandmarkType.Spawn && landmarkType != LandmarkType.None)
            {
                if (!currentValidLandmark.Contains(landmarkType) && landmarkResult)
                {
                    currentValidLandmark.Add(landmarkType);
                }
                else if (currentValidLandmark.Contains(landmarkType) && !landmarkResult)
                {
                    currentValidLandmark.Remove(landmarkType);
                }
            }

            if (currentValidLandmark.Count >= wantedLandmarkCount && !landmarkObjective)
            {
                landmarkObjective = true;

                if(animalObjective)
                {
                    isPhaseOneGood = true;

                    SetPhaseTwo();
                }

                OnResolveLandmarkObjective?.Invoke();
            }
            else if (currentValidLandmark.Count < wantedLandmarkCount && landmarkObjective)
            {
                landmarkObjective = false;

                OnFailLandmarkObjective?.Invoke();
            }
        }
    }

    public void ValidateBearObjective()
    {
        if (isPhaseOneGood && !bearObjective)
        {
            OnResolveBearObjective?.Invoke();

            bearObjective = true;
        }
    }

    public void SetPhaseOne()
    {
        OnSetAnimalObjective?.Invoke();

        OnSetLandmarkObjective?.Invoke();
    }

    public void SetPhaseTwo()
    {
        OnValidatePhaseOne?.Invoke();

        OnSetBearObjective?.Invoke();
    }
}
