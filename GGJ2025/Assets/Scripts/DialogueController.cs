using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueNodeSO initialDialogue;

    private int currentLineIndex = 0;
    DialogueNodeSO currentDialogue;
    
    public event Action<DialogueNodeSO> OnDialogueChanged;
    public event Action<DialogueLine> OnDialogueLineChanged;
    void Start()
    {
        currentDialogue = initialDialogue;
    }
    
    public void NextDialogue(BubbleType decision = BubbleType.Normal)
    {
        // Reset line index
        
        currentLineIndex = 0;
        var previousDialogue = currentDialogue;
        currentDialogue = previousDialogue.GetNextDialogue(decision);
        
        OnDialogueChanged?.Invoke(currentDialogue);
    }

    // TODO: A definir: El dialogo a elegir siempre se escribe ultimo? Se van a escribir todos los dialogos a la vez y se elige solo la burbuja?
    // TODO: Se va a escribir siempre un dialogo de otro personaje y luego del jugador? O se podran intercalar libremente?
    public void NextLine()
    {
        if (currentLineIndex < currentDialogue.TotalLines - 1)
        {
            currentLineIndex++;
            var newLine = currentDialogue.dialogueLines[currentLineIndex];
            
            OnDialogueLineChanged?.Invoke(newLine);
        }
    }
}
