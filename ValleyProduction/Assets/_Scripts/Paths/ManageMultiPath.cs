using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMultiPath : MonoBehaviour
{
    [SerializeField] private IST_PathPoint pathPoint;
    [SerializeField] private GameObject prefabSign;
    [SerializeField] private GameObject prefabCairn;
    [SerializeField] private GameObject arrowTagRef;

    private float offset = 0.4f;
    private GameObject currentArrow = null;
    private int nbArrow = 0;

    private List<MultiPathClass> multiPathList = new List<MultiPathClass>();

    public class MultiPathClass
    {
        public GameObject arrowTag;
        public PathData pathData;

        public MultiPathClass(GameObject _arrowTag, PathData _pathdata = null)
        {
            arrowTag = _arrowTag;
            pathData = _pathdata;
        }
    }

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
        multiPathList.Add(new MultiPathClass(arrowTagRef, null));
        PathManager.manageMultiPath = this;
        nbArrow++;
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
                nbArrow++;
                arrowTagRef.transform.right = pfd.path[1] - arrowTagRef.transform.position;
                arrowTagRef.transform.eulerAngles = new Vector3(0f, arrowTagRef.transform.eulerAngles.y, 0f);
                multiPathList.Add(new MultiPathClass(arrowTagRef, pd));
            }
        }
    }

    public GameObject SpawnNewTag()
    {
        return Instantiate(arrowTagRef, prefabSign.transform);
    }

    public void AddPathDataToList(PathData pd)
    {
        foreach(MultiPathClass mpc in multiPathList)
        {
            if(mpc.pathData == null)
            {
                mpc.pathData = pd;
                PathManager.manageMultiPath = null;
                return;
            }
        }
    }

    public void DeleteArrow()
    {
        Debug.Log("bite");
    }
}
