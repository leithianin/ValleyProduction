using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_AvoidForest : OnBoarding
{
    public UnityEvent fakeBehavior;
    public UnityEvent endMessage;
    [SerializeField] public List<Vector3> vectorList;
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
        OnBoardingManager.OnWindMill += OnClick;
    }

    public void OnClick(bool condition)
    {
        OnBoardingManager.OnWindMill -= OnClick;
        Over();
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
        yield return new WaitForSeconds(3f);
        PlayEndMessage();
    }
}
