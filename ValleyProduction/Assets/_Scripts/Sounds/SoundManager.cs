using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance;

    [Header("Sounds")]
    [Space(10)]
    [Header("Ambiant")]
    [SerializeField] private AudioSource musicTheme = default;
    [SerializeField] private AudioSource forestAmbiant = default;
    private List<AudioSource> birdsSoundsist = new List<AudioSource>();
    [SerializeField] private AudioSource birdAmbiant01 = default;
    [SerializeField] private AudioSource birdAmbiant02 = default;
    [SerializeField] private AudioSource birdAmbiant03 = default;
    [Header("UI")]
    [SerializeField] private AudioSource click01 = default;
    [SerializeField] private AudioSource pop01 = default;
    [SerializeField] private AudioSource chime01 = default;
    [SerializeField] private AudioSource baliseConstructed = default;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }

        birdsSoundsist.Add(birdAmbiant01);
        birdsSoundsist.Add(birdAmbiant02);
        birdsSoundsist.Add(birdAmbiant03);
    }

    public void  PlayMusicTheme()
    {
        musicTheme.Play();
    }

    public void PlayForestAmbiant()
    {
        forestAmbiant.Play();
    }

    public void PlayBirdSound()
    {
        birdsSoundsist[Random.Range(0, 2)].Play();
    }

    public void PlayClick()
    {
        click01.Play();
    }

    public void PlayPop()
    {
        pop01.Play();
    }

    public void PlayChime()
    {
        chime01.Play();
    }

    public void PlayBaliseConstructed()
    {
        baliseConstructed.Play();
    }
}
