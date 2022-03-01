using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeVisitorsPath : MonoBehaviour
{
    public OB_AvoidForest avoidForest_ob;
    public List<CPN_Movement> movementList;
    public void StartWalking()
    {
        foreach(CPN_Movement movement in movementList)
        {
            movement.WalkOnNewPath(avoidForest_ob.vectorList);
        }
    }
}
