using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
    [Header("Info")]
    public string characterName;
        
    [Header("Body")]
    public Sprite bodySprite;
    
    [Header("Face")]
    public Sprite normalFaceSprite;
    public Sprite angryFaceSprite;
    public Sprite thinkingFaceSprite;
    
    [Header("Audio")]
    public AudioClip normalVoice;
    public AudioClip angryVoice;
    public AudioClip thinkingVoice;

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
    
    public AudioClip GetVoice(Emotion type)
    {
        return type switch
        {
            Emotion.Normal => normalVoice,
            Emotion.Scream => angryVoice,
            Emotion.Thinking => thinkingVoice,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
