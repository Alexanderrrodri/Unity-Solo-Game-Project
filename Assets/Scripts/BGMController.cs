using System.Collections;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        // Get the Audio Source component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Start playing the music if not already playing
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    // Optional method to stop the music
    public void StopMusic()
    {
        audioSource.Stop();
    }

    // Optional method to fade out the music
    public void FadeOutMusic(float fadeDuration)
    {
        StartCoroutine(FadeOut(fadeDuration));
    }

    private IEnumerator FadeOut(float fadeDuration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // Reset volume for potential replays
    }
}
