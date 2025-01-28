using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [Header("Info")]
    public string characterName;
        
    [Header("Body")]
    public Sprite bodySprite;
    public EmotionSpriteSound[] emotionBodySprites;
    
    [Header("Audio")]
    [ShowInInspector]
    [SerializeField]
    private List<EmotionAudio> emotionAudioList;

    private Dictionary<Emotion, List<AudioClip>> emotionAudioClips;

    [ShowInInspector]
    public Dictionary<Emotion, List<AudioClip>> EmotionAudioClips
    {
        get
        {
            if (emotionAudioClips == null)
            {
                emotionAudioClips = emotionAudioList.ToDictionary(e => e.emotion, e => e.audioClips);
            }
            return emotionAudioClips;
        }
    }

    public Sprite GetBody(Emotion type)
    { 
        var sprite = emotionBodySprites.FirstOrDefault(e => e.emotion == type).sprite;
        return sprite ? sprite : bodySprite;
    }

    [Serializable]
    public class EmotionAudio
    {
        public Emotion emotion;
        public List<AudioClip> audioClips;
    }
}

