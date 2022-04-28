using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessManager : MonoBehaviour
{
    [SerializeField] private SphericalTransform cameraSphericalTransform = default;

    [Header("PostProcess Referecences")]
    [SerializeField] private Volume volume = default;
    VolumeProfile profile;

    [Header("Depth of Field Values")]
    [SerializeField, Tooltip("Only apply on Awake")] private bool _useDepthOfField;
    [SerializeField] private float activationDOFDistance;
    [SerializeField] private float standartDOF;
    [SerializeField] private float closeViewDOF;

    Bloom bloom;
    public Bloom Bloom { get; private set; }

    ChannelMixer channelMixer;
    public ChannelMixer ChannelMixer { get; private set; }

    ChromaticAberration chromaticAbberration;
    public ChromaticAberration ChromaticAbberration { get; private set; }
    
    ColorAdjustments colorAdjustments;
    public ColorAdjustments ColorAdjustments { get; private set; }

    ColorCurves colorCurves;
    public ColorCurves ColorCurves { get; private set; }

    ColorLookup colorLookup;
    public ColorLookup ColorLookup { get; private set; }

    DepthOfField depthOfField;
    public DepthOfField DepthOfField { get; private set; }

    FilmGrain filmGrain;
    public FilmGrain FilmGrain { get; private set; }

    LensDistortion lensDistortion;
    public LensDistortion LensDistortion { get; private set; }

    LiftGammaGain liftGammaGain;
    public LiftGammaGain LiftGammaGain { get; private set; }

    MotionBlur motionBlur;
    public MotionBlur MotionBlur { get; private set; }

    PaniniProjection paniniProjection;
    public PaniniProjection PaniniProjection { get; private set; }

    ShadowsMidtonesHighlights shadowsMidtonesHighlights;
    public ShadowsMidtonesHighlights ShadowsMidtonesHighlights { get; private set; }

    SplitToning splitToning;
    public SplitToning SplitToning { get; private set; }

    Tonemapping tonemapping;
    public Tonemapping Tonemapping { get; private set; }

    Vignette vignette;
    public Vignette Vignette { get; private set; }

    WhiteBalance whiteBalance;
    public WhiteBalance WhiteBalance { get; private set; }

    private bool activeDepthOfField;


    private void Awake()
    {
        GetPostProcessDOF();
        SetDOFState();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDOF();
    }


    public bool CheckIfProfileIsSet()
    {
        return volume;
    }

    public void SetProfile(VolumeProfile profile)
    {
        volume.profile = profile;
    }

    public void GetProfileOverrides()
    {
        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<ChannelMixer>(out channelMixer);
        volume.profile.TryGet<ChromaticAberration>(out chromaticAbberration);
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        volume.profile.TryGet<ColorCurves>(out colorCurves);
        volume.profile.TryGet<ColorLookup>(out colorLookup);
        volume.profile.TryGet<DepthOfField>(out depthOfField);
        volume.profile.TryGet<FilmGrain>(out filmGrain);
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
        volume.profile.TryGet<LiftGammaGain>(out liftGammaGain);
        volume.profile.TryGet<MotionBlur>(out motionBlur);
        volume.profile.TryGet<PaniniProjection>(out paniniProjection);
        volume.profile.TryGet<ShadowsMidtonesHighlights>(out shadowsMidtonesHighlights);
        volume.profile.TryGet<SplitToning>(out splitToning);
        volume.profile.TryGet<Tonemapping>(out tonemapping);
        volume.profile.TryGet<Vignette>(out vignette);
        volume.profile.TryGet<WhiteBalance>(out whiteBalance);

    }

    void GetPostProcessDOF()
    {
        if (!volume)
        {
            Debug.LogWarning("Cannot find sharedProfile");
            return;
        }

        if (!volume.sharedProfile)
        {
            Debug.LogWarning("Cannot find sharedProfile");
            return;
        }

        volume.profile.TryGet<DepthOfField>(out depthOfField);
        standartDOF = depthOfField.gaussianEnd.value;
        
    }

    void SetDOFState()
    {
        depthOfField.active = _useDepthOfField;
    }

    void ChangeDOF()
    {
        if (!depthOfField.active)
            return;

        if (cameraSphericalTransform.GetTargetDistanceToOrigin() < activationDOFDistance)
        {
            depthOfField.gaussianEnd.value = closeViewDOF;
        }
        else
        {
            depthOfField.gaussianEnd.value = standartDOF;
        }
    }
}
