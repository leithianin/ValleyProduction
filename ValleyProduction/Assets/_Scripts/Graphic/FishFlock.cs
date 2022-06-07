using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlock : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject goalPosPrefab;
    public static int tankSize = 5;

    public bool patrol;
    public Transform[] patrolPoints;

    static int numFish = 10;
    public static GameObject[] fishes = new GameObject[numFish];

    public static Vector3 goalPos = Vector3.zero;

    [SerializeField] private Vector2 refreshPosRandom;

    //TimerManager.Timer refreshTargetTimer = null;

    public static Vector3 center = Vector3.zero;

    void Start()
    {
        center = this.transform.position;

        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3
            (
                Random.Range(center.x - tankSize, center.x + tankSize),
                Random.Range(center.y - tankSize, center.y + tankSize),
                Random.Range(center.z - tankSize, center.z + tankSize)
            );
            fishes[i] = Instantiate(fishPrefab, pos, Quaternion.identity/*, gameObject.transform*/);
        }

        RefreshTarget();
    }

    private void RefreshTarget()
    {
        center = this.transform.position;

        goalPos = new Vector3
        (
            Random.Range(center.x - tankSize, center.x + tankSize),
            Random.Range(center.y - tankSize, center.y + tankSize),
            Random.Range(center.z - tankSize, center.z + tankSize)
        );
        goalPosPrefab.transform.position = goalPos;

        TimerManager.CreateGameTimer(Random.Range(refreshPosRandom.x, refreshPosRandom.y), RefreshTarget);
    }
}
