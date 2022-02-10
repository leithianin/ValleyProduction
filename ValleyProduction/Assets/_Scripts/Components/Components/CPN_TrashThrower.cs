using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPN_TrashThrower : VLY_Component<CPN_Data_TrashThrower>
{
    private float throwRadius;

    private Vector2 throwTimeRange;

    TimerManager.Timer newThrowTimer = null;

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
        newThrowTimer = TimerManager.CreateGameTimer(Random.Range(throwTimeRange.x, throwTimeRange.y), ThrowPosition);
    }

    private void StopTimer()
    {
        newThrowTimer?.Stop();
        newThrowTimer = null;
    }

    public void ThrowPosition()
    {
        Vector3 randomPosition = Random.insideUnitCircle * throwRadius;

        PollutionManager.ThrowTrash(transform.position + new Vector3(randomPosition.x, 0, randomPosition.y));

        StopTimer();
        StartTimer();
    }
}
