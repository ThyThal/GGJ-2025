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
    
    public BubbleUI GetTargetBubble(bool isPlayer)
    {
        BubbleUI targetBubble = isPlayer ? playerBubble : otherCharacterBubble;
        return targetBubble;
    }
    
    public IEnumerator AnimateBubble(BubbleUI target, Emotion emotion)
    {
        // Don't animate if sprite already set
        if (target.GetEmotion() == emotion) yield break;

        if (target.IsShowing)
        {
            target.Hide();
            yield return new WaitForSeconds(target.HideTime);
            //waitTime += targetBubble.HideTime;
        }
        target.SetEmotion(emotion);
        target.Show();
        yield return new WaitForSeconds(target.ShowTime);
    }

    public void HideAllBubbles()
    {
        playerBubble.TurnOff();
        otherCharacterBubble.TurnOff();
    }
}