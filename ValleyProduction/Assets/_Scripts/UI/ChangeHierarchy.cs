using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHierarchy : MonoBehaviour
{
    public Transform parentToSet;
    public Transform gameObjectToMove;

    private void Start()
    {
        gameObjectToMove.parent = null;
        gameObjectToMove.parent = parentToSet;
    }
}
