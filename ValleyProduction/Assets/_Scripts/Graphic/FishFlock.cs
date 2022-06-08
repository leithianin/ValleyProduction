using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlock : MonoBehaviour
{
    public GameObject fishPrefab;

    public int numFish = 10;
    [HideInInspector] public GameObject[] fishes;

    public Vector3 tankSize;

    [HideInInspector] public Vector3 goalPos = Vector3.zero;

    public GameObject goalPosPrefab;
    [SerializeField] private Vector2 refreshPosRandom;

    //TimerManager.Timer refreshTargetTimer = null;

    [HideInInspector] public Vector3 center = Vector3.zero;

    void Start()
    {
        center = this.transform.position;
        fishes = new GameObject[numFish];

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3
            (
                Random.Range(center.x - tankSize.x, center.x + tankSize.x),
                Random.Range(center.y - tankSize.y, center.y + tankSize.y),
                Random.Range(center.z - tankSize.z, center.z + tankSize.z)
            );
            fishes[i] = Instantiate(fishPrefab, pos, Quaternion.identity, gameObject.transform);
            fishes[i].GetComponent<Flock>().fishFlock = this;
        }

        RefreshTarget();
    }

    private void RefreshTarget()
    {
        center = this.transform.position;

        goalPos = new Vector3
        (
            Random.Range(center.x - tankSize.x, center.x + tankSize.x),
            Random.Range(center.y - tankSize.y, center.y + tankSize.y),
            Random.Range(center.z - tankSize.z, center.z + tankSize.z)
        );
        goalPosPrefab.transform.position = goalPos;

        TimerManager.CreateGameTimer(Random.Range(refreshPosRandom.x, refreshPosRandom.y), RefreshTarget);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(transform.position, new Vector3(tankSize.x * 2, tankSize.y * 2, tankSize.z * 2));
    }
}
