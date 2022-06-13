using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class UI_SoundsSetting : MonoBehaviour
{
    [Header("AudioMixer")]
    public AudioMixerGroup masterMixer;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;
    public AudioMixerGroup uiMixer;
    public AudioMixerGroup ambientMixer;

    [Header("UI")]
    public UI_AudioSettings master;
    public UI_AudioSettings music;
    public UI_AudioSettings sfx;
    public UI_AudioSettings ui;
    public UI_AudioSettings ambient;

    private void OnEnable()
    {
        SetBaseValue();
    }

    public void SetBaseValue()
    {
        float value;

        //Master
        masterMixer.audioMixer.GetFloat("MasterVolume", out value);
        master.value.text = (Mathf.Pow(value/20, 10)).ToString();
        //master.slider.value = Mathf.Log10(((int)value)) / 20;

        //Music
        musicMixer.audioMixer.GetFloat("MusicVolume", out value);
        music.value.text = (Mathf.Log(value) * 20).ToString();
        //music.slider.value = Mathf.Log10(((int)value)) / 20;

        //SFX
        sfxMixer.audioMixer.GetFloat("SFXVolume", out value);
        sfx.value.text = (Mathf.Log(value) * 20).ToString();
        //sfx.slider.value = Mathf.Log10(((int)value)) / 20;

        //UI
        uiMixer.audioMixer.GetFloat("UIVolume", out value);
        ui.value.text = (Mathf.Log(value) * 20).ToString();
        //ui.slider.value = Mathf.Log10(((int)value)) / 20;

        //Ambient
        ambientMixer.audioMixer.GetFloat("AmbientVolume", out value);
        ambient.value.text = (Mathf.Log(value) * 20).ToString();
        //ambient.slider.value = Mathf.Log10(((int)value)) / 20;
    }

    public void SetMasterVolume(float sliderValue)
    {
        masterMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        master.value.text = ((int)(sliderValue * 100)).ToString();
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        music.value.text = ((int)(sliderValue * 100)).ToString();
    }

    public void SetSFXVolume(float sliderValue)
    {
        sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        sfx.value.text = ((int)(sliderValue * 100)).ToString();
    }

    public void SetUIVolume(float sliderValue)
    {
        uiMixer.audioMixer.SetFloat("UIVolume", Mathf.Log10(sliderValue) * 20);
        ui.value.text = ((int)(sliderValue * 100)).ToString();
    }

    public void SetAmbientVolume(float sliderValue)
    {
        ambientMixer.audioMixer.SetFloat("AmbientVolume", Mathf.Log10(sliderValue) * 20);
        ambient.value.text = ((int)(sliderValue * 100)).ToString();
    }
}
