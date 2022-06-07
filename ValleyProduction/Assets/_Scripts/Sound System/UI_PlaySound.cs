using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PlaySound : MonoBehaviour
{
    [SerializeField] private AudioSound toPlay;

    public void Play()
    {
        ValleyAudioManager.PlayTemporaryGlobalSound(toPlay);
    }
}
