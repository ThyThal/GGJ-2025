using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DecisionsUIManager : MonoBehaviour
{
    [SerializeField] private DecisionUI[] decisionButtons;
    [FormerlySerializedAs("bubbleSprites")] [SerializeField] private EmotionSpriteDataSO spriteSprites;

    public event Action<Emotion> OnPlayerClickedDecision;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    public void Setup(Emotion[] emotions)
    {
        for (int i = 0; i < decisionButtons.Length; i++)
        {
            decisionButtons[i].Setup(spriteSprites.GetSpriteFromEmotion(emotions[i]), emotions[i]);
        }
    }

    public void PlayerDecided(Emotion emotion)
    {
        OnPlayerClickedDecision?.Invoke(emotion);
    }
}
