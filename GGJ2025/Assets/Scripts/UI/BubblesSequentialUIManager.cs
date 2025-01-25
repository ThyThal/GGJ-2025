using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;

public class BubblesSequentialUIManager : MonoBehaviour
{
    [SerializeField] private BubbleUI[] playerBubbles;
    [SerializeField] private BubbleUI[] otherCharacterBubbles;

    [SerializeField] private EmotionBubbleDataSO bubbleSprites;
    
    public TextMeshProUGUI GetBubbleTarget(bool isPlayer, int index, Emotion emotion)
    {
        if (index > playerBubbles.Length - 1)
        {
            Debug.LogError("Get Bubble index out of range");
            return null;
        }
        
        BubbleUI targetBubble = isPlayer ? playerBubbles[index] : otherCharacterBubbles[index];
        targetBubble.Show();
        targetBubble.SetSprite(bubbleSprites.GetSpriteFromEmotion(emotion));
        
        return targetBubble.TextComponent;
    }

    public void HideAllBubbles()
    {
        for (int i = 0; i < playerBubbles.Length; i++)
        {
            playerBubbles[i].Hide();
        }

        for (int i = 0; i < otherCharacterBubbles.Length; i++)
        {
            otherCharacterBubbles[i].Hide();
        }
    }
}
