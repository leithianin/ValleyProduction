using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QST_OBJB_ValidPathToLandmark : QST_ObjectiveBehavior<QST_OBJ_ValidPathToLandmark>
{
    private List<QST_OBJ_ValidPathToLandmark> pendingObjectives = new List<QST_OBJ_ValidPathToLandmark>();

    protected override void OnCompleteObjective(QST_OBJ_ValidPathToLandmark objective)
    {
        pendingObjectives.Remove(objective);
        if (pendingObjectives.Count <= 0)
        {
            VLY_LandmarkManager.OnLandmarkHasValidPath -= OnLandmarkWithValidPath;
        }
    }

    protected override void OnRefreshObjective(QST_OBJ_ValidPathToLandmark objective)
    {
        throw new System.NotImplementedException();
    }

    protected override void OnStartObjective(QST_OBJ_ValidPathToLandmark objective)
    {
        pendingObjectives.Add(objective);
        if (pendingObjectives.Count < 2)
        {
            VLY_LandmarkManager.OnLandmarkHasValidPath += OnLandmarkWithValidPath;
        }
    }

    private void OnLandmarkWithValidPath(LandmarkType landmark)
    {
        for (int i = 0; i < pendingObjectives.Count; i++)
        {
            if (landmark == pendingObjectives[i].Landmark)
            {
                AskCompleteObjective(pendingObjectives[i]);
            }
        }
    }
}
