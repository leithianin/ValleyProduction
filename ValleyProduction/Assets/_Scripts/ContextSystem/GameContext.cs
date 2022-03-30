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
    Satisfaction
}

[CreateAssetMenu(fileName ="Game Context", menuName = "Create Game Context")]
public class GameContext : ScriptableObject
{
    [System.Serializable]
    public class Context
    {
        [SerializeField] private string contextName;
        public LayerMask contextLayer;
        public List<FeatureLocker> lockedFeatures;
    }

    [SerializeField] private List<Context> contexts;

    public Context GetContext(int contextID)
    {
        return contexts[contextID];
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
