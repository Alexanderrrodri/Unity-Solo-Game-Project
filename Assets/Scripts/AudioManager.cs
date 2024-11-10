using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private AudioSource audioSource;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("AudioManager").AddComponent<AudioManager>();
                DontDestroyOnLoad(instance.gameObject); // Prevent destruction across scenes
            }
            return instance;
        }
    }

    private void Awake()
    {
        // Ensure there's only one instance of the AudioManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); // Destroy if duplicate AudioManager
        }
    }

    public void PlaySound(AudioClip clip, bool loop = false)
    {
        if (audioSource != null)
        {
            audioSource.Stop();        // Stop any currently playing sound
            audioSource.clip = clip;   // Set the new clip
            audioSource.loop = loop;   // Set looping
            audioSource.Play();        // Play the sound
        }
    }

    public void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}
