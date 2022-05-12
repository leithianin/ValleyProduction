using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PebbleSpawner : MonoBehaviour
{
    public List<GameObject> pebbleList = new List<GameObject>();
    public SphereCollider collider;

    public void OnEnable()
    {
        InstantiatePebbles();
    }

    public void InstantiatePebbles()
    {
        int randomNb = Random.Range(0, 101);

        if (randomNb > 20)
        {
            Instantiate(pebbleList[Random.Range(0, 3)], collider.center, Quaternion.identity, gameObject.transform);
        }
    }
}
