using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EmotionSpriteDataSO", menuName = "Scriptable Objects/Emotions and Sprites Data")]
public class EmotionSpriteDataSO : ScriptableObject
{
    public EmotionSprite[] emotionSprites;

    public Sprite GetSpriteFromEmotion(Emotion emotion)
    {
        return emotionSprites.FirstOrDefault(b => b.emotion == emotion).sprite;
    }
}
