using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Heatmap : MonoBehaviour
{
    [SerializeField] private HeatmapViewController heatmapViewController;

    [SerializeField] private List<UI_ButtonController> heatmapButtons;

    public void SelectHeatmap(int index)
    {
        int buttonIndex = index - 1;

        for(int i =0; i < heatmapButtons.Count; i++)
        {
            if(i == buttonIndex)
            {
                heatmapButtons[i].SetSelected(true);
            }
            else
            {
                heatmapButtons[i].SetSelected(false);
            }
        }
    }
}
