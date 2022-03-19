using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_AvoidForest : OnBoarding
{
    public float time = 5f;
    public UnityEvent fakeBehavior;
    public UnityEvent endMessage;
    public UnityEvent HideUI;
    [SerializeField] public List<Vector3> vectorList;

    private bool clickEnd = false;
    protected override void OnEnd()
    {
        Debug.Log("END");
        foreach(IST_PathPoint pp in PathManager.GetCurrentPathpointList)
        {
            vectorList.Add(pp.transform.position);
        }

        PathManager.CreatePathData();
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.1f);
        PathManager.DestroyLineList();
        yield return new WaitForSeconds(0.5f);
        PlayFakeBehavior();
    }

    protected override void OnPlay()
    {
        OnBoardingManager.OnWindMill += OnClickWindMill;
        OnBoardingManager.OnStartPoint += OnClickStartPoint;
    }

    public void OnClickWindMill(bool condition)
    {
        OnBoardingManager.OnWindMill -= OnClickWindMill;

        if(clickEnd) { Over(); }
        else { clickEnd = true; }
    }

    public void OnClickStartPoint(bool condition)
    {
        OnBoardingManager.OnStartPoint -= OnClickStartPoint;

        if (clickEnd) { Over(); }
        else { clickEnd = true; }
    }

    public void PlayFakeBehavior()
    {
        fakeBehavior?.Invoke();
        StartCoroutine(EndEcosystem());
    }

    public void PlayEndMessage()
    {
        endMessage?.Invoke();
    }

    IEnumerator EndEcosystem()
    {
        yield return new WaitForSeconds(time);
        HideUI?.Invoke();
        PlayEndMessage();
    }
}
