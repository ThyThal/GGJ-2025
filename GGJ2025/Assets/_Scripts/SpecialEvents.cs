using System;
using System.Collections.Generic;
using _Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpecialEvents : MonoBehaviour
{
    [SerializeField] private CutsceneController cutsceneController;
    [TableList] public List<GameEventEntry> specialEvents = new();
    
    public void PlayEvent(GameEvents eventType)
    {
        GameEventEntry eventEntry = specialEvents.Find(e => e.eventType == eventType);
        if (eventEntry == null)
        {
            Debug.LogError("Could not find event entry for " + eventType);
            return;
        }
        
        cutsceneController.Play(eventEntry.spriteDisplays);
    }
}

[Serializable]
public class GameEventEntry
{
    public GameEvents eventType;

    [TableList(ShowIndexLabels = true)]
    public List<SpriteDisplay> spriteDisplays = new List<SpriteDisplay>();
}

[Serializable]
public class SpriteDisplay
{
    [HideLabel] public Sprite sprite;
    [Range(2f, 10f)] public float displayTime = 2f;
}