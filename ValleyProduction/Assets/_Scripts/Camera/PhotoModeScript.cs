using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum VolumeProfilesEnum
{
    None,
    BlackAndWhite,
    Sepia
}

public class PhotoModeScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera = default;
    [SerializeField] private Transform playerCameraTransform = default;
    [SerializeField] private SphericalTransform sphericalTransform = default;
    [SerializeField] private PostProcessManager postProcessManager = default;
    [SerializeField] private GameObject ui = default;

    [SerializeField] private bool active = false;

    [Header("Parameters")]
    [SerializeField, Range(10, 300)] private float focalLength;
    private float baseFocalLength = 21f;

    [SerializeField] private float verticalOffset;
    [SerializeField, Range(-90, 90)] private float roll;

    [SerializeField] private VolumeProfilesEnum profile;

    [SerializeField] private float depthOfField;
    [SerializeField] private float focusDistance;
    [SerializeField] private float exposure;
    [SerializeField] private float constrasts;
    [SerializeField] private float colorGradingIntensity;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        SetFocalLength();
        SetRolling();
        SetProfile();
    }

    private void SetFocalLength()
    {
        playerCamera.focalLength = Mathf.Lerp(playerCamera.focalLength, focalLength, 0.9f);
    }

    private void SetRolling()
    {
        //transform.rotation = Quaternion.FromToRotation()
        playerCameraTransform.eulerAngles = new Vector3(playerCameraTransform.eulerAngles.x, playerCameraTransform.eulerAngles.y, roll);
    }

    private void SetProfile()
    {       
        postProcessManager.SetProfile(postProcessManager.GetPhotoProfile((int)profile));
    
    }
}
