using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DataDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text ressourceCounter;
    [SerializeField] private TMP_Text attractivityCounter;
    [SerializeField] private TMP_Text visitorsCounter;

    public void UpdateNbRessources(int nb)
    {
        ressourceCounter.text = nb.ToString();
    }

    public void UpdateNbAttractivity(int nb)
    {
        attractivityCounter.text = nb.ToString();
    }

    public void UpdateNbVisitors(int nb)
    {
        visitorsCounter.text = nb.ToString();
    }
}
