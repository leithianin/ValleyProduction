using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum LandmarkType
{
    None,
    Spawn,
    Natural,
    Human,
    Cultural
}

public class CPN_IsLandmark : VLY_Component
{
    [SerializeField] private LandmarkType type;

    public LandmarkType Type => type;

    [SerializeField] private List<PathNode> pathPointNextTo = new List<PathNode>();

    [SerializeField] private UnityEvent<bool, LandmarkType> OnValidatePathToSpawn;

    public void AddPointToLandmark(PathNode toAdd)
    {
        if (!pathPointNextTo.Contains(toAdd))
        {
            pathPointNextTo.Add(toAdd);

            HasValidPathTo(LandmarkType.Spawn);

            toAdd.OnDeleteNode += RemovePointToLandmark;
            NodePathProcess.CallOnUpdateNode(OnUpdateNode);
        }
    }

    public void RemovePointToLandmark(PathNode toRemove)
    {
        toRemove.OnDeleteNode -= RemovePointToLandmark;
        NodePathProcess.UncallOnUpdateNode(OnUpdateNode);

        pathPointNextTo.Remove(toRemove);

        HasValidPathTo(LandmarkType.Spawn);
    }

    [System.Obsolete]
    public void OnInteractWithLandmark(CPN_InteractionHandler interactor)
    {
        //VLY_LandmarkManager.OnLandmarkInteraction(type, interactor);
    }

    private void OnUpdateNode()
    {
        HasValidPathTo(LandmarkType.Spawn);
    }


    private bool HasValidPathTo(LandmarkType target)
    {
        bool toReturn = false;

        for(int i = 0; i < pathPointNextTo.Count; i++)
        {
            List<CPN_IsLandmark> landmarks = VLY_LandmarkManager.GetLandmarkOfType(target);

            for (int j = 0; j < landmarks.Count; j++)
            {
                if (pathPointNextTo[i].HasValidPathForLandmark(landmarks[j]))
                {
                    toReturn = true;
                    OnValidatePathToSpawn?.Invoke(true, type);

                    VLY_LandmarkManager.AddValidLandmark(type);
                    break;
                }
            }

            if(toReturn == true)
            {
                break;
            }
        }

        if(!toReturn)
        {
            OnValidatePathToSpawn?.Invoke(false, type);

            VLY_LandmarkManager.RemoveValidLandmark(type);
        }

        return toReturn;
    }
}
