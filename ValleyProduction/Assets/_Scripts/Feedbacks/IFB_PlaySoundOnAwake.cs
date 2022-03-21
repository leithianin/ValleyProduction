using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IFB_PlaySoundOnAwake : MonoBehaviour, IFeedbackPlayer
{
    [SerializeField] private AudioPlayer audioPlayer;

    private void Start()
    {
        Play();
    }

    public void Play()
    {
        audioPlayer.Play();
    }
}
