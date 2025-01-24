using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu( fileName = "New Dialogue Node", menuName = "Scriptable Objects/New Dialogue Node")]
public class DialogueNodeSO : ScriptableObject
{
    [SerializeField] private EmotionToDialogue[] nextDialogues;
    
    public DialogueLine[] dialogueLines;
    public Emotion initialPlayerEmotion;
    public Emotion initialOtherEmotion;
    public int TotalLines => dialogueLines.Length;

    public SceneDataSO sceneData;
    
    // Normal decision passed as default parameter, to use with normal dialogues that don't require a player decision
    public DialogueNodeSO GetNextDialogue(Emotion decision = Emotion.Normal) => nextDialogues.FirstOrDefault(d => d.emotion == decision).dialogueNode;

    /// <summary>
    /// Get all possible emotions in this Dialogue, to populate decisions UI
    /// </summary>
    /// <returns></returns>
    public Emotion[] GetPossibleEmotions() => nextDialogues.Select(d => d.emotion).ToArray();
}

[Serializable]
public struct DialogueLine
{
    public bool isPlayerLine;
    [TextArea] public string dialogueID;
    public bool isDecisionLine;
    public Emotion emotion;
}

[Serializable]
public struct EmotionToDialogue
{
    public Emotion emotion;
    public DialogueNodeSO dialogueNode;
}

public enum Emotion
{
    Normal,
    Scream,
    Thinking
}
