using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [Header("Info")]
    public string characterName;
        
    [Header("Body")]
    public Sprite bodySprite;
    public EmotionSprite[] emotionBodySprites;
    
    [Header("Audio")]
    [ShowInInspector]
    [SerializeField]
    private List<EmotionAudio> emotionAudioList;

    private Dictionary<Emotion, List<AudioClip>> _emotionAudioClips;

    [ShowInInspector]
    public Dictionary<Emotion, List<AudioClip>> EmotionAudioClips
    {
        get
        {
            if (_emotionAudioClips == null)
            {
                _emotionAudioClips = emotionAudioList.ToDictionary(e => e.emotion, e => e.audioClips);
            }
            return _emotionAudioClips;
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

