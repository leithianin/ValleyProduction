using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OB_AvoidForest : OnBoarding
{
    public UnityEvent fakeBehavior;
    protected override void OnEnd()
    {
        Debug.Log("END");
        PathManager.CreatePathData();
        StartCoroutine(Wait());

        //Cinematiquuuee
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
    }
}
