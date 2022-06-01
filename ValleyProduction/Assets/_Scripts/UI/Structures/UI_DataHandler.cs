using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DataHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreHandler;

    public void SetScore(float value)
    {
        if (value != 0)
        {
            scoreHandler.text = value.ToString();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
