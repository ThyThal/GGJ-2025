using System;
using System.Collections.Generic;
using _Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpecialEvents : MonoBehaviour
{
    [SerializeField] private CutsceneController cutsceneController;
    [TableList] public List<GameEventEntry> specialEvents = new();
    GameEvents currentGameEvent;

    public event Action<GameEvents> OnEventEnded;

    private void OnEnable()
    {
        cutsceneController.OnCutsceneFinished += CutsceneFinishedHandler;
    }

    private void CutsceneFinishedHandler()
    {
        OnEventEnded?.Invoke(currentGameEvent);
    }

    public void PlayEvent(GameEvents eventType)
    {
        currentGameEvent = eventType;
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
    [Range(0.1f, 10f)] public float displayTime = 2f;
}