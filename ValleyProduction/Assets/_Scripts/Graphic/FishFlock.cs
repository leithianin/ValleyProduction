using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFlock : MonoBehaviour
{
    public GameObject fishPrefab;
    public static int tankSize = 5;

    static int numFish = 10;
    public static GameObject[] fishes = new GameObject[numFish];

    public static Vector3 goalPos = Vector3.zero;

    void Start()
    {
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = new Vector3
            (
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize)
            );
            fishes[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
        }
    }

    void Update()
    {
        if(Random.Range(0, 10000) < 50)
        {
            goalPos = new Vector3
            (
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize),
                Random.Range(-tankSize, tankSize)
            );
        }
    }
}
