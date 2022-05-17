using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private float timeMultiplier;
    [SerializeField] private float startHour;
    [SerializeField] private TextMeshProUGUI timeText;

    [SerializeField] private Light sunLight;
    [SerializeField] private float sunriseHour;
    [SerializeField] private float sunsetHour;

    //[SerializeField] private Color dayAmbientLight;
    //[SerializeField] private Color nightAmbientLight;
    [SerializeField] private Gradient hourGrandient;
    [SerializeField] private AnimationCurve lightChangeCurve;
    [SerializeField] private AnimationCurve sunRotationY;
    [SerializeField] private float maxSunLightIntensity;


    private DateTime currentTime;
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    private int dayNb = 1;

    private void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }

    private void Update()
    {
        UpdateTimeOfDay();
        RotateOrbits();
        UpdateLightSettings();
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

        if(timeText != null)
        {
            timeText.text = $"{currentTime.ToString("HH:mm")}, day {dayNb}";
        }
    }
    private void RotateOrbits()
    {
        float sunLigthRotationX;
        float sunLightRotationY;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);

            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLigthRotationX = Mathf.Lerp(0, 180, (float)percentage);
            sunLightRotationY = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);

            TimeSpan timeSinceSunset = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunset.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;

            sunLigthRotationX = Mathf.Lerp(-360, -180, (float)percentage);
            sunLightRotationY = Mathf.Lerp(0, 180, (float)percentage);
        }

        sunLight.transform.rotation = Quaternion.Euler(sunLigthRotationX, sunLightRotationY, 0);
        //sunLight.transform.rotation = Quaternion.AngleAxis(sunLigthRotation, Vector3.right);

    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        //sunLight.intensity = Mathf.Lerp(0, maxSunLightIntensity, lightChangeCurve.Evaluate(dotProduct));
        //moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, lightChangeCurve.Evaluate(dotProduct));
        sunLight.color = hourGrandient.Evaluate(((float)currentTime.Hour + (float)currentTime.Minute * 1f/60f) / 24f);
        Debug.Log(sunLight.color);
        Debug.Log(((float)currentTime.Hour + (float)currentTime.Minute * 1f/60f) / 24f);
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if(difference.TotalSeconds < 0)
        {
            difference += TimeSpan.FromHours(24);
        }

        return difference;
    }
}
