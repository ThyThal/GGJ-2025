using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "New Dialogue Node", menuName = "Scriptable Objects/New Dialogue Node")]
public class DialogueNodeSO : ScriptableObject
{
    public DialogueLine[] dialogueLines;
    [SerializeField] private bool isDecision;

    [SerializeField] private DialogueNodeSO normalNextDialogue;
    [SerializeField] private DialogueNodeSO screamNextDialogue;
    [SerializeField] private DialogueNodeSO thinkingNextDialogue;
    public bool IsDecision => isDecision;
    public int TotalLines => dialogueLines.Length;
    
    // Normal decision passed as default parameter, to use with normal dialogues that don't require a player decision
    public DialogueNodeSO GetNextDialogue(BubbleType decision = BubbleType.Normal)
    {
        switch (decision)
        {
            case BubbleType.Normal:
                return normalNextDialogue;
            case BubbleType.Scream:
                return screamNextDialogue;
            case BubbleType.Thinking:
                return thinkingNextDialogue;
            default:
                throw new ArgumentOutOfRangeException(nameof(decision), decision, null);
        }
    }
}

public struct DialogueLine
{
    public string dialogueID;
    public bool player;
    public bool isDecisionLine;
}

public enum BubbleType
{
    Normal,
    Scream,
    Thinking
}
