using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class QST_UI_Reward : MonoBehaviour
{
    public abstract void ShowReward(QST_Reward reward, Action callback);

    public abstract void HideReward();
}

public abstract class QST_UI_Reward<T> : QST_UI_Reward where T : QST_Reward
{
    protected Action endCallback;

    public abstract void OnShowReward(T reward);
    public abstract void OnHideReward();

    public override void ShowReward(QST_Reward reward, Action callback)
    {
        if(reward is T)
        {
            endCallback = callback;

            OnShowReward(reward as T);
        }
    }

    public override void HideReward()
    {
        OnHideReward();
        endCallback?.Invoke();
    }
}
