using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLayers : MonoBehaviour
{
    [SerializeField] private LayerMask usableLayers;

    [SerializeField] private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera.eventMask = usableLayers;
    }
}
