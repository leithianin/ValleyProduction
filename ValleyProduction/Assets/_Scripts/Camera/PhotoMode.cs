using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Rendering;

public enum VolumeProfilesEnum
{
    None,
    Sepia
}

public class PhotoMode : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera = default;
    [SerializeField] private Transform playerCameraTransform = default;
    [SerializeField] private SphericalTransform sphericalTransform = default;
    [SerializeField] private PostProcessManager postProcessManager = default;
    [SerializeField] private GameObject ui = default;

    [SerializeField, ReadOnly] private bool active = false;

    [Header("Parameters")]
    [SerializeField, Range(10, 300)] private float focalLength;
    private float baseFocalLength = 21f;

    [SerializeField, Range(0, 10)] private float verticalOffset;
    float baseVerticalOffset;
    [SerializeField, Range(-90, 90)] private float roll;

    [Header("Depth of Field"), Space(10)]
    [SerializeField] private bool useDepthOfField;
    [SerializeField, Range(0,100)] private float focusDistance;

    [Header("Post Effects"), Space(10)]
    [SerializeField, Range(-10, 10)] private float exposure;
    [SerializeField, Range(-100, 100)] private float contrasts;
    [SerializeField, Range(-100, 100)] private float saturation;
    [SerializeField, Range(0, 5)] private float bloom;
    [SerializeField, Range(0, 100)] private float grain;
    [SerializeField, Range(0, 100)] private float vignette;

    [Header("Filters"), Space(10)]
    [SerializeField] private VolumeProfilesEnum filtersSelection;
    [SerializeField, Range(0,100)] private float filterIntensity = 100.0f;
    [SerializeField] private Texture[] filters = default;

    [Header("Events"), Space(10)]
    [SerializeField] private UnityEvent enablePhotoMode;
    [SerializeField] private UnityEvent disablePhotoMode;


    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        SetBloom(bloom);

        SetPostExposure(exposure);
        SetContrasts(contrasts);
        SetSaturation(saturation);
        SetFilmGrain(grain);
        SetVignette(vignette);

        SetColorLookup(filtersSelection);
        SetFilterIntensity(filterIntensity);

        UseDepthOfField(useDepthOfField);
        SetFocusDistance(focusDistance);

        SetFocalLength();
        SetVerticalOffset(verticalOffset);
        SetRolling(roll);
    }

    [Button]
    public void EnablePhotoMode()
    {
        enablePhotoMode.Invoke();

        focalLength = playerCamera.focalLength;
        baseVerticalOffset = sphericalTransform.OriginVisualOffset;
        postProcessManager.Volume.profile = postProcessManager.PhotoProfile;
        postProcessManager.GetProfileOverrides();

        active = true;
    }

    [Button]
    public void DisablePhotoMode()
    {
        active = false;
        disablePhotoMode.Invoke();

        playerCamera.focalLength = baseFocalLength;
        sphericalTransform.OriginVisualOffset = baseVerticalOffset;
        SetRolling(0.0f);
        postProcessManager.Volume.profile = postProcessManager.DefaultProfile;

    }

    private void SetFocalLength()
    {
        playerCamera.focalLength = Mathf.Lerp(playerCamera.focalLength, focalLength, 0.9f);
    }

    private void SetRolling(float value)
    {
        //transform.rotation = Quaternion.FromToRotation()
        playerCameraTransform.eulerAngles = new Vector3(playerCameraTransform.eulerAngles.x, playerCameraTransform.eulerAngles.y, value);
    }

    #region VerticalOffset
    public void SetVerticalOffset(float value)
    {
        sphericalTransform.OriginVisualOffset = value;
    }
    #endregion

    #region Bloom
    private void EnableBloom(bool enable)
    {
        postProcessManager.Bloom.active = enable;
    }

    public void SetBloom(float value)
    {
        postProcessManager.Bloom.intensity.Override(value);
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
    public void EnableColorAdjustments(bool enable)
    {
        postProcessManager.ColorAdjustments.active = enable;
    }

    public void SetPostExposure(float value)
    {
        postProcessManager.ColorAdjustments.postExposure.Override(value);
    }

    public void SetContrasts(float value)
    {
        postProcessManager.ColorAdjustments.contrast.Override(value);
    }

    public void SetSaturation(float value)
    {
        postProcessManager.ColorAdjustments.saturation.Override(value);
    }
    #endregion

    #region ColorCurves
    private void EnableColorCurves(bool enable)
    {
        postProcessManager.ColorCurves.active = enable;
    }
    #endregion

    #region ColorLookup
    public void EnableColorLookup(bool enable)
    {
        postProcessManager.ColorLookup.active = enable;
    }

    public void SetColorLookup(VolumeProfilesEnum profile)
    {
        switch (profile)
        {
            case VolumeProfilesEnum.None:
                postProcessManager.ColorLookup.texture.Override(null);
                break;

            case VolumeProfilesEnum.Sepia:
                postProcessManager.ColorLookup.texture.Override(filters[0]);
                break;
        }
    }

    public void SetFilterIntensity(float value)
    {
        postProcessManager.ColorLookup.contribution.Override(value / 100.0f);
    }
    #endregion

    #region DepthOfField
    private void EnableDepthOfField(bool enable)
    {
        postProcessManager.DepthOfField.active = enable;
    }

    public void UseDepthOfField(bool value)
    {
        if (value)
        {
            postProcessManager.DepthOfField.mode.Override(UnityEngine.Rendering.Universal.DepthOfFieldMode.Bokeh);
        }
        else
        {
            postProcessManager.DepthOfField.mode.Override(UnityEngine.Rendering.Universal.DepthOfFieldMode.Off);
        }
    }

    public void SetFocusDistance(float value)
    {
        if (postProcessManager.DepthOfField.mode.value == UnityEngine.Rendering.Universal.DepthOfFieldMode.Bokeh)
        {
            postProcessManager.DepthOfField.focalLength.Override(playerCamera.focalLength);
            postProcessManager.DepthOfField.focusDistance.Override(value);
        }

    }
    #endregion

    #region FilmGrain
    private void EnableFilmGrain(bool enable)
    {
        postProcessManager.FilmGrain.active = enable;
    }

    public void SetFilmGrain(float value)
    {
        postProcessManager.FilmGrain.intensity.Override(value / 100.0f);
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

    public void SetVignette(float value)
    {
        postProcessManager.Vignette.intensity.Override(value / 100.0f);
    }
    #endregion

    #region WhiteBalance
    private void EnableWhiteBalance(bool enable)
    {
        postProcessManager.WhiteBalance.active = enable;
    }
    #endregion
}
