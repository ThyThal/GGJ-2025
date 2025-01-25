using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private DialogueNodeSO initialDialogue;
    [SerializeField] public WritingAudio writingAudio;
    [SerializeField] private bool testingScene;

    private int _currentLineIndex = 0;
    DialogueNodeSO _currentDialogue;
    
    public DialogueNodeSO CurrentDialogue => _currentDialogue;
    public int CurrentLineIndex => _currentLineIndex;
    public DialogueLine CurrentLine => _currentDialogue.dialogueLines[_currentLineIndex];
    public event Action<DialogueNodeSO> OnDialogueChanged;
    public event Action<DialogueLine> OnDialogueLineChanged;
    public event Action<DialogueNodeSO> OnDialogueFinished;
    public event Action OnInteractionFinished;

    private List<DialogueLine> currentLines;

    private void Awake()
    {
        //if (!testingScene) return;
        //_currentDialogue = initialDialogue;
        //currentLines = initialDialogue.dialogueLines.ToList();
    }

    void Start()
    {
        // TODO: Para iniciar facil, cambiar
        if(testingScene) StartDialogues(initialDialogue);
        //OnDialogueChanged?.Invoke(_currentDialogue);
    }

    public void StartDialogues(DialogueNodeSO initialNode)
    {
        //StartCoroutine(StartDialogueCoroutine(initialNode));
        _currentDialogue = initialNode;
        currentLines = initialNode.dialogueLines.ToList();
        
        OnDialogueChanged?.Invoke(_currentDialogue);
    }

    IEnumerator StartDialogueCoroutine(DialogueNodeSO initialNode)
    {
        yield return new WaitForSeconds(FadeController.Instance.FadeIn());
        
        _currentDialogue = initialNode;
        currentLines = initialNode.dialogueLines.ToList();
        
        OnDialogueChanged?.Invoke(_currentDialogue);
    }
    
    public DialogueNodeSO PeekNextDialogue(Emotion decision) => _currentDialogue.GetNextDialogue(decision);
    public void NextDialogue(Emotion decision = Emotion.Normal)
    {
        // Reset line index
        currentLines.Clear();
        _currentLineIndex = 0;
        
        var previousDialogue = _currentDialogue;
        _currentDialogue = previousDialogue.GetNextDialogue(decision);
        
        // Is dialogue tree ending
        if (_currentDialogue.isInteractionEnd)
        {
            OnInteractionFinished?.Invoke();
        }
        
        OnDialogueChanged?.Invoke(_currentDialogue);
        currentLines = _currentDialogue.dialogueLines.ToList();
    }

    // TODO: A definir: El dialogo a elegir siempre se escribe ultimo? Se van a escribir todos los dialogos a la vez y se elige solo la burbuja?
    // TODO: Se va a escribir siempre un dialogo de otro personaje y luego del jugador? O se podran intercalar libremente?
    public void NextLine()
    {
        if (_currentLineIndex < currentLines.Count - 1)
        {
            _currentLineIndex++;
            var newLine = currentLines[_currentLineIndex];
            
            OnDialogueLineChanged?.Invoke(newLine);
        }
        else OnDialogueFinished?.Invoke(_currentDialogue);
    }

    public void AddNewLines(DialogueLine[] newLines)
    {
        //if (newLines is null) return;
        currentLines.AddRange(newLines);
    }
}
