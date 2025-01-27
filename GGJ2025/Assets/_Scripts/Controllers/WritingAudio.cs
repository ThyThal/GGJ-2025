using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WritingAudio : MonoBehaviour
{
    [ShowInInspector]
    [SerializeField] private List<EmotionAudioClip> emotionAudioClips = new(); // Use a list of EmotionAudioClip to make it serializable
    [SerializeField] private List<AudioSource> audioSources = new();

    private AudioSource mainAudioSource;
    private AudioClip lastPlayedClip;
    private int currentAudioSourceIndex = 0; // To track the current AudioSource index

    private void Start()
    {
        // If the mainAudioSource is still null, try to find it
        if (mainAudioSource == null)
        {
            mainAudioSource = GetComponent<AudioSource>();
            if (mainAudioSource == null)
            {
                Debug.LogError("Main AudioSource component is missing even though it is required.");
            }
        }

        // Ensure the main AudioSource doesn't play on awake
        mainAudioSource.playOnAwake = false;

        // Add additional audio sources if they don't exist yet
        if (audioSources.Count == 0)
        {
            for (int i = 0; i < 3; i++)  // Example: Create 3 additional sources
            {
                var newSource = gameObject.AddComponent<AudioSource>();
                newSource.playOnAwake = false;
                audioSources.Add(newSource);
            }
        }
    }

    public void SetEmotionAudioClips(List<EmotionAudioClip> newEmotionAudioClips)
    {
        emotionAudioClips = newEmotionAudioClips;
    }

    public void PlayEmotionAudio(Emotion emotion)
    {
        var emotionAudioClip = emotionAudioClips.Find(e => e.emotion == emotion);
        if (emotionAudioClip != null && emotionAudioClip.audioClips.Count > 0)
        {
            var clip = GetNextAudioClip(emotionAudioClip.audioClips);

            if (clip != null)
            {
                // Get the next AudioSource in a cyclic manner
                AudioSource availableSource = GetNextAudioSource();

                if (availableSource != null)
                {
                    availableSource.clip = clip;
                    availableSource.Play();
                    lastPlayedClip = clip;
                }
                else
                {
                    Debug.LogWarning("No available audio sources to play the new clip.");
                }
            }
            else
            {
                Debug.LogWarning($"No available audio clip to play for emotion: {emotion}");
            }
        }
        else
        {
            Debug.LogWarning($"No audio clips found for emotion: {emotion}");
        }
    }

    private AudioSource GetNextAudioSource()
    {
        // Cycle through the AudioSources by using the currentAudioSourceIndex and modulo operation
        AudioSource selectedSource = audioSources[currentAudioSourceIndex];
        currentAudioSourceIndex = (currentAudioSourceIndex + 1) % audioSources.Count; // Loop back to 0 when reaching the end

        return selectedSource;
    }

    private AudioClip GetNextAudioClip(List<AudioClip> audioClips)
    {
        if (audioClips.Count == 0) return null;

        // If there is only one clip, return it
        if (audioClips.Count == 1)
        {
            return audioClips[0];
        }

        // Remove the last played clip from the available pool
        var availableClips = new List<AudioClip>(audioClips);
        if (lastPlayedClip != null)
        {
            availableClips.Remove(lastPlayedClip);
        }

        // If all clips are removed, reset the pool
        if (availableClips.Count == 0)
        {
            availableClips = new List<AudioClip>(audioClips);
        }

        // Randomly select a clip from the available pool
        var clip = availableClips[UnityEngine.Random.Range(0, availableClips.Count)];
        return clip;
    }
}

[Serializable]
public class EmotionAudioClip
{
    public Emotion emotion;
    public List<AudioClip> audioClips = new();
}
