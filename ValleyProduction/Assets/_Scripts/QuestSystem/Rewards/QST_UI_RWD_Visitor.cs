using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QST_UI_RWD_Visitor : QST_UI_Reward<QST_RWD_VisitorType>
{
    [SerializeField] private Image visitorIcon;
    [SerializeField] private TextMeshProUGUI visitorTypeName;
    [SerializeField] private TextMeshProUGUI visitorObjective;

    public override void OnHideReward()
    {
        Destroy(gameObject);
    }

    public override void OnShowReward(QST_RWD_VisitorType reward)
    {
        gameObject.SetActive(true);

        visitorIcon.sprite = reward.Visitor.Icon;
        visitorTypeName.text = reward.Visitor.Nom;
    }
}
