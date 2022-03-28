using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum LandmarkType
{
    None,
    Spawn,
    Landmark1,
    Landmark2,
    Landmark3
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

    private void OnUpdateNode()
    {
        HasValidPathTo(LandmarkType.Spawn);
    }


    private bool HasValidPathTo(LandmarkType target)
    {
        bool toReturn = false;

        for(int i = 0; i < pathPointNextTo.Count; i++)
        {
            if(pathPointNextTo[i].HasValidPathForLandmark(target))
            {
                toReturn = true;
                OnValidatePathToSpawn?.Invoke(true, type);
                break;
            }
        }

        if(!toReturn)
        {
            OnValidatePathToSpawn?.Invoke(false, type);
        }

        return toReturn;
    }
}
