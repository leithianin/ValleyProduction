using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyAudioManager : VLY_Singleton<ValleyAudioManager>
{
    [SerializeField] private AudioSound music;
    [SerializeField] private AudioPlayer musicPlayer;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        musicPlayer.Play(music);
    }

    public static void SetMainMusic(AudioSound newMusic)
    {
        instance.OnSetMainMusic(newMusic);
    }

    private void OnSetMainMusic(AudioSound newMusic)
    {
        music = newMusic;
        musicPlayer.Play(music);
    }
}
