using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_DataHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreHandler;
    [SerializeField] private string textScore;

    public void SetScore(float value)
    {
        if (value != 0)
        {
            scoreHandler.text = textScore + value.ToString();
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
