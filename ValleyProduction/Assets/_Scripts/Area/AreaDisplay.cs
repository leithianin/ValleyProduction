using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaDisplay : MonoBehaviour
{
    public abstract void OnUpdateScore(int newScore);

    public void AffectToArea(List<AreaData> area)
    {
        // CODE REVIEW : Voir comment on différencie quel display est affecté par quel score
        for (int i = 0; i < area.Count; i++)
        {
            area[i].OnUpdateScore += OnUpdateScore;
        }
    }
}
