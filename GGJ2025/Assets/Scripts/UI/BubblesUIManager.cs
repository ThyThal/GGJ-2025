using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BubblesUIManager : MonoBehaviour
{
    [SerializeField] private BubbleUI playerBubble;
    [SerializeField] private BubbleUI otherCharacterBubble;

    [FormerlySerializedAs("bubbleSprites")] [SerializeField] private EmotionSpriteDataSO spriteSprites;
    
    public TextMeshProUGUI GetBubbleTarget(bool isPlayer, int index, Emotion emotion)
    {        
        BubbleUI targetBubble = isPlayer ? playerBubble : otherCharacterBubble;
        targetBubble.Show();
        targetBubble.SetSprite(spriteSprites.GetSpriteFromEmotion(emotion));
        
        return targetBubble.TextComponent;
    }

    public void HideAllBubbles()
    {
        playerBubble.Hide();
        otherCharacterBubble.Hide();
    }
}

[Serializable]
public struct EmotionSprite
{
    public Emotion emotion;
    public Sprite sprite;
}