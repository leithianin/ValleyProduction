using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaDisplay : MonoBehaviour
{
    public abstract void OnUpdateScore(int newScore);

    public void AffectToArea(List<AreaData> area)
    {
        // CODE REVIEW : Voir comment on diff�rencie quel display est affect� par quel score
        for (int i = 0; i < area.Count; i++)
        {
            area[i].OnUpdateScore += OnUpdateScore;
        }
    }
}
