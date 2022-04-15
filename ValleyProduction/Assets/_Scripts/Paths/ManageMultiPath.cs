using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageMultiPath : MonoBehaviour
{
    [SerializeField] private IST_PathPoint thisPathPoint;
    [SerializeField] private GameObject prefabSign;
    [SerializeField] private GameObject prefabCairn;
    [SerializeField] private GameObject arrowTagRef;
    [SerializeField] private GameObject pivot;

    private float offset = 0.2f;
    private GameObject currentArrow = null;
    private int nbArrow = 0;

    //List des chemins ou y'a un panneau
    private List<MultiPathClass> multiPathList = new List<MultiPathClass>();

    public class MultiPathClass
    {
        public GameObject arrowTag;
        public PathFragmentData pathFragmentData;

        public MultiPathClass(GameObject _arrowTag, PathFragmentData _pathFragmentData = null)
        {
            arrowTag = _arrowTag;
            pathFragmentData = _pathFragmentData;
        }
    }

    //Remplace le cairn par le panneau
    public void ActivateMultiPath()
    {
        if (!prefabSign.activeSelf)
        {
            prefabCairn.SetActive(false);
            prefabSign.SetActive(true);
            OrientateFirstTag();
        }
    }

    public void DesactivateMultiPath()
    {
        prefabCairn.SetActive(true);
        prefabSign.SetActive(false);
        nbArrow = 0;
    }

    //Call for the already existed PathData
    private void OrientateFirstTag()
    {
        //Orienter les 2 premiers panneaux 
        foreach (PathData pd in PathManager.GetAllPath)                                                                     //Pour chaque PathData
        {
            List<PathFragmentData> pathFragmentList = new List<PathFragmentData>(pd.GetPathFragments(thisPathPoint));       //Je Get les fragment ou il y'a le pathpoint (La liste est vide si il n'y a pas le point)
            if(pathFragmentList.Count > 0)
            {
                RegisterPathFragment(pathFragmentList);
            }
        }
    }

    public void SetRegisterPathFragment(List<PathFragmentData> pfdList)
    {
        RegisterPathFragment(pfdList);
    }

    /// <summary>
    /// Spawn ArrowTag and register pathFragment 
    /// </summary>
    /// <param name="pfdList"></param>
    private void RegisterPathFragment(List<PathFragmentData> pfdList)
    {
        foreach(MultiPathClass mpc in multiPathList)
        {
            foreach(PathFragmentData pfd in pfdList)
            {
                if(mpc.pathFragmentData == pfd)
                {
                    pfdList.Remove(pfd);
                }
            }
        }

        foreach (PathFragmentData pfd in pfdList)
        {
            GameObject newArrow = Instantiate(arrowTagRef, prefabSign.transform);
            newArrow.SetActive(true);
            newArrow.transform.position = new Vector3(pivot.transform.position.x, pivot.transform.position.y - (offset * nbArrow), pivot.transform.position.z);
            nbArrow++;

            if (pfd.startPoint == thisPathPoint)
            {
                newArrow.transform.forward = pfd.path[1] - newArrow.transform.position;
                newArrow.transform.eulerAngles = new Vector3(0f, newArrow.transform.eulerAngles.y, 0f);
            }
            else
            {
                newArrow.transform.forward = pfd.path[0] - newArrow.transform.position;
                newArrow.transform.eulerAngles = new Vector3(0f, newArrow.transform.eulerAngles.y, 0f);
            }

            multiPathList.Add(new MultiPathClass(newArrow, pfd));
        }
    }

    public GameObject SpawnNewTag()
    {
        return Instantiate(arrowTagRef, prefabSign.transform);
    }

    public void CheckIfMultiPath(PathData pd)
    {
        if(multiPathList.Count > 0)
        {
            DeleteArrow(pd);
        }
    }

    public void DeleteArrow(PathData pd)
    {
        /*List<MultiPathClass> toDeleteList = new List<MultiPathClass>();
        foreach(MultiPathClass mpc in multiPathList)
        {
            if(mpc.pathData == pd)
            {
                Destroy(mpc.arrowTag.gameObject);
                toDeleteList.Add(mpc);
            }
        }

        foreach(MultiPathClass mpc in toDeleteList)
        {
            multiPathList.Remove(mpc);
        }

        if (multiPathList.Count < 2)
        {
            DesactivateMultiPath();
            Destroy(multiPathList[0].arrowTag.gameObject);
            multiPathList.Clear();
        }*/
    }
}
