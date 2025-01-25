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
    
    public BubbleUI GetTargetBubble(bool isPlayer)
    {
        BubbleUI targetBubble = isPlayer ? playerBubble : otherCharacterBubble;
        return targetBubble;
    }
    
    public IEnumerator AnimateBubble(BubbleUI target, Emotion emotion)
    {
        var newSprite = spriteSprites.GetSpriteFromEmotion(emotion);

        // Don't animate if sprite already set
        if (target.IsSpriteSet(newSprite) && target.IsShowing) yield break;

        if (target.IsShowing)
        {
            target.Hide();
            yield return new WaitForSeconds(target.HideTime);
            //waitTime += targetBubble.HideTime;
        }
        target.SetSprite(newSprite);
        target.Show();
        yield return new WaitForSeconds(target.ShowTime);
    }

    public void HideAllBubbles()
    {
        playerBubble.TurnOff();
        otherCharacterBubble.TurnOff();
    }
}

[Serializable]
public struct EmotionSprite
{
    public Emotion emotion;
    public Sprite sprite;
}