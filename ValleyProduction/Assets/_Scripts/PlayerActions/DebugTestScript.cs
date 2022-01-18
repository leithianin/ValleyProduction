using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTestScript : MonoBehaviour
{
    public void TryInput()
    {
        Debug.Log("Try input");
    }

    public void TryInputPosition(Vector3 position)
    {
        Debug.Log("Try input " + position);
    }

    public void TryInputObject(GameObject hitObject)
    {
        Debug.Log("Try input " + hitObject);
    }
}
