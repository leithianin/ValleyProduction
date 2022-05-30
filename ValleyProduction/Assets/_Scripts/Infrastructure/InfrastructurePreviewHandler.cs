using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructurePreviewHandler : MonoBehaviour
{
    private InfrastructurePreview currentPreview;
    public bool snaping = false;
    public bool isRotating = false;

    public float rotateSpeed = 50f;

    public InfrastructurePreview GetPreview => currentPreview;

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
            Debug.Log(isRotating);
            if (!isRotating)
            {
                transform.position = currentPreview.TrySetPosition();
                currentPreview.CheckAvailability();
            }
            else
            {
                Rotate();
            }
        }
    }

    public void Rotate()
    {
        CursorControl.SetAtSaveMousePosition();                                                                                 //CODE REVIEW : Block la position à cette position (LockState.Locked met au centre et peut poser des problemes) Autre moyen d'éviter de faire ça serait d'empêcher les snap pendant la rotation )
        transform.Rotate(0f, currentPreview.TrySetRotation(), 0, Space.World);
    }
}
