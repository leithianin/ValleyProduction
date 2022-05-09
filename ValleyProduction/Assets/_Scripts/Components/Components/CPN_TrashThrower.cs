using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_TrashThrower : VLY_Component<CPN_Data_TrashThrower>
{
    private float throwRadius;

    private Vector2 throwTimeRange;

    TimerManager.Timer throwTimer = null;

    public override void SetData(CPN_Data_TrashThrower dataToSet)
    {
        if (dataToSet.ThrowTimeRange().y <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            throwRadius = dataToSet.ThrowRadius();
            throwTimeRange = dataToSet.ThrowTimeRange();
        }
    }

    public float TimeBeforeThrow()
    {
        if(throwTimer != null)
        {
            return throwTimer.DurationLeft;
        }
        return -1;
    }

    private void OnEnable()
    {
        StartTimer();
    }

    private void OnDisable()
    {
        StopTimer();
    }

    private void StartTimer()
    {
        StartTimer(Random.Range(throwTimeRange.x, throwTimeRange.y));
    }

    private void StartTimer(float duration)
    {
        Debug.Log("START ThrowTrash");
        throwTimer = TimerManager.CreateGameTimer(duration, ThrowPosition);
    }

    private void StopTimer()
    {
        throwTimer?.Stop();
        throwTimer = null;
    }

    private void ThrowPosition()
    {
        Vector3 randomPosition = Random.insideUnitCircle * throwRadius;

        PollutionManager.ThrowTrash(transform.position + new Vector3(randomPosition.x, 0, randomPosition.y));

        Debug.Log("ThrowTrash");

        StopTimer();
        StartTimer();
    }

    public void DelayThrow(float delayTime)
    {
        if (throwTimeRange.y > 0)
        {
            if (throwTimer != null)
            {
                delayTime += throwTimer.DurationLeft;
            }

            StopTimer();
            StartTimer(delayTime);
        }
    }
}
