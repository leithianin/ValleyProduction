using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QST_UI_RWD_Infrastructure : QST_UI_Reward<QST_RWD_UnlockStructure>
{
    [SerializeField] private List<Image> structuresIcons;

    public override void OnHideReward()
    {
        Destroy(gameObject);
    }

    public override void OnShowReward(QST_RWD_UnlockStructure reward)
    {
        gameObject.SetActive(true);

        for(int i = 0; i < reward.ToUnlock.Count; i++)
        {
            structuresIcons[i].sprite = reward.ToUnlock[i].RewardIcon;
            structuresIcons[i].gameObject.SetActive(true);
        }
    }
}
