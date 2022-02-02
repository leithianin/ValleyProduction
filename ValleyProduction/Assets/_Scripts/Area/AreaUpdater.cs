using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaUpdater : MonoBehaviour
{
    public abstract void UpdateData();
}

/// <summary>
/// Gère l'Update d'une data d'un objet de type T. Exemple : Voir AU_MakeSound.
/// </summary>
/// <typeparam name="T">Le type contenant la data.</typeparam>
public abstract class AreaUpdater<T> : AreaUpdater// where T : MonoBehaviour
{
    [SerializeField] protected T data;

    protected T lastUpdatedData;

    protected Area currentArea;

    protected Vector2 Position => new Vector2(transform.position.x, transform.position.z);

    private void OnEnable()
    {
        AreaManager.AddAreaUpdater(this);
    }

    private void OnDisable()
    {
        AreaManager.RemoveAreaUpdater(this);
    }

    public override void UpdateData()
    {
        Area toCheck = AreaManager.GetAreaAtPosition(Position);

        if (toCheck != currentArea)
        {
            if (lastUpdatedData != null)
            {
                AreaManager.RemoveDataToArea<T>(currentArea, lastUpdatedData);
            }
            currentArea = toCheck;

            AreaManager.AddDataToArea<T>(currentArea, data);
        }
        else
        {
            currentArea = toCheck;

            if (lastUpdatedData != null)
            {
                AreaManager.RefreshDataToArea<T>(currentArea, lastUpdatedData, data);
            }
            else
            {
                AreaManager.AddDataToArea<T>(currentArea, data);
            }
        }

        SetLastUpdateData(data);
    }

    public abstract void SetData(T newData);

    protected virtual void SetLastUpdateData(T lastData)
    {
        lastUpdatedData = lastData;
    }
}
