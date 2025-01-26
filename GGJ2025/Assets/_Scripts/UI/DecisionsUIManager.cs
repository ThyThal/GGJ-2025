using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DecisionsUIManager : MonoBehaviour
{
    [SerializeField] private DecisionUI[] decisionButtons;
    [SerializeField] private EmotionSpriteDataSO decisionSprites;

    private void Awake()
    {
        foreach (var button in decisionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public event Action<Emotion> OnPlayerClickedDecision;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        foreach (var button in decisionButtons)
        {
            button.gameObject.SetActive(false);
        }
        
        gameObject.SetActive(false);
    }
    
    public void Setup(Emotion[] emotions)
    {
        Debug.Log(emotions);
        for (int i = 0; i < emotions.Length; i++)
        {
            //Debug.LogWarning("Setting up " + decisionButtons[i].name + " with emotion " + emotions[i]);
            decisionButtons[i].gameObject.SetActive(true);
            decisionButtons[i].Setup(decisionSprites.GetSpriteFromEmotion(emotions[i]), emotions[i]);
        }
    }

    public void PlayerDecided(Emotion emotion)
    {
        OnPlayerClickedDecision?.Invoke(emotion);
    }
}
