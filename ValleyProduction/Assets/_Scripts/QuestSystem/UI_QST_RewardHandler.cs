using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QST_RewardHandler : MonoBehaviour
{
    [SerializeField] private Transform rewardHolder;

    public void ShowReward(List<QST_Reward> rewards)
    {
        gameObject.SetActive(true);

        foreach(QST_Reward reward in rewards)
        {
            QST_UI_Reward rewardDisplay = Instantiate(reward.RewardDisplay, rewardHolder);
            rewardDisplay.ShowReward(reward);
        }
    }

    public void HideReward()
    {
        gameObject.SetActive(false);

        while (rewardHolder.childCount > 0)
        {
            Destroy(rewardHolder.GetChild(0).gameObject);
        }
    }
}
