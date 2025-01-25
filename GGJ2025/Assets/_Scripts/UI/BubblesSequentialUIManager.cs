using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BubblesSequentialUIManager : MonoBehaviour
{
    [SerializeField] private BubbleUI[] playerBubbles;
    [SerializeField] private BubbleUI[] otherCharacterBubbles;

    [FormerlySerializedAs("bubbleSprites")] [SerializeField] private EmotionSpriteDataSO spriteSprites;
    
    public TextMeshProUGUI GetBubbleTarget(bool isPlayer, int index, Emotion emotion)
    {
        if (index > playerBubbles.Length - 1)
        {
            Debug.LogError("Get Bubble index out of range");
            return null;
        }
        
        BubbleUI targetBubble = isPlayer ? playerBubbles[index] : otherCharacterBubbles[index];
        targetBubble.Show();

        
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
