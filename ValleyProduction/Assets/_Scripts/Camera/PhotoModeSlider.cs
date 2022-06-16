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
                
                break;
            case ModePhoto.Focal_Length:
                
                break;
            case ModePhoto.Grain:
                
                break;
            case ModePhoto.Offset:
                
                break;
            case ModePhoto.Roll:
                
                break;
            case ModePhoto.Saturation:
                
                break;
            case ModePhoto.Vignette:
                
                break;
            case ModePhoto.Bloom:
                slider.value = postProcessManager.Bloom.intensity.value;
                break;
            case ModePhoto.Contrasts:
                
                break;
            case ModePhoto.DOF:
                
                break;
            case ModePhoto.Exposure:
                
                break;
        }
    }

}
