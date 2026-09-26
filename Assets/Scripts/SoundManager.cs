using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using JetBrains.Annotations;

public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance { get; private set; }

   [SerializeField] private AudioSource musicAudioSource;
   [SerializeField] private AudioSource SFXAudioSource;
   [SerializeField] List<AudioClip> music;

   private int currentIndex = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicAudioSource = GetComponent<AudioSource>();
    }

    public void StartMusic(int index)
    {
        if (currentIndex == index)
            return;

        currentIndex = index;
        musicAudioSource.Stop();
        if (index < music.Count)
        {
            musicAudioSource.clip = music[index];
            musicAudioSource.Play();
        }
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void PlaySFC(AudioClip audioClip)
    {
        SFXAudioSource.PlayOneShot(audioClip);
    }

}