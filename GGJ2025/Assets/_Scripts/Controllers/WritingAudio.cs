using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WritingAudio : MonoBehaviour
{
    [ShowInInspector] 
    [SerializeField] private List<EmotionAudioClip> emotionAudioClips = new(); // Use a list of EmotionAudioClip to make it serializable

    [SerializeField] private AudioSource audioSource;

    private AudioClip lastPlayedClip;

    private void Start()
    {
        // If the audioSource is still null, try to find it
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component is missing even though it is required.");
            }
        }

        // Ensure the AudioSource doesn't play on awake
        audioSource.playOnAwake = false;
    }

    public void SetEmotionAudioClips(List<EmotionAudioClip> newEmotionAudioClips)
    {
        emotionAudioClips = newEmotionAudioClips;
    }

    public void PlayEmotionAudio(Emotion emotion)
    {
        var emotionClip = emotionAudioClips.Find(e => e.emotion == emotion);

        if (emotionClip != null && emotionClip.audioClips.Count > 0)
        {
            var clip = GetNextAudioClip(emotionClip.audioClips);

            if (clip != null)
            {
                audioSource.clip = clip;
                audioSource.Play();
                lastPlayedClip = clip;
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
