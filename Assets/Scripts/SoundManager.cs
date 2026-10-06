using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using JetBrains.Annotations;   // Added automatically by the IDE at some point. Not used.

// SoundManager
// Plays background music and one-off sound effects.
// Lives in the MainMenu scene (on the MusicManager object) and survives
// every scene change, so the music doesn't restart each time a scene loads.
// Same Singleton pattern as GameManager: reach it with SoundManager.Instance.
public class SoundManager : MonoBehaviour
{
   public static SoundManager Instance { get; private set; }

   [SerializeField] private AudioSource musicAudioSource;  // Loops the background music
   [SerializeField] private AudioSource SFXAudioSource;    // Plays short effects on top of the music
   [SerializeField] List<AudioClip> music;                 // All songs. Scripts pick one by its number in this list.

   private int currentIndex = -1;  // Song playing now. -1 means nothing yet.

    void Awake()
    {
        if (Instance == null)  // First SoundManager: keep it
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else  // Returning to MainMenu creates a second one: remove it
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Replaces the Inspector value with the first AudioSource on this
        // object. On MusicManager that happens to be the music source.
        musicAudioSource = GetComponent<AudioSource>();
    }

    // Switch to a song by its number in the music list
    public void StartMusic(int index)
    {
        if (currentIndex == index)  // Already playing this song, so don't restart it
            return;

        currentIndex = index;
        musicAudioSource.Stop();
        if (index < music.Count)  // Only play if the number is a real slot in the list
        {
            musicAudioSource.clip = music[index];
            musicAudioSource.Play();
        }
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    // Plays a sound effect. (SFC is a typo for SFX, kept as-is.)
    // PlayOneShot lets several effects overlap on one AudioSource.
    public void PlaySFC(AudioClip audioClip)
    {
        SFXAudioSource.PlayOneShot(audioClip,0.7f);
    }

}