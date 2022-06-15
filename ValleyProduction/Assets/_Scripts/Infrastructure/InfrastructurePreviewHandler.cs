using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InfrastructurePreviewHandler : MonoBehaviour
{
    private InfrastructurePreview currentPreview;
    public bool snaping = false;
    public bool isRotating = false;

    public float rotateSpeed = 50f;

    public InfrastructurePreview GetPreview => currentPreview;

    public bool Snaping => snaping;

    private bool isDisplayShown;

    public void SetSnaping(bool toSet)
    {
        snaping = toSet;
        if(currentPreview != null)
        {
            currentPreview.SetSnap(toSet);
        }
    }

    public void SetInfrastructurePreview(InfrastructurePreview preview)
    {
        if (preview != null)
        {
            gameObject.SetActive(true);

            if(preview != currentPreview)
            {
                if (currentPreview != null)
                {
                    currentPreview.DespawnObject();
                }

                currentPreview = Instantiate(preview, transform.position, Quaternion.identity, transform);

                currentPreview.transform.localPosition = Vector3.zero;

                currentPreview.SpawnObject(transform.position);

                if(!isDisplayShown)
                {
                    currentPreview.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if(currentPreview != null)
            {
                currentPreview.DespawnObject();
            }
            currentPreview = null;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!snaping)
        {
            currentPreview.CheckAvailability();
            if (!isRotating)
            {
                transform.position = currentPreview.TrySetPosition();
            }
            else
            {
                Rotate();
            }
        }
    }
    
    public void ShowDisplay()
    {
        isDisplayShown = true;
        if (currentPreview != null)
        {
            currentPreview.gameObject.SetActive(true);
        }
    }

    public void HideDisplay()
    {
        isDisplayShown = false;
        if (currentPreview != null)
        {
            currentPreview.gameObject.SetActive(false);
        }
    }

    public void Rotate()
    {
        CursorControl.SetAtSaveMousePosition();                                                                                 //CODE REVIEW : Block la position � cette position (LockState.Locked met au centre et peut poser des problemes) Autre moyen d'�viter de faire �a serait d'emp�cher les snap pendant la rotation )
        transform.Rotate(0f, currentPreview.TrySetRotation(), 0, Space.World);
    }
}
