using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_OBJB_VisitorReachLandmark : QST_ObjectiveBehavior<QST_OBJ_VisitorReachLandmark>
{
    private List<QST_OBJ_VisitorReachLandmark> pendingObjectives = new List<QST_OBJ_VisitorReachLandmark>();

    protected override void OnCompleteObjective(QST_OBJ_VisitorReachLandmark objective)
    {
        pendingObjectives.Remove(objective);
        if (pendingObjectives.Count <= 0)
        {
            VLY_LandmarkManager.OnVisitorInteractWithLandmark -= CheckLandmarkInteraction;
        }
    }

    protected override void OnRefreshObjective(QST_OBJ_VisitorReachLandmark objective)
    {
        
    }

    protected override void OnStartObjective(QST_OBJ_VisitorReachLandmark objective)
    {
        if (pendingObjectives.Count <= 0)
        {
            VLY_LandmarkManager.OnVisitorInteractWithLandmark += CheckLandmarkInteraction;
        }
        pendingObjectives.Add(objective);
    }

    private void CheckLandmarkInteraction(LandmarkType landmark, VisitorBehavior visitor)
    {
        for(int i = 0; i < pendingObjectives.Count; i++)
        {
            if (landmark == pendingObjectives[i].Landmark)
            {
                if (visitor != null && visitor.visitorType == pendingObjectives[i].Visitor)
                {
                    int validValues = 0;

                    CPN_Stamina stamina = visitor.Handler.GetComponentOfType<CPN_Stamina>();

                    if (stamina != null && stamina.GetStamina >= pendingObjectives[i].Stamina)
                    {
                        validValues++;
                    }

                    CPN_SatisfactionHandler satisfaction = visitor.Handler.GetComponentOfType<CPN_SatisfactionHandler>();

                    if(satisfaction != null && satisfaction.CurrentSatisfaction >= pendingObjectives[i].Satisfaction)
                    {
                        validValues++;
                    }

                    if (validValues >= 2)
                    {
                        AskCompleteObjective(pendingObjectives[i]);
                    }
                }
            }
        }
    }
}
