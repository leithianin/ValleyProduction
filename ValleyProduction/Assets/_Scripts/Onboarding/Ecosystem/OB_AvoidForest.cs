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

    }

    public void OnClickMenhir()
    {
        Debug.Log("Menhir");
        if(clickEnd) { Over(); }
        else { clickEnd = true; }
    }

    public void OnClickStartPoint()
    {
        Debug.Log("Startpoint");
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
