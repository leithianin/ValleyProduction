using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QST_UI_RWD_Infrastructure : QST_UI_Reward<QST_RWD_UnlockStructure>
{
    [SerializeField] private Image structuresIcons;
    [SerializeField] TextMeshProUGUI structureName;

    public override void OnHideReward()
    {
        Destroy(gameObject);
    }

    public override void OnShowReward(QST_RWD_UnlockStructure reward)
    {
        gameObject.SetActive(true);

        structureName.text = reward.ToUnlock[0].Name;
        structuresIcons.sprite = reward.ToUnlock[0].RewardIcon;

    }
}
