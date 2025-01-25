using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class PersonAudio : MonoBehaviour
{
    [SerializeField] private CharacterDataSO characterData;
    [SerializeField] private List<AudioComponents> emotionAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        if (characterData == null)
        {
            Debug.LogWarning("Character data is not set.");
            return;
        }

        if (emotionAudio.Count == 0)
        {
            // If the user did not manually add anything, create a single AudioSource if there are emotions in character data
            if (characterData.emotionAudioClips != null && characterData.emotionAudioClips.Count > 0)
            {
                var newAudioComponent = new AudioComponents
                {
                    audioSource = gameObject.AddComponent<AudioSource>(),
                    emotionAudioClips = new Dictionary<Emotion, List<AudioClip>>()
                };

                // Populate the dictionary with data from CharacterDataSO
                foreach (var kvp in characterData.emotionAudioClips)
                {
                    newAudioComponent.emotionAudioClips[kvp.Key] = new List<AudioClip>(kvp.Value);
                }

                emotionAudio.Add(newAudioComponent);
            }
            else
            {
                Debug.LogWarning("Character data has no emotion audio clips.");
                return;
            }
        }
        else
        {
            // If the user manually added components, ensure dictionaries are initialized and populated
            foreach (var audioComponent in emotionAudio)
            {
                if (audioComponent.audioSource == null)
                {
                    audioComponent.audioSource = gameObject.AddComponent<AudioSource>();
                }

                if (audioComponent.emotionAudioClips == null)
                {
                    audioComponent.emotionAudioClips = new Dictionary<Emotion, List<AudioClip>>();
                }

                foreach (var kvp in characterData.emotionAudioClips)
                {
                    if (!audioComponent.emotionAudioClips.ContainsKey(kvp.Key))
                    {
                        audioComponent.emotionAudioClips[kvp.Key] = new List<AudioClip>(kvp.Value);
                    }
                }
            }
        }
    }
    
    public void SetCharacterData(CharacterDataSO data)
    {
        characterData = data;
    }
    
    [Serializable] private class AudioComponents
    {
        [SerializeField] public AudioSource audioSource;
        [ShowInInspector] public Dictionary<Emotion, List<AudioClip>> emotionAudioClips;
    }
}
