using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRendererManager : MonoBehaviour
{
    [Header("PathRenderer")]
    public GameObject pathRendererObject;
    public List<GameObject> pathRendererList = new List<GameObject>();

    public void ManagePathRenderer(IST_PathPoint start, IST_PathPoint end, List<Vector3> vecs)
    {
        for(int i = 0; i < vecs.Count-1; i++)
        {
            GameObject go = Instantiate(pathRendererObject, vecs[i], Quaternion.identity);
            go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y, (Vector3.Distance(vecs[i], vecs[i+1])));
            go.transform.LookAt(end.transform);
        }
    }
}
