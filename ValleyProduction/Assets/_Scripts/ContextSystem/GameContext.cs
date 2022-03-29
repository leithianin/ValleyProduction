using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Game Context", menuName = "Create Game Context")]
public class GameContext : ScriptableObject
{
    [System.Serializable]
    public class Context
    {
        [SerializeField] private string contextName;
        public LayerMask contextLayer;
    }

    [SerializeField] private List<Context> contexts;

    public LayerMask GetContextLayers(int contextID)
    {
        return contexts[contextID].contextLayer;
    }
}
