using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class QST_UI_Reward : MonoBehaviour
{
    public abstract void ShowReward(QST_Reward reward);

    public abstract void HideReward();
}

public abstract class QST_UI_Reward<T> : QST_UI_Reward where T : QST_Reward
{
    public abstract void OnShowReward(T reward);
    public abstract void OnHideReward();

    public override void ShowReward(QST_Reward reward)
    {
        if(reward is T)
        {
            OnShowReward(reward as T);
        }
    }

    public override void HideReward()
    {
        OnHideReward();
    }
}
