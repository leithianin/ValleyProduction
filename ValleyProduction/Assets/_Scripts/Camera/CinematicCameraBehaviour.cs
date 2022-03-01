using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicCameraBehaviour : MonoBehaviour
{
    [SerializeField] private SphericalTransform cameraTransform = default;
    [SerializeField] private Collider cameraBoundaries = default;

    [SerializeField] private Vector2 timeRange = default;

    [SerializeField] private Vector2 radiusRange = default;
    [SerializeField] private Vector2 polarAngleRange = default;
    [SerializeField] private float rotationSpeed;

    public bool cinematicModeTriggered = false;
    public bool inCinematicMode = false;

    [Header("Fade Values")]
    [SerializeField] private AnimationCurve fadeCurve = new AnimationCurve();
    [SerializeField] private float fadeDuration = 1f;
    private float textureAlpha = 1;
    private Texture2D fadeTexture;
    private bool fadeDone;
    private float fadeTime;


    private void Update()
    {
        if (!inCinematicMode && cinematicModeTriggered)
            StartCoroutine(RotateAround());
    }

    public IEnumerator RotateAround()
    {
        inCinematicMode = true;
        yield return new WaitForSeconds(0.5f);

        SelectDestination();
        SelectAngles();

        int rotateDir = (int)Mathf.Sign(Random.value - 0.5f);
        float referenceTime = Random.Range(timeRange.x, timeRange.y);

        for (float time = referenceTime; time > 0; time -= Time.deltaTime)
        {
            cameraTransform.AzimuthalRotation(rotateDir, rotationSpeed);
            yield return null;
        }

        FadeReset();
        inCinematicMode = false;
    }

    void SelectDestination()
    {
        if (!cameraBoundaries)
        {
            Debug.LogError("No Boundaries Collider is set on the Cinematic Camera Behaviour");
        }

        float xPos = cameraBoundaries.bounds.center.x + Random.Range(-cameraBoundaries.bounds.extents.x, cameraBoundaries.bounds.extents.x);
        float zPos = cameraBoundaries.bounds.center.z + Random.Range(-cameraBoundaries.bounds.extents.z, cameraBoundaries.bounds.extents.z);
        Vector3 newPos = new Vector3(xPos, cameraTransform.GetOriginPosition().y, zPos);
        cameraTransform.SetOriginPosition(newPos);
    }

    void SelectAngles()
    {
        cameraTransform.SetRadius(Random.Range(radiusRange.x, radiusRange.y));
        cameraTransform.SetPolarAngle(Random.Range(polarAngleRange.x, polarAngleRange.y));
        cameraTransform.SetAzimuthalAngle(Random.Range(0, 360));
    }

    public void FadeReset()
    {
        fadeDone = false;
        textureAlpha = 1;
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

        fadeTime += Time.deltaTime;
        textureAlpha = fadeCurve.Evaluate(fadeTime / fadeDuration);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);

        if (textureAlpha <= 0)
            fadeDone = true;

    }
}
