using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public float minSpeed = .5f;
    public float maxSpeed = 1f;

    float speed = .001f;
    float rotationSpeed = 4f;
    public float neighbourDistance = 3f;

    Vector3 averageHeading;
    Vector3 averagePositon;

    bool turning = false;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, FishFlock.center) >= FishFlock.tankSize)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if (turning)
        {
            Vector3 direction = FishFlock.goalPos - transform.position;
            transform.rotation = Quaternion.Slerp
            (
                transform.rotation,
                Quaternion.LookRotation(direction),
                rotationSpeed * Time.deltaTime
            );
            speed = Random.Range(minSpeed, maxSpeed);
        }
        else
        {
            if (Random.Range(0, 5) < 1)
            {
                ApplyRules();
            }
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] fishes;
        fishes = FishFlock.fishes;

        Vector3 vCenter = FishFlock.goalPos;
        Vector3 vAvoid = FishFlock.goalPos;
        float gSpeed = 0.1f;

        Vector3 goalPos = FishFlock.goalPos;

        Debug.Log(FishFlock.goalPos + "+" + goalPos);

        float dist;

        int groupSize = 0;

        foreach(GameObject fish in fishes)
        {
            if(fish != this.gameObject)
            {
                dist = Vector3.Distance(fish.transform.position, this.transform.position);

                if(dist <= neighbourDistance)
                {
                    vCenter += fish.transform.position;
                    groupSize++;

                    if(dist < 1f)
                    {
                        vAvoid = vAvoid + (this.transform.position - fish.transform.position);
                    }

                    Flock anotherFlock = fish.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }

        if(groupSize > 0)
        {
            vCenter = vCenter / groupSize + (goalPos - this.transform.position);
            speed = gSpeed / groupSize;

            Vector3 direction = (vCenter + vAvoid) - transform.position;

            if(direction != FishFlock.center)
            {
                transform.rotation = Quaternion.Slerp
                (
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}
