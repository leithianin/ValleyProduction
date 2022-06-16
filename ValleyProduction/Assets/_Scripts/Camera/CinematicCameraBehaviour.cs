using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CinematicCameraBehaviour : MonoBehaviour
{
    [SerializeField] private SphericalTransform cameraTransform = default;
    [SerializeField] private Collider cameraBoundaries = default;
    [SerializeField] private CinematicShotDataBase database = default;

    [SerializeField] private Vector2 timeRange = default;

    [SerializeField] private Vector2 radiusRange = default;
    [SerializeField] private Vector2 polarAngleRange = default;
    [SerializeField] private float rotationSpeed;

    public bool cinematicModeTriggered = false;
    public bool inCinematicMode = false;

    [Header("Fade Values")]
    [SerializeField] private AnimationCurve fadeCurve = new AnimationCurve();
    [SerializeField] private float fadeDuration = 1f;
    private float textureAlpha = 0.0f;
    private Texture2D fadeTexture;
    private bool fadeDone;
    private float fadeTime;

    public UnityEvent OnPlayCinematic;
    public UnityEvent OnEndCinematic;
    private void Update()
    {
        if (!inCinematicMode && cinematicModeTriggered)
        {
            if (Random.Range(-5, 5) > 0) //Play custom or random shot
            {
                Debug.Log("1");
                StartCoroutine(PlayShotWithRandomRotation());
            }
            else
            {
                Debug.Log("2");
                StartCoroutine(PlayShotWithCustomsParameters());
            }
        }
    }

    [Button]
    public void PlayCinemtic(ShotsSequence sequenceScriptableObject)
    {
        OnPlayCinematic?.Invoke();
        StartCoroutine(PlayCinematic(sequenceScriptableObject));
    }

    public IEnumerator PlayCinematic(ShotsSequence sequenceScriptableObject)
    {
        CameraData[] sequence = sequenceScriptableObject.sequence;
        float refVerticalOffest = cameraTransform.OriginVisualOffset;

        inCinematicMode = true;

        foreach (CameraData shot in sequence)
        {
            Debug.Log("Play Cinematic");
            // Set the shot duration
            float referenceTime = shot.isTraveling ?
                        Vector3.Distance(shot.cameraOriginPosition, shot.travelPosition) / shot.speed
                        : Random.Range(timeRange.x, timeRange.y);
            referenceTime = shot.useCustomDuration ? shot.duration : referenceTime;

            while (textureAlpha < 1.0f)
            {
                yield return null;
            }
            // Set the shot position and angle (+ offset)
            cameraTransform.OriginVisualOffset = shot.verticalOffset != 0.0f ? shot.verticalOffset : cameraTransform.OriginVisualOffset;
            SelectDestination(shot.cameraOriginPosition.x, shot.cameraOriginPosition.z);
            SetAngles(shot.radius, shot.azimuthalAngle, shot.polarAngle);
           
            // Run the shot
            for (float time = referenceTime; time > 0; time -= Time.unscaledDeltaTime)
            {
                if (shot.isTraveling)
                {
                    cameraTransform.SetOrigin(Vector3.Lerp(shot.travelPosition, shot.cameraOriginPosition, shot.speedOverTime.Evaluate(time / referenceTime)));

                    if (shot.isRotating)
                    {
                        cameraTransform.AzimuthalRotation(shot.clockwise ? 1.0f : -1.0f, shot.rotationSpeed);
                    }
                    yield return null;
                }
                else
                {
                    if (shot.isRotating)
                    {
                        cameraTransform.AzimuthalRotation(shot.clockwise ? 1.0f : -1.0f, shot.rotationSpeed);
                    }
                    yield return null;
                }
            }

            FadeReset();
        }
        cameraTransform.OriginVisualOffset = refVerticalOffest;
        CameraManager.OnEndCinematic?.Invoke();
        OnEndCinematic?.Invoke();
        cinematicModeTriggered = false;
        inCinematicMode = false;
    }

    public IEnumerator PlayShotWithCustomsParameters()
    {
        inCinematicMode = true;
        while(textureAlpha < 1.0f)
        {
            yield return null;
        }

        CameraData shotData = SelectShotData(database.shotsDataBase);

        // Set the shot duration
        float referenceTime = shotData.isTraveling ?
            Vector3.Distance(shotData.cameraOriginPosition, shotData.travelPosition) / shotData.speed
            : Random.Range(timeRange.x, timeRange.y);
        referenceTime = shotData.useCustomDuration ? shotData.duration : referenceTime;

        // Set the shot position and angle (+ offset)
        cameraTransform.OriginVisualOffset = shotData.verticalOffset != 0.0f ? shotData.verticalOffset : cameraTransform.OriginVisualOffset;
        SelectDestination(shotData.cameraOriginPosition.x, shotData.cameraOriginPosition.z);
        SetAngles(shotData.radius, shotData.azimuthalAngle, shotData.polarAngle);

        // Run the shot
        for (float time = referenceTime; time > 0; time -= Time.deltaTime)
        {
            if (shotData.isTraveling)
            {
                cameraTransform.SetOrigin(Vector3.Lerp(shotData.travelPosition, shotData.cameraOriginPosition, time / referenceTime));
                yield return null;
            }
            else
            {
                yield return null;
            }
        }

        FadeReset();
        inCinematicMode = false;
    }

    public IEnumerator PlayShotWithCustomsParameters(CameraData cameraData)
    {
        float refVerticalOffest = cameraTransform.OriginVisualOffset;
        inCinematicMode = true;
        while (textureAlpha < 1.0f)
        {
            yield return null;
        }

        // Set the shot duration
        float referenceTime = cameraData.isTraveling ?
            Vector3.Distance(cameraData.cameraOriginPosition, cameraData.travelPosition) / cameraData.speed
            : Random.Range(timeRange.x, timeRange.y);
        referenceTime = cameraData.useCustomDuration ? cameraData.duration : referenceTime;

        // Set the shot position and angle (+ offset)
        cameraTransform.OriginVisualOffset = cameraData.verticalOffset != 0.0f ? cameraData.verticalOffset : cameraTransform.OriginVisualOffset;
        SelectDestination(cameraData.cameraOriginPosition.x, cameraData.cameraOriginPosition.z);
        SetAngles(cameraData.radius, cameraData.azimuthalAngle, cameraData.polarAngle);

        // Run the shot
        for (float time = referenceTime; time > 0; time -= Time.unscaledDeltaTime)
        {
            if (cameraData.isTraveling)
            {
                cameraTransform.SetOrigin(Vector3.Lerp(cameraData.travelPosition, cameraData.cameraOriginPosition, time / referenceTime));

                if (cameraData.isRotating)
                {
                    cameraTransform.AzimuthalRotation(cameraData.clockwise ? 1.0f : -1.0f, cameraData.rotationSpeed);
                }
                yield return null;
            }
            else
            {
                if (cameraData.isRotating)
                {
                    cameraTransform.AzimuthalRotation(cameraData.clockwise ? 1.0f : -1.0f, cameraData.rotationSpeed);
                }
                yield return null;
            }
        }

        FadeReset();
        cameraTransform.OriginVisualOffset = refVerticalOffest;
        inCinematicMode = false;
    }


    public IEnumerator PlayShotWithRandomRotation()
    {
        inCinematicMode = true;
        while (textureAlpha < 1.0f)
        {
            yield return null;
        }

        SelectDestination();
        SetAngles();

        int rotateDir = (int)Mathf.Sign(Random.value - 0.5f);
        float referenceTime = Random.Range(timeRange.x, timeRange.y);

        for (float time = referenceTime; time > 0; time -= Time.unscaledDeltaTime)
        {
            cameraTransform.AzimuthalRotation(rotateDir, rotationSpeed);
            yield return null;
        }

        FadeReset();
        inCinematicMode = false;
    }

    CameraData SelectShotData(List<CameraData> database)
    {
        // Check if cameraData is set for the current scene
        List<CameraData> tempDataBase = new List<CameraData>();
        foreach (var item in database)
        {
            if (item.scene == SceneManager.GetActiveScene().name)
            {
                if (!item.cinematic)
                    tempDataBase.Add(item);
            }
        }

        return tempDataBase[(int)Random.Range(0, tempDataBase.Count)];
    }

    void SelectDestination()
    {
        if (!cameraBoundaries)
        {
            Debug.LogError("No Boundaries Collider is set on the Cinematic Camera Behaviour");
            return;
        }

        float xPos = cameraBoundaries.bounds.center.x + Random.Range(-cameraBoundaries.bounds.extents.x, cameraBoundaries.bounds.extents.x);
        float zPos = cameraBoundaries.bounds.center.z + Random.Range(-cameraBoundaries.bounds.extents.z, cameraBoundaries.bounds.extents.z);
        Vector3 newPos = new Vector3(xPos, cameraTransform.GetOriginPosition().y, zPos);
        cameraTransform.SetOriginPosition(newPos);
    }
    void SelectDestination(float x, float z)
    {
        Vector3 newPos = new Vector3(x, cameraTransform.GetOriginPosition().y, z);
        cameraTransform.SetOriginPosition(newPos);
    }

    void SetAngles()
    {
        cameraTransform.SetRadius(Random.Range(radiusRange.x, radiusRange.y));
        cameraTransform.SetAzimuthalAngle(Random.Range(0, 360));
        cameraTransform.SetPolarAngle(Random.Range(polarAngleRange.x, polarAngleRange.y));
    }

    void SetAngles(float radius, float azimuthalAngle, float polarAngle)
    {
        cameraTransform.SetRadius(radius);
        cameraTransform.SetAzimuthalAngle(azimuthalAngle);
        cameraTransform.SetPolarAngle(polarAngle);
    }

    public void FadeReset()
    {
        fadeDone = false;
        textureAlpha = 0.0f;
        fadeTime = 0;
    }

    private void OnGUI()
    {
        Fade();
    }

    private void Fade()
    {
        if (fadeDone)
            return;

        if (!inCinematicMode)
            return;

        if (fadeTexture == null)
            fadeTexture = new Texture2D(1, 1);

        fadeTexture.SetPixel(0, 0, new Color(0, 0, 0, textureAlpha));
        fadeTexture.Apply();

        fadeTime += Time.unscaledDeltaTime;
        textureAlpha = fadeCurve.Evaluate(fadeTime / fadeDuration);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);

        if (textureAlpha <= 0)
            fadeDone = true;

    }
}
