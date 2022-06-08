using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckRessourceValue : MonoBehaviour
{
    public UnityEvent OnValid;
    public UnityEvent OnUnvalid;

    [SerializeField] private VLY_GlobalData attractivityScore;
    [SerializeField] private QST_OBJ_FlagValue quest;

    private void Update()
    {
        if(attractivityScore.Value >= quest.Value)
        {
            OnValid?.Invoke();
        }
        else
        {
            OnUnvalid?.Invoke();
        }
    }
}
