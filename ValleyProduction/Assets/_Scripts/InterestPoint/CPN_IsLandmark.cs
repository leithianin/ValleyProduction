using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum LandmarkType
{
    None,
    Spawn,
    Ruin
}

public class CPN_IsLandmark : VLY_Component
{
    [SerializeField] private LandmarkType type;

    public LandmarkType Type => type;

    [SerializeField] private List<PathNode> pathPointNextTo = new List<PathNode>(); //TEMP

    [SerializeField] private UnityEvent<bool, LandmarkType> OnValidatePathToSpawn; //TEMP

    public void AddPointToLandmark(PathNode toAdd)  //TEMP
    {
        if (!pathPointNextTo.Contains(toAdd))
        {
            pathPointNextTo.Add(toAdd);

            HasValidPathTo(LandmarkType.Spawn);

            toAdd.OnDeleteNode += RemovePointToLandmark;
            NodePathProcess.CallOnUpdateNode(OnUpdateNode);
        }
    }

    public void RemovePointToLandmark(PathNode toRemove) //TEMP
    {
        toRemove.OnDeleteNode -= RemovePointToLandmark;
        NodePathProcess.UncallOnUpdateNode(OnUpdateNode);

        pathPointNextTo.Remove(toRemove);

        HasValidPathTo(LandmarkType.Spawn);
    }

    private void OnUpdateNode() //TEMP
    {
        HasValidPathTo(LandmarkType.Spawn);
    }


    private bool HasValidPathTo(LandmarkType target) //TEMP
    {
        bool toReturn = false;

        for(int i = 0; i < pathPointNextTo.Count; i++)
        {
            if(pathPointNextTo[i].HasValidPathForLandmark(target))
            {
                toReturn = true;
                Debug.Log("Invoke with " + type);
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
