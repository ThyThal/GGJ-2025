using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class BubblesUIManager : MonoBehaviour
{
    [SerializeField] private BubbleUI playerBubble;
    [SerializeField] private BubbleUI otherCharacterBubble;
    public BubbleUI GetTargetBubble(bool isPlayer)
    {
        return isPlayer ? playerBubble : otherCharacterBubble;
    }
    
    public IEnumerator AnimateBubble(BubbleUI target, Emotion emotion)
    {
        // Don't animate if sprite already set
        if (target.GetEmotion() == emotion) yield break;

        // Get the correct prefab for the emotion
        EmotionPrefab? emotionPrefab = target.emotionPrefabs.FirstOrDefault(e => e.emotion == emotion);

        if (emotionPrefab == null || emotionPrefab.Value.prefab == null)
        {
            Debug.LogWarning($"No prefab found for emotion: {emotion}");
            yield break;
        }

        var newPrefab = emotionPrefab.Value.prefab;

        // Don't animate if prefab is already set
        if (target.GetEmotion() == emotion && target.IsShowing) yield break;

        if (target.IsShowing)
        {
            target.Hide();
            yield return new WaitForSeconds(target.HideTime);
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