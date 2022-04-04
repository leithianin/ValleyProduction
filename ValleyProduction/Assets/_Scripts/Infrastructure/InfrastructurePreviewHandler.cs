using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructurePreviewHandler : MonoBehaviour
{
    private InfrastructurePreview currentPreview;
    public bool snaping = false;
    public bool isRotating = false;

    public float rotateSpeed = 5f;

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
            if (!isRotating)
            {
                transform.position = PlayerInputManager.GetMousePosition;
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
        Debug.Log("Rotate");
        Debug.Log(Input.GetAxis("Mouse X"));
        //transform.Rotate((Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime), (Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime), 0, Space.World);
        //transform.Rotate(PlayerInputManager.GetOnMouseMove. * rotateSpeed * Time.deltaTime), (Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime), 0, Space.World);
    }
}
