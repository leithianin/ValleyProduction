using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMultiPath : MonoBehaviour
{
    [SerializeField] private IST_PathPoint pathPoint;
    [SerializeField] private GameObject prefabSign;
    [SerializeField] private GameObject prefabCairn;
    [SerializeField] private GameObject arrowTagRef;

    public List<ArrowTagClass> arrowTagClassList = new List<ArrowTagClass>();
    private float offset = 0.4f;
    private GameObject currentArrow = null;

    public class ArrowTagClass
    {
        public GameObject arrowTag;
        public PathData pathData;

        public ArrowTagClass(GameObject _arrowTag, PathData _pathData = null)
        {
            arrowTag = _arrowTag;
            pathData = _pathData;
        }
    }

    private void Update()
    {
        if (pathPoint == PathManager.previousPathpoint)
        {
            if(PathCreationManager.navmeshPositionsList.Count > 0)
            {
                Vector3 posTar = PathCreationManager.navmeshPositionsList[1];
                currentArrow.transform.right = posTar - currentArrow.transform.position;
                currentArrow.transform.eulerAngles = new Vector3(0f, currentArrow.transform.eulerAngles.y, 0f);

            }
            //arrowTagRef.transform.LookAt(new Vector3(test.transform.position.x, test.transform.position.y, test.transform.position.z));
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

    private void TagFollowCurrentPath()
    {
        currentArrow = SpawnNewTag();
        currentArrow.transform.position = new Vector3(arrowTagRef.transform.position.x, arrowTagRef.transform.position.y - (offset * arrowTagClassList.Count), arrowTagRef.transform.position.z);
        arrowTagClassList.Add(new ArrowTagClass(arrowTagRef, null));
    }

    //Call for the already existed PathData
    private void OrientateTag()
    {
        foreach(PathData pd in PathManager.GetAllPath)
        {
            if(pd.ContainsPoint(pathPoint))
            {
                CheckIfAlreadyRegistered(pd);
            }
        }
    }

    private bool CheckIfAlreadyRegistered(PathData pd)
    {
        foreach(ArrowTagClass atc in arrowTagClassList)
        {
            if(atc.pathData == pd)
            {
                return true;
            }
        }

        RegisterPathData(pd);
        return false;
    }

    private void RegisterPathData(PathData pd)
    {
        arrowTagRef.transform.position = new Vector3(arrowTagRef.transform.position.x, arrowTagRef.transform.position.y - (offset*arrowTagClassList.Count), arrowTagRef.transform.position.z);
        
        foreach(PathFragmentData pfd in pd.pathFragment)
        {
            if(pfd.HasThisStartingPoint(pathPoint))
            {
                arrowTagClassList.Add(new ArrowTagClass(arrowTagRef, pd));
                arrowTagRef.transform.right = pfd.path[1] - arrowTagRef.transform.position;
                arrowTagRef.transform.eulerAngles = new Vector3(0f, arrowTagRef.transform.eulerAngles.y, 0f);
                Instantiate(arrowTagRef, pfd.path[1], Quaternion.identity);
            }
        }
    }

    public GameObject SpawnNewTag()
    {
        return Instantiate(arrowTagRef, prefabSign.transform);
    }

    private void ResetArrowTag()
    {

    }
}
