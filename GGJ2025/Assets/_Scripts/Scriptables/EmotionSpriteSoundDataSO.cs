using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EmotionSpriteDataSO", menuName = "Scriptable Objects/Emotions and Sprites Data")]
public class EmotionSpriteSoundDataSO : ScriptableObject
{
    public EmotionSpriteSound[] emotionSprites;

    public Sprite GetSpriteFromEmotion(Emotion emotion)
    {
        return emotionSprites.FirstOrDefault(b => b.emotion == emotion).sprite;
    }
    
    public AudioClip GetSoundFromEmotion(Emotion emotion)
    {
        return emotionSprites.FirstOrDefault(b => b.emotion == emotion).sound;
    }
}

[Serializable]
public struct EmotionSpriteSound
{
    public Emotion emotion;
    public Sprite sprite;
    public AudioClip sound;
}
