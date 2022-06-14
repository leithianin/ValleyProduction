using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValleyAudioManager : VLY_Singleton<ValleyAudioManager>
{
    //Mettre à jour les MixerGroup avec les valeurs de base ?

    [SerializeField] private AudioSound music;
    [SerializeField] private AudioPlayer musicPlayer;

    [SerializeField] private AudioPlayer audioPlayerPrefab;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (musicPlayer != null)
        {
            musicPlayer.Play(music);
        }
    }

    public static void PlayTemporaryGlobalSound(AudioSound toPlay)
    {
        AudioPlayer newPlayer = Instantiate(instance.audioPlayerPrefab, instance.transform);

        newPlayer.Play(toPlay, toPlay.Mixer);

        TimerManager.CreateRealTimer(newPlayer.CurrentAudio.length, () => Destroy(newPlayer.gameObject));
    }

    public static void SetMainMusic(AudioSound newMusic)
    {
        instance.OnSetMainMusic(newMusic);
    }

    private void OnSetMainMusic(AudioSound newMusic)
    {
        Debug.Log("Play Music");

        music = newMusic;
        musicPlayer.Play(music);
    }
}
