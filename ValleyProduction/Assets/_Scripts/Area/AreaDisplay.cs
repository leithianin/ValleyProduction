using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AreaDisplayDataHandler
{
    public AreaDataType dataToCheck;
    public int score;
    public float coef;
}

public abstract class AreaDisplay : MonoBehaviour
{
    [SerializeField] private List<AreaDisplayDataHandler> datas;

    public abstract void OnUpdateScore(float newScore);

    public void AffectToArea(List<AreaData> possibleDatas)
    {
        // CODE REVIEW : Voir comment on différencie quel display est affecté par quel score
        for (int i = 0; i < possibleDatas.Count; i++)
        {
            AreaData areaData = possibleDatas[i];

            if (NeedDataType(areaData.GetDataType()))
            {
                areaData.OnUpdateScore += (int s) => UpdateData(s, areaData.GetDataType());
            }
        }
    }

    private void UpdateData(int score, AreaDataType data)
    {
        int totalScore = 0;
        float totalCoef = 0;
        for(int i = 0; i < datas.Count; i++)
        {
            if(datas[i].dataToCheck == data)
            {
                datas[i].score = score;
            }

            totalScore += datas[i].score;
            totalCoef += datas[i].coef;
        }

        OnUpdateScore(totalScore / totalCoef);
    }

    private bool NeedDataType(AreaDataType dataType)
    {
        bool toReturn = false;

        for (int i = 0; i < datas.Count; i++)
        {
            if(datas[i].dataToCheck == dataType)
            {
                toReturn = true;
            }
        }

        return toReturn;
    }
}
