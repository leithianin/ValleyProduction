using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMultiPath : MonoBehaviour
{
    [SerializeField] private IST_PathPoint pathPoint;
    [SerializeField] private GameObject prefabSign;
    [SerializeField] private GameObject prefabCairn;
    [SerializeField] private GameObject arrowTagRef;

    //public List<ArrowTagClass> arrowTagClassList = new List<ArrowTagClass>();
    private float offset = 0.4f;
    private GameObject currentArrow = null;
    private int nbArrow = 0;

    /*public class ArrowTagClass
    {
        public GameObject arrowTag;
        public PathData pathData;

        public ArrowTagClass(GameObject _arrowTag, PathData _pathData = null)
        {
            arrowTag = _arrowTag;
            pathData = _pathData;
        }
    }*/

    private void Update()
    {
        if (pathPoint == PathManager.previousPathpoint && currentArrow != null)
        {
            if(PathCreationManager.navmeshPositionsList.Count > 0)
            {
                Vector3 posTar = PathCreationManager.navmeshPositionsList[1];
                currentArrow.transform.right = posTar - currentArrow.transform.position;
                currentArrow.transform.eulerAngles = new Vector3(0f, currentArrow.transform.eulerAngles.y, 0f);
            }
        }
    }

    //Remplace le cairn par le panneau
    public void ActivateMultiPath()
    {
        if (!prefabSign.activeSelf)
        {
            prefabCairn.SetActive(false);
            prefabSign.SetActive(true);
            OrientateTag();
        }

        TagFollowCurrentPath();
    }

    public void DesactivateMultiPath()
    {
        prefabCairn.SetActive(false);
        prefabSign.SetActive(true);
    }

    //Tag qui follow le chemin entrain d'être crée
    private void TagFollowCurrentPath()
    {
        currentArrow = SpawnNewTag();
        currentArrow.transform.position = new Vector3(arrowTagRef.transform.position.x, arrowTagRef.transform.position.y - (offset * nbArrow), arrowTagRef.transform.position.z);
        nbArrow++;
        //arrowTagClassList.Add(new ArrowTagClass(currentArrow, null));
    }

    //Call for the already existed PathData
    private void OrientateTag()
    {
        foreach(PathData pd in PathManager.GetAllPath)
        {
            if(pd.ContainsPoint(pathPoint))
            {
                RegisterPathData(pd);
            }
        }
    }

    private void RegisterPathData(PathData pd)
    {
        arrowTagRef.transform.position = new Vector3(arrowTagRef.transform.position.x, arrowTagRef.transform.position.y - (offset* nbArrow), arrowTagRef.transform.position.z);
        
        foreach(PathFragmentData pfd in pd.pathFragment)
        {
            if(pfd.HasThisStartingPoint(pathPoint))
            {
                //arrowTagClassList.Add(new ArrowTagClass(arrowTagRef, pd));
                nbArrow++;
                arrowTagRef.transform.right = pfd.path[1] - arrowTagRef.transform.position;
                arrowTagRef.transform.eulerAngles = new Vector3(0f, arrowTagRef.transform.eulerAngles.y, 0f);
            }
        }
    }

    public GameObject SpawnNewTag()
    {
        return Instantiate(arrowTagRef, prefabSign.transform);
    }
}
