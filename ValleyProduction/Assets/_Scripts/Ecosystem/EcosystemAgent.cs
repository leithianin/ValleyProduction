using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EcosystemAgent : MonoBehaviour
{
    [SerializeField] private float range;

    public float Range => range;

    public abstract float GetScore();

    public abstract EcosystemDataType UsedDataType();
}

/// <summary>
/// Gère l'Update d'une data d'un objet de type T. Exemple : Voir AU_MakeSound.
/// </summary>
/// <typeparam name="T">Le type contenant la data.</typeparam>
public abstract class EcosystemAgent<T> : EcosystemAgent// where T : MonoBehaviour
{
    [SerializeField] private float scoreProduced;

    protected T data;

    protected Vector2 Position => new Vector2(transform.position.x, transform.position.z);

    bool asSpawnOnce = false;

    bool isApplicationQuitting = false;

    private void Start()
    {
        asSpawnOnce = true;
        SetData();
        VLY_EcosystemManager.AddAgent(this);
    }

    private void OnEnable()
    {
        if (asSpawnOnce)
        {
            SetData();
            VLY_EcosystemManager.AddAgent(this);
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
            VLY_EcosystemManager.RemoveAgent(this);
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

    public abstract void SetData(T newData);
    public abstract void SetData();
}
