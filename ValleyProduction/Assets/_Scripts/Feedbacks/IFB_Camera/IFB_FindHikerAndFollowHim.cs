using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_FindHikerAndFollowHim : MonoBehaviour, IFeedbackPlayer
{
    private Camera cam;
    private Transform origin;

    private GameObject hiker;

    private bool isFollowing = false;

    private void Start()
    {
        cam = Camera.main;
        origin = cam.transform.parent.GetChild(0).transform;
    }

    private void Update()
    {
        if(isFollowing)
        {
            if(hiker.activeSelf)
            {
                origin.transform.position = hiker.transform.position;
            }
            else
            {
                Play();
            }
        }
    }

    public void Play()
    {
        Debug.Log("Find Hiker");
        hiker = VisitorManager.FindActiveHiker();

        if (hiker != null)
        {
            isFollowing = true;
        }
    }

    public void End()
    {
        Debug.Log("end hiker");
        isFollowing = false;
    }
}
