using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        //postProcessManager.SetProfile(postProcessManager.GetPhotoProfile((int)profile));
    
    }

    #region Bloom
    private void EnableBloom(bool enable)
    {
        postProcessManager.Bloom.active = enable;
    }
    #endregion

    #region ChannelMixer
    private void EnableChannelMixer(bool enable)
    {
        postProcessManager.ChannelMixer.active = enable;
    }
    #endregion

    #region ChromaticAbberation
    private void EnableChromaticAbberation(bool enable)
    {
        postProcessManager.ChromaticAbberration.active = enable;
    }
    #endregion

    #region ColorAdjustments
    private void EnableColorAdjustments(bool enable)
    {
        postProcessManager.ColorAdjustments.active = enable;
    }
    #endregion

    #region ColorCurves
    private void EnableColorCurves(bool enable)
    {
        postProcessManager.ColorCurves.active = enable;
    }
    #endregion

    #region ColorLookup
    private void EnableColorLookup(bool enable)
    {
        postProcessManager.ColorLookup.active = enable;
    }
    #endregion

    #region DepthOfField
    private void EnableDepthOfField(bool enable)
    {
        postProcessManager.DepthOfField.active = enable;
    }
    #endregion

    #region FilmGrain
    private void EnableFilmGrain(bool enable)
    {
        postProcessManager.FilmGrain.active = enable;
    }
    #endregion

    #region LensDistortion
    private void EnableLensDistortion(bool enable)
    {
        postProcessManager.LensDistortion.active = enable;
    }
    #endregion

    #region LiftGammaGain
    private void EnableLiftGammaGain(bool enable)
    {
        postProcessManager.LiftGammaGain.active = enable;
    }
    #endregion

    #region MotionBlur
    private void EnableMotionBlur(bool enable)
    {
        postProcessManager.MotionBlur.active = enable;
    }
    #endregion

    #region PaniniProjection
    private void EnablePaniniProjection(bool enable)
    {
        postProcessManager.PaniniProjection.active = enable;
    }
    #endregion

    #region ShadowsMidtonesHighlights
    private void EnableShadowsMidtonesHighlights(bool enable)
    {
        postProcessManager.ShadowsMidtonesHighlights.active = enable;
    }
    #endregion

    #region SplitToning
    private void EnableSplitToning(bool enable)
    {
        postProcessManager.SplitToning.active = enable;
    }
    #endregion

    #region Tonemapping
    private void EnableTonemapping(bool enable)
    {
        postProcessManager.Tonemapping.active = enable;
    }
    #endregion

    #region Vignette
    private void EnableVignette(bool enable)
    {
        postProcessManager.Vignette.active = enable;
    }
    #endregion

    #region WhiteBalance
    private void EnableWhiteBalance(bool enable)
    {
        postProcessManager.WhiteBalance.active = enable;
    }
    #endregion
}
