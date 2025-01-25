using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu( fileName = "New Dialogue Node", menuName = "Scriptable Objects/New Dialogue Node")]
public class DialogueNodeSO : ScriptableObject
{
    [Header("Next Dialogue Nodes")]
    [SerializeField] private EmotionToDialogue[] nextDialogues;
    [Space]
    [Header("Dialogue Lines")]
    public DialogueLine[] dialogueLines;
    [Space]
    [Header("Initial Emotions")]
    public Emotion initialPlayerEmotion;
    public Emotion initialOtherEmotion;
    public int TotalLines => dialogueLines.Length;
    [Space]
    [Header("Scene Data")]
    public SceneDataSO sceneData;

    [Header("Interaction End")] 
    public bool isInteractionEnd;
    public CharacterDataSO characterToUnlock;
    public CharacterDataSO characterToBlock;
    
    // Normal decision passed as default parameter, to use with normal dialogues that don't require a player decision
    public DialogueNodeSO GetNextDialogue(Emotion decision = Emotion.Normal) => nextDialogues.FirstOrDefault(d => d.emotion == decision).dialogueNode;
    public EmotionLinesBranch GetEmotionLineBranch(Emotion decision, int index) => dialogueLines[index].branches.FirstOrDefault(b => b.emotion == decision);

    /// <summary>
    /// Get all possible emotions in this Dialogue, to populate decisions UI
    /// </summary>
    /// <returns></returns>
    public Emotion[] GetPossibleEmotions() => nextDialogues.Select(d => d.emotion).ToArray();
}

[Serializable]
public struct DialogueLine
{
    [Header("Is Player line?")]
    public bool isPlayerLine;
    [Header("Dialogue Text")]
    [TextArea] public string dialogueID;
    [Header("Is Decisive?")]
    public bool isDecisionLine;
    [Header("Emotion")]
    public Emotion emotion;
    [Header("Time")]
    [Range(0.03f, 0.1f)]
    public float typeSpeed;
    public float dialogueTime;
    [Header("Branching")]
    public EmotionLinesBranch[] branches;
}

[Serializable]
public struct EmotionToDialogue
{
    public Emotion emotion;
    public DialogueNodeSO dialogueNode;
}

[Serializable]
public struct EmotionLinesBranch
{
    public Emotion emotion;
    public DialogueLine[] branchLines;
}

public enum Emotion
{
    Normal,
    Scream,
    Thinking,
    Undefined
}
