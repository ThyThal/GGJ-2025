using System;
<<<<<<< Updated upstream
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> Stashed changes
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EmotionSpriteDataSO", menuName = "Scriptable Objects/Emotions and Sprites Data")]
public class EmotionSpriteDataSO : ScriptableObject
{
    public EmotionSprite[] emotionSprites;

    public Sprite GetSpriteFromEmotion(Emotion emotion)
    {
        return emotionSprites.FirstOrDefault(b => b.emotion == emotion).sprite;
    }
}

[Serializable]
public struct EmotionSprite
{
    public Emotion emotion;
    public Sprite sprite;
}
