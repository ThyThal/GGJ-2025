using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EmotionBubbleDataSO", menuName = "Scriptable Objects/Emotion Bubble Sprite Data")]
public class EmotionBubbleDataSO : ScriptableObject
{
    public BubbleEmotionSprite[] bubbleSprites;

    public Sprite GetSpriteFromEmotion(Emotion emotion)
    {
        return bubbleSprites.FirstOrDefault(b => b.emotion == emotion).sprite;
    }
}
