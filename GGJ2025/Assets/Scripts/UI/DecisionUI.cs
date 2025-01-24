using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DecisionUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private DecisionsUIManager manager;
    Emotion emotion;

    public void OnClick()
    {
        manager.PlayerDecided(emotion);
    }
    
    public void Setup(Sprite sprite, Emotion value)
    {
        image.sprite = sprite;
        emotion = value;
    }
}
