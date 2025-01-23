using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Scriptable Objects/Character Data")]
public class CharacterDataSO : ScriptableObject
{
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

    public Sprite GetFace(BubbleType type)
    {
        return type switch
        {
            BubbleType.Normal => normalFaceSprite,
            BubbleType.Scream => angryFaceSprite,
            BubbleType.Thinking => thinkingFaceSprite,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
    
    public AudioClip GetVoice(BubbleType type)
    {
        return type switch
        {
            BubbleType.Normal => normalVoice,
            BubbleType.Scream => angryVoice,
            BubbleType.Thinking => thinkingVoice,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}
