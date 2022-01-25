using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfrastructurePreviewHandler : MonoBehaviour
{
    private InfrastructurePreview currentPreview;
    public bool snaping = false;

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
            transform.position = PlayerInputManager.GetMousePosition;
            currentPreview.CheckAvailability();
        }
    }
}
