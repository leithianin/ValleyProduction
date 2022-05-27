using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FeatureLocker
{
    CameraMovement,
    CameraRotation,
    Construction,
    Modification,
    Destruction,
    Ressource,
    Satisfaction,
    BlockAllKeyboardInput,
    BlockMouseInput
}

[CreateAssetMenu(fileName ="Game Context", menuName = "Valley/Create Game Context")]
public class GameContext : ScriptableObject
{
    [System.Serializable]
    public class Context
    {
        [SerializeField] private string contextName;
        public string GetContextName => contextName;
        public LayerMask contextLayer;
        public List<FeatureLocker> lockedFeatures;
    }

    [SerializeField] private List<Context> contexts;

    public Context GetContext(int contextID)
    {
        return contexts[contextID];
    }

    public int GetContextID(string contextString)
    {
        int i = 0;
        foreach(Context ctxt in contexts)
        {
            if(ctxt.GetContextName.Equals(contextString))
            {
                return i;
            }
            i++;
        }

        return -1;
    }

    public LayerMask GetContextLayers(int contextID)
    {
        return contexts[contextID].contextLayer;
    }

    public List<FeatureLocker> GetLockedFeatures(int contextID)
    {
        return contexts[contextID].lockedFeatures;
    }
}
