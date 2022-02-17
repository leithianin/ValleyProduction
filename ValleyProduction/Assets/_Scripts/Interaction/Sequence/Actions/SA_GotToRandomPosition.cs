using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SA_GotToRandomPosition : InteractionActions
{
    private struct UsedPoint
    {
        public CPN_InteractionHandler caller;
        public Transform point;

        public UsedPoint(CPN_InteractionHandler c, Transform p)
        {
            caller = c;
            point = p;
        }
    }

    [SerializeField] private List<Transform> possiblePoints;

    private List<UsedPoint> pointUsed;

    protected override void OnPlayAction(CPN_InteractionHandler caller)
    {
        CPN_Movement movement = null;
        if (caller.HasComponent<CPN_Movement>(ref movement) && possiblePoints.Count > 0)
        {
            List<Vector3> vectorPath = new List<Vector3>();
            vectorPath.Add(caller.transform.position);

            Transform wantedPoint = possiblePoints[Random.Range(0, possiblePoints.Count)];
            pointUsed.Add(new UsedPoint(caller, wantedPoint));
            possiblePoints.Remove(wantedPoint);

            vectorPath.Add(wantedPoint.position);
            
            movement.WalkOnNewPath(vectorPath, () => EndAction(caller));
        }
        else
        {
            EndAction(caller);
        }
    }

    protected override void OnEndAction(CPN_InteractionHandler caller)
    {
        for(int i = 0; i < pointUsed.Count; i++)
        {
            if(pointUsed[i].caller == caller)
            {
                possiblePoints.Add(pointUsed[i].point);
                pointUsed.RemoveAt(i);
                break;
            }
        }
    }

    protected override void OnInteruptAction(CPN_InteractionHandler caller)
    {
        CPN_Movement movement = null;
        if (caller.HasComponent<CPN_Movement>(ref movement))
        {
            movement.InteruptWalk();
            for (int i = 0; i < pointUsed.Count; i++)
            {
                if (pointUsed[i].caller == caller)
                {
                    possiblePoints.Add(pointUsed[i].point);
                    pointUsed.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
