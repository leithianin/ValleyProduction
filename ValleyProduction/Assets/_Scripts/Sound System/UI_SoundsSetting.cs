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

    [SerializeField] private SoundDatas datas;

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
        //Master
        master.value.text = Mathf.RoundToInt(datas.masterVolume*100).ToString();
        master.slider.value = datas.masterVolume;

        //Music
        music.value.text = Mathf.RoundToInt(datas.musicVolume * 100).ToString();
        music.slider.value = datas.musicVolume;

        //SFX
        sfx.value.text = Mathf.RoundToInt(datas.sfxVolume * 100).ToString();
        sfx.slider.value = datas.sfxVolume;

        //UI
        ui.value.text = Mathf.RoundToInt(datas.uiVolume * 100).ToString();
        ui.slider.value = datas.uiVolume;

        //Ambient
        ambient.value.text = Mathf.RoundToInt(datas.ambientVolume * 100).ToString();
        ambient.slider.value = datas.ambientVolume;
    }

    public void SetMasterVolume(float sliderValue)
    {
        datas.masterVolume = sliderValue;
        masterMixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        master.value.text = Mathf.RoundToInt(datas.masterVolume * 100).ToString();
    }

    public void SetMusicVolume(float sliderValue)
    {
        datas.musicVolume = sliderValue;
        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
        music.value.text = Mathf.RoundToInt(datas.musicVolume * 100).ToString();
    }

    public void SetSFXVolume(float sliderValue)
    {
        datas.sfxVolume = sliderValue;
        sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sliderValue) * 20);
        sfx.value.text = Mathf.RoundToInt(datas.sfxVolume * 100).ToString();
    }

    public void SetUIVolume(float sliderValue)
    {
        datas.uiVolume = sliderValue;
        uiMixer.audioMixer.SetFloat("UIVolume", Mathf.Log10(sliderValue) * 20);
        ui.value.text = Mathf.RoundToInt(datas.uiVolume * 100).ToString();
    }

    public void SetAmbientVolume(float sliderValue)
    {
        datas.ambientVolume = sliderValue;
        ambientMixer.audioMixer.SetFloat("AmbientVolume", Mathf.Log10(sliderValue) * 20);
        ambient.value.text = Mathf.RoundToInt(datas.ambientVolume * 100).ToString();
    }
}
