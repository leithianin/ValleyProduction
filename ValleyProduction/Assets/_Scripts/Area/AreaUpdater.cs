using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaUpdater<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T data;

    private Area currentArea;

    private Vector2 Position => new Vector2(transform.position.x, transform.position.z);

    public void UpdateData()
    {
        Area toCheck = AreaManager.GetAreaAtPosition(Position);

        if(toCheck != currentArea && toCheck != null)
        {
            AreaManager.RemoveDataToArea(currentArea, data);
            currentArea = toCheck;
            AreaManager.AddDataToArea<T>(currentArea, data);
        }
    }
}
