using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRendererManager : MonoBehaviour
{
    [Header("PathRenderer")]
    public GameObject pathRendererObject;
    public List<GameObject> pathRendererList = new List<GameObject>();

    /// <summary>
    /// Create the PathRenderer beetwen points
    /// </summary>
    /// <param name="start">t</param>
    /// <param name="end"></param>
    /// <param name="vecs"></param>
    /// <param name="pfd"></param>
    public void ManagePathRenderer(List<Vector3> vecs, PathFragmentData pfd)
    {
        for(int i = 0; i < vecs.Count-1; i++)
        {
            GameObject go = Instantiate(pathRendererObject, vecs[i], Quaternion.identity);
            go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y, (Vector3.Distance(vecs[i], vecs[i+1])/2));
            go.transform.LookAt(new Vector3(vecs[i + 1].x, 0, vecs[i + 1].z));
            pfd.pathRendererObject.Add(go);
        }
    }

    public static void DeletePathRenderer(PathFragmentData pfd)
    {
        foreach(GameObject go in pfd.pathRendererObject)
        {
            Destroy(go);
        }
    }
}
