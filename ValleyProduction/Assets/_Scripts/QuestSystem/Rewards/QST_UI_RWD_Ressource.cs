using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QST_UI_RWD_Ressource : QST_UI_Reward<QST_RWD_Ressource>
{
    [SerializeField] private Image ressourceIcon;
    [SerializeField] private TextMeshProUGUI amountText;

    public override void OnHideReward()
    {
        Destroy(gameObject);
    }

    public override void OnShowReward(QST_RWD_Ressource reward)
    {
        gameObject.SetActive(true);

        ressourceIcon.sprite = reward.RessourceType.Icon;
        amountText.text = reward.Amount.ToString();
    }
}
