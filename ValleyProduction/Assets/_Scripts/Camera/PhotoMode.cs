using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public Camera playerCamera = default;
    public Transform playerCameraTransform = default;
    public SphericalTransform sphericalTransform = default;
    [SerializeField] private PostProcessManager postProcessManager = default;
    [SerializeField] private GameObject ui = default;
    [SerializeField] private ScreenshotsManager screenshotsManager;

    [SerializeField, ReadOnly] private bool active = false;

    [Header("Resolution")]
    [SerializeField] private int pictureWidth = 1920;
    [SerializeField] private int pictureHeight = 1080;

    [Header("Parameters")]
    [SerializeField, Range(10, 300)] private float focalLength;
    private float baseFocalLength = 21f;

    [SerializeField, Range(0, 10)] private float verticalOffset = default;
    float baseVerticalOffset;
    [SerializeField, Range(-90, 90)] private float roll = default;

    [Header("Depth of Field"), Space(10)]
    [SerializeField] private bool useDepthOfField = default;
    [SerializeField, Range(0,100)] private float focusDistance = default;

    [Header("Post Effects"), Space(10)]
    [SerializeField, Range(-10, 10)] private float exposure = default;
    [SerializeField, Range(-100, 100)] private float contrasts = default;
    [SerializeField, Range(-100, 100)] private float saturation = default;
    [SerializeField, Range(0, 5)] public float bloom = default;
    [SerializeField, Range(0, 100)] private float grain = default;
    [SerializeField, Range(0, 100)] private float vignette = default;

    [Header("Filters"), Space(10)]
    [SerializeField] private VolumeProfilesEnum filtersSelection = default;
    [SerializeField, Range(0,100)] private float filterIntensity = 100.0f;
    [SerializeField] private Texture[] filters = default;

    [Header("Events"), Space(10)]
    [SerializeField] private UnityEvent enablePhotoMode = default;
    [SerializeField] private UnityEvent disablePhotoMode = default;

    public bool isModePhoto = false;

    private void Awake()
    {
        //screenshotsManager = GameObject.Find("ScreenshotManager").GetComponent<ScreenshotsManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        /*SetBloom(bloom);

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
        SetRolling(roll);*/
    }

    private void LateUpdate()
    {
    }

    public void ChangeState()
    {
        if(!isModePhoto)
        { 
            EnablePhotoMode();
            isModePhoto = true;
        }
        else
        {
            DisablePhotoMode();
            isModePhoto = false;
        }
    }

    [Button]
    public void EnablePhotoMode()
    {
        enablePhotoMode.Invoke();
        ui.SetActive(false);

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
        ui.SetActive(true);

        playerCamera.focalLength = baseFocalLength;
        sphericalTransform.OriginVisualOffset = baseVerticalOffset;
        SetRolling(0.0f);
        postProcessManager.Volume.profile = postProcessManager.DefaultProfile;
    }

    [Button]
    public void Shoot()
    {
        RenderTexture rt = new RenderTexture(pictureWidth, pictureHeight, 24);
        playerCamera.targetTexture = rt;
        playerCamera.Render();
        RenderTexture.active = rt;
        playerCamera.targetTexture = null;
        RenderTexture.active = null;
        //Debug.Log(ScreenshotsManager.instance.name);
        ScreenshotsManager.instance.screenshotList.Add(rt);
        //screenshotsManager.screenshotList.Add(rt);
    }
    #region Convert Gallery to PNG
    [Button]
    public void ConvertGalleryToPNG()
    {
        if (!screenshotsManager)
        {
            Debug.LogError("Cannot find ScreenshotManager");
            return;
        }
        List<RenderTexture> screenshotList = ScreenshotsManager.instance.screenshotList;

        if (!System.IO.Directory.Exists(string.Format("{0}/Screenshots", Application.persistentDataPath)))
            System.IO.Directory.CreateDirectory(string.Format("{0}/Screenshots", Application.persistentDataPath));

        foreach (RenderTexture rTex in screenshotList)
        {
            Texture2D tempTex = ToTexture2D(rTex);
            ToPNG(tempTex, ScreenshotName(screenshotList.IndexOf(rTex)));
        }

        ShowExplorer(string.Format("{0}/Screenshots", Application.persistentDataPath));
    }

    private Texture2D ToTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }

    private void ToPNG(Texture2D tex, string pathDest)
    {
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(pathDest, bytes);
    }

    private string ScreenshotName(int texIndex)
    {
        return string.Format("{0}/Screenshots/screen_{1}.png",
            Application.persistentDataPath,
            texIndex.ToString());
    }
    private void ShowExplorer(string path)
    {
        path = path.Replace(@"/", @"\");
        System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
    }
    [Button]
    private void ShowExplorer()
    {
        string path = string.Format("{0}/Screenshots/", Application.persistentDataPath);
        path = path.Replace(@"/", @"\");
        System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
    }
    #endregion

    #region FocalLength
    private void SetFocalLength()
    {
        playerCamera.focalLength = Mathf.Lerp(playerCamera.focalLength, focalLength, 0.9f);
    }

    public void SetFocalLength(float focalLength)
    {
        playerCamera.focalLength = Mathf.Lerp(playerCamera.focalLength, focalLength, 0.9f);
    }
    #endregion

    #region Rolling
    public void SetRolling(float value)
    {
        //transform.rotation = Quaternion.FromToRotation()
        playerCameraTransform.eulerAngles = new Vector3(playerCameraTransform.eulerAngles.x, playerCameraTransform.eulerAngles.y, value);
    }
    #endregion

    #region VerticalOffset
    public void SetVerticalOffset(float value)
    {
        sphericalTransform.OriginVisualOffset = value * 10.0f;
    }
    #endregion

    #region Bloom
    private void EnableBloom(bool enable)
    {
        postProcessManager.Bloom.active = enable;
    }

    public void SetBloom(float value)
    {
        Debug.Log(value);
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
        postProcessManager.Vignette.intensity.Override(value);
    }
    #endregion

    #region WhiteBalance
    private void EnableWhiteBalance(bool enable)
    {
        postProcessManager.WhiteBalance.active = enable;
    }
    #endregion
}
