using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AreaUpdater : MonoBehaviour
{
    public abstract void UpdateData();

    public abstract void RemoveData();

    public abstract float GetScore();
}

/// <summary>
/// G�re l'Update d'une data d'un objet de type T. Exemple : Voir AU_MakeSound.
/// </summary>
/// <typeparam name="T">Le type contenant la data.</typeparam>
public abstract class AreaUpdater<T> : AreaUpdater// where T : MonoBehaviour
{
    [SerializeField] private float scoreProduced;

    protected T data;

    protected T lastUpdatedData;

    protected Area currentArea;

    protected Vector2 Position => new Vector2(transform.position.x, transform.position.z);

    bool asSpawnOnce = false;

    bool isApplicationQuitting = false;

    private void Start()
    {
        asSpawnOnce = true;
        SetData();
        AreaManager.AddAreaUpdater(this);
    }

    private void OnEnable()
    {
        if (asSpawnOnce)
        {
            SetData();
            AreaManager.AddAreaUpdater(this);
        }
    }

    private void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }

    private void OnDisable()
    {
        if (!isApplicationQuitting)
        {
            AreaManager.RemoveAreaUpdater(this);
        }
    }

    public void SetScore(float nScore)
    {
        scoreProduced = nScore;
    }

    public override float GetScore()
    {
        return scoreProduced;
    }

    public override void UpdateData()
    {
        if (data != null)
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
    }

    public override void RemoveData()
    {
        AreaManager.RemoveDataToArea<T>(currentArea, lastUpdatedData);
    }

    public abstract void SetData(T newData);
    public abstract void SetData();

    protected virtual void SetLastUpdateData(T lastData)
    {
        lastUpdatedData = lastData;
    }

    //protected abstract bool IsLastDataSame(T currentData, T lastData);
}
