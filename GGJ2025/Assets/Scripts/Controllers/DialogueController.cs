using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueNodeSO initialDialogue;

    private int _currentLineIndex = 0;
    DialogueNodeSO _currentDialogue;
    
    public DialogueNodeSO CurrentDialogue => _currentDialogue;
    public int CurrentLineIndex => _currentLineIndex;
    public DialogueLine CurrentLine => _currentDialogue.dialogueLines[_currentLineIndex];
    public event Action<DialogueNodeSO> OnDialogueChanged;
    public event Action<DialogueLine> OnDialogueLineChanged;
    public event Action OnDialogueFinished;

    private void Awake()
    {
        _currentDialogue = initialDialogue;
    }

    void Start()
    {
        // TODO: Para iniciar facil, cambiar
        OnDialogueChanged?.Invoke(_currentDialogue);
    }
    public void NextDialogue(Emotion decision = Emotion.Normal)
    {
        // Reset line index
        _currentLineIndex = 0;
        
        var previousDialogue = _currentDialogue;
        _currentDialogue = previousDialogue.GetNextDialogue(decision);
        
        OnDialogueChanged?.Invoke(_currentDialogue);
    }

    // TODO: A definir: El dialogo a elegir siempre se escribe ultimo? Se van a escribir todos los dialogos a la vez y se elige solo la burbuja?
    // TODO: Se va a escribir siempre un dialogo de otro personaje y luego del jugador? O se podran intercalar libremente?
    public void NextLine()
    {
        if (_currentLineIndex < _currentDialogue.TotalLines - 1)
        {
            _currentLineIndex++;
            var newLine = _currentDialogue.dialogueLines[_currentLineIndex];
            
            OnDialogueLineChanged?.Invoke(newLine);
        }
        else OnDialogueFinished?.Invoke();
    }
}
