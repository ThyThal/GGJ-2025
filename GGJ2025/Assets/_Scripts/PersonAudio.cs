using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PersonAudio : MonoBehaviour
{
    [SerializeField] private CharacterDataSO characterData;
    [SerializeField] private List<AudioComponents> emotionAudio = new();
    
    [SerializeField] private AudioClip lastPlayedClip;

    private void Start()
    {
        if (characterData == null)
        {
            Debug.LogWarning("Character data is not set.");
            return;
        }

        InitializeAudioComponents();
    }

    private void InitializeAudioComponents()
    {
        // If `emotionAudio` is empty, create and populate a default AudioComponent
        if (emotionAudio.Count == 0)
        {
            if (characterData.EmotionAudioClips != null && characterData.EmotionAudioClips.Count > 0)
            {
                AddDefaultAudioComponent();
            }
            else
            {
                Debug.LogWarning("Character data has no emotion audio clips.");
            }

            return;
        }

        // Ensure all existing `emotionAudio` components are properly initialized
        foreach (var audioComponent in emotionAudio)
        {
            if (audioComponent.audioSource == null)
            {
                audioComponent.audioSource = gameObject.AddComponent<AudioSource>();
            }

            if (audioComponent.emotionAudioClips == null || audioComponent.emotionAudioClips.Count == 0)
            {
                PopulateEmotionAudioClips(audioComponent);
            }
        }
    }

    private void AddDefaultAudioComponent()
    {
        var newAudioComponent = new AudioComponents
        {
            audioSource = gameObject.AddComponent<AudioSource>(),
            emotionAudioClips = new List<EmotionAudioClip>()
        };
        
        newAudioComponent.audioSource.playOnAwake = false;

        foreach (var kvp in characterData.EmotionAudioClips)
        {
            newAudioComponent.emotionAudioClips.Add(new EmotionAudioClip
            {
                emotion = kvp.Key,
                audioClips = new List<AudioClip>(kvp.Value)
            });
        }

        emotionAudio.Add(newAudioComponent);
    }

    private void PopulateEmotionAudioClips(AudioComponents audioComponent)
    {
        audioComponent.emotionAudioClips ??= new List<EmotionAudioClip>();

        foreach (var kvp in characterData.EmotionAudioClips)
        {
            if (!audioComponent.emotionAudioClips.Exists(e => e.emotion == kvp.Key))
            {
                audioComponent.emotionAudioClips.Add(new EmotionAudioClip
                {
                    emotion = kvp.Key,
                    audioClips = new List<AudioClip>(kvp.Value)
                });
            }
        }
    }

    public void SetCharacterData(CharacterDataSO data)
    {
        characterData = data;
        InitializeAudioComponents();
    }
    
    public void PlayEmotionAudio(Emotion emotion)
    {
        foreach (var audioComponent in emotionAudio)
        {
            var emotionAudioClip = audioComponent.emotionAudioClips.Find(e => e.emotion == emotion);

            if (emotionAudioClip != null && emotionAudioClip.audioClips.Count > 0)
            {
                var clip = GetNextAudioClip(emotionAudioClip.audioClips);

                if (clip != null)
                {
                    audioComponent.audioSource.clip = clip;
                    audioComponent.audioSource.Play();
                    lastPlayedClip = clip;
                }
                else
                {
                    Debug.LogWarning($"No available audio clip to play for emotion: {emotion}");
                }

                return;
            }
        }

        Debug.LogWarning($"No audio clips found for emotion: {emotion}");
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

    [Serializable]
    private class AudioComponents
    {
        [SerializeField] public AudioSource audioSource;
        [ShowInInspector] public List<EmotionAudioClip> emotionAudioClips;
    }

    [Serializable]
    private class EmotionAudioClip
    {
        public Emotion emotion;
        public List<AudioClip> audioClips;
    }
}
