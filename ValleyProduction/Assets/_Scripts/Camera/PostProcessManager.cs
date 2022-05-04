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
    public Volume Volume { get => volume; }
    [SerializeField] private VolumeProfile defaultProfile = default;
    public VolumeProfile DefaultProfile { get => defaultProfile; private set => defaultProfile = value; }
    [SerializeField] private VolumeProfile photoProfile = default;
    public VolumeProfile PhotoProfile { get => photoProfile; private set => photoProfile = value; }

    VolumeProfile profile;


    Bloom bloom;
    public Bloom Bloom { get => bloom; private set => bloom = value; }

    ChannelMixer channelMixer;
    public ChannelMixer ChannelMixer { get => channelMixer; private set => channelMixer = value; }

    ChromaticAberration chromaticAbberration;
    public ChromaticAberration ChromaticAbberration { get => chromaticAbberration; private set => chromaticAbberration = value; }

    ColorAdjustments colorAdjustments;
    public ColorAdjustments ColorAdjustments { get => colorAdjustments; private set => colorAdjustments = value; }

    ColorCurves colorCurves;
    public ColorCurves ColorCurves { get => colorCurves; private set => colorCurves = value; }

    ColorLookup colorLookup;
    public ColorLookup ColorLookup { get => colorLookup; private set => colorLookup = value; }

    DepthOfField depthOfField;
    public DepthOfField DepthOfField { get => depthOfField; private set => depthOfField = value; }

    FilmGrain filmGrain;
    public FilmGrain FilmGrain { get => filmGrain; private set => filmGrain = value; }

    LensDistortion lensDistortion;
    public LensDistortion LensDistortion { get => lensDistortion; private set => lensDistortion = value; }

    LiftGammaGain liftGammaGain;
    public LiftGammaGain LiftGammaGain { get => liftGammaGain; private set => liftGammaGain = value; }

    MotionBlur motionBlur;
    public MotionBlur MotionBlur { get => motionBlur; private set => motionBlur = value; }

    PaniniProjection paniniProjection;
    public PaniniProjection PaniniProjection { get => paniniProjection; private set => paniniProjection = value; }

    ShadowsMidtonesHighlights shadowsMidtonesHighlights;
    public ShadowsMidtonesHighlights ShadowsMidtonesHighlights { get => shadowsMidtonesHighlights; private set => shadowsMidtonesHighlights = value; }

    SplitToning splitToning;
    public SplitToning SplitToning { get => splitToning; private set => splitToning = value; }

    Tonemapping tonemapping;
    public Tonemapping Tonemapping { get => tonemapping; private set => tonemapping = value; }

    Vignette vignette;
    public Vignette Vignette { get => vignette; private set => vignette = value; }

    WhiteBalance whiteBalance;
    public WhiteBalance WhiteBalance { get => whiteBalance; private set => whiteBalance = value; }

    private bool activeDepthOfField;


    private void Awake()
    {
        //GetProfileOverrides();
    }

    // Update is called once per frame
    void Update()
    {
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
    }
}
