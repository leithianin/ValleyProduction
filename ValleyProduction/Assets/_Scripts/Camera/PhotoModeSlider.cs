using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ModePhoto { Filter, Focal_Length, Grain, Offset, Roll, Saturation, Vignette, Bloom, Contrasts, DOF, Exposure}

public class PhotoModeSlider : MonoBehaviour
{
    public ModePhoto currentMode = ModePhoto.Filter;
    [SerializeField] private PostProcessManager postProcessManager;
    public PhotoMode photoModeScript;
    public Slider slider;

    public void OnChangeValue(float value)
    {
        switch(currentMode)
        {
            case ModePhoto.Filter:
                photoModeScript.SetFilterIntensity(value);
                break;
            case ModePhoto.Focal_Length:
                photoModeScript.SetFocalLength(value);
                break;
            case ModePhoto.Grain:
                photoModeScript.SetFilmGrain(value);
                break;
            case ModePhoto.Offset:
                photoModeScript.SetVerticalOffset(value);
                break;
            case ModePhoto.Roll:
                photoModeScript.SetRolling(value);
                break;
            case ModePhoto.Saturation:
                photoModeScript.SetSaturation(value);
                break;
            case ModePhoto.Vignette:
                photoModeScript.SetVignette(value);
                break;
            case ModePhoto.Bloom:
                photoModeScript.SetBloom(value);
                break;
            case ModePhoto.Contrasts:
                photoModeScript.SetContrasts(value);
                break;
            case ModePhoto.DOF:
                photoModeScript.SetFocusDistance(value);
                break;
            case ModePhoto.Exposure:
                photoModeScript.SetPostExposure(value);
                break;
        }
    }

    public void SetCurrentMode(int i)
    {
        currentMode = (ModePhoto)i;

        switch (currentMode)
        {
            case ModePhoto.Filter:
                slider.value = postProcessManager.ColorLookup.contribution.value;
                break;
            case ModePhoto.Focal_Length:
                slider.value = photoModeScript.playerCamera.focalLength / 10.0f;
                break;
            case ModePhoto.Grain:
                slider.value = postProcessManager.FilmGrain.intensity.value;
                break;
            case ModePhoto.Offset:
                slider.value = photoModeScript.sphericalTransform.OriginVisualOffset / 10.0f;
                break;
            case ModePhoto.Roll:
                slider.value = photoModeScript.playerCameraTransform.eulerAngles.z;
                break;
            case ModePhoto.Saturation:
                slider.value = postProcessManager.ColorAdjustments.saturation.value;
                break;
            case ModePhoto.Vignette:
                slider.value = postProcessManager.Vignette.intensity.value;
                break;
            case ModePhoto.Bloom:
                slider.value = postProcessManager.Bloom.intensity.value;
                break;
            case ModePhoto.Contrasts:
                slider.value = postProcessManager.ColorAdjustments.contrast.value;
                break;
            case ModePhoto.DOF:
                slider.value = postProcessManager.DepthOfField.focusDistance.value / 10.0f;
                break;
            case ModePhoto.Exposure:
                slider.value = postProcessManager.ColorAdjustments.postExposure.value;
                break;
        }
    }

}
