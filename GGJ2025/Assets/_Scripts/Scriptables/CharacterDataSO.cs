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
    public Sprite bodyScared;
    public EmotionSprite[] emotionBodySprites;
    
    [Header("Face")]
    public Sprite normalFaceSprite;
    public Sprite angryFaceSprite;
    public Sprite thinkingFaceSprite;
    
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

    public Sprite GetFace(Emotion type)
    {
        return type switch
        {
            Emotion.Normal => normalFaceSprite,
            Emotion.Scream => angryFaceSprite,
            Emotion.Thinking => thinkingFaceSprite,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    [Serializable]
    public class EmotionAudio
    {
        public Emotion emotion;
        public List<AudioClip> audioClips;
    }
}

