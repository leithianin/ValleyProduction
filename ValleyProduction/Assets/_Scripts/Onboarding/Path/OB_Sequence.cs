using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OB_Sequence : OnBoarding
{
    [Serializable]
    public class SequenceStep
    {
        public OnBoarding mainAction;
        public List<OnBoarding> secondaryActionList = new List<OnBoarding>();
    }

    public List<SequenceStep> onBoardingList = new List<SequenceStep>();

    private int increment = 0;

    protected override void OnEnd()
    {
        
    }

    protected override void OnPlay()
    {
        increment = -1;
        OnNextEvent();
    }

    protected void OnNextEvent()
    {
        increment++;
        if (onBoardingList.Count <= increment)
        {
            Over();
        }
        else
        {
            onBoardingList[increment].mainAction.Play(OnNextEvent);
            
            foreach(OnBoarding ob in onBoardingList[increment].secondaryActionList)
            {
                ob.Play();
            }
        }
    }
}
