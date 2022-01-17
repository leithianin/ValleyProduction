using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaUpdater : MonoBehaviour
{
    public abstract void UpdateData();
}

public class AreaUpdater<T> : AreaUpdater// where T : MonoBehaviour
{
    [SerializeField] private T data;

    private Area currentArea;

    private Vector2 Position => new Vector2(transform.position.x, transform.position.z);

    private void OnEnable()
    {
        AreaManager.AddAreaUpdater(this);
    }

    private void OnDisable()
    {
        AreaManager.RemoveAreaIpdater(this);
    }

    public override void UpdateData()
    {
        Area toCheck = AreaManager.GetAreaAtPosition(Position);

        Debug.Log("Update Data : " + gameObject.name);

        AreaManager.RemoveDataToArea(currentArea, data);
        currentArea = toCheck;
        AreaManager.AddDataToArea<T>(currentArea, data);

    }
}
