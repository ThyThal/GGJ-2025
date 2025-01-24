using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    /*[SerializeField] private CharacterObject characterPrefab;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Vector3 otherCharacterPosition;*/
    
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private BubblesUIManager bubblesUIManager;
    [SerializeField] private DecisionsUIManager decisionsUIManager;
    [SerializeField] private SpriteRenderer backgroundRenderer;
    
    [SerializeField] private CharacterObject playerCharacter;
    [SerializeField] private CharacterObject otherCharacter;    

    [SerializeField] private GameObject nextButton;

    SceneDataSO currentSceneData;

    private float decisionTimer;

    private bool isWritingDialogue = false;
    private bool playerChose = false;
    private Emotion playerDecision;
    private void OnEnable()
    {
        dialogueController.OnDialogueChanged += DialogueChangedHandler;
        dialogueController.OnDialogueLineChanged += LineChangedHandler;
        dialogueController.OnDialogueFinished += DialogueFinishedHandler;
        decisionsUIManager.OnPlayerClickedDecision += OnPlayerChoseHandler;
    }

    private void OnDisable()
    {
        dialogueController.OnDialogueChanged -= DialogueChangedHandler;
        dialogueController.OnDialogueLineChanged -= LineChangedHandler;
        dialogueController.OnDialogueFinished -= DialogueFinishedHandler;
        decisionsUIManager.OnPlayerClickedDecision += OnPlayerChoseHandler;
    }

    private void Update()
    {
        if (decisionTimer > 0)
        {
            decisionTimer -= Time.deltaTime;
            if (decisionTimer <= 0)
            {
                
                playerChose = true;
            }
        }
    }

    private void DialogueFinishedHandler()
    {
        // Activate Next/Skip button, fade and load next dialogue
        nextButton.SetActive(true);
        
    }
    
    private void LineChangedHandler(DialogueLine newLine)
    {
        var target = newLine.isPlayerLine ? playerCharacter : otherCharacter;
        
        // Update target character's face and voice according to emotion
        target.UpdateEmotion(newLine.emotion);
        
        // TODO: Si es decisiva, escribir la linea en UI de una, capaz en gris? y luego de elegir la burbuja escribirla con el sonido correspondiente y tal vez un efectito
        // Write line if not decisive, else, start timer and wait for player to choose bubble
        if (!newLine.isDecisionLine)
        {
            StartCoroutine(WriteDialogueLineRoutine(newLine, dialogueController.CurrentLineIndex));
        }
        else // Start decision process
        {
            // WriteDecisionLine();
            WriteDialogueLineInstantly(newLine, dialogueController.CurrentLineIndex);
            StartDecisionTimer(dialogueController.CurrentDialogue, newLine);
        }
    }
    private void DialogueChangedHandler(DialogueNodeSO newDialogue)
    {
       // ...Wait FadeIn/Transition
        if (newDialogue.sceneData != currentSceneData)
        {
            UpdateScene(newDialogue.sceneData);
        }
        
        bubblesUIManager.HideAllBubbles();
        
        // Checking IsActive to be able to use this SceneController even in cutscenes where there are no characters, or only one
        if(playerCharacter.IsActive) playerCharacter.UpdateEmotion(newDialogue.initialPlayerEmotion);
        if(otherCharacter.IsActive) otherCharacter.UpdateEmotion(newDialogue.initialOtherEmotion);
        // ...Fade Out
        
        // ...Start delay

        if (dialogueController.CurrentDialogue.TotalLines == 0) return;
        
        LineChangedHandler(dialogueController.CurrentLine);
        

        // TODO: Capaz el tipo de reacción debería estar definida en cáda línea de dialogo, y también la velocidad en la que se escribe
    }

    private void UpdateScene(SceneDataSO newDialogueSceneData)
    {
        currentSceneData = newDialogueSceneData;

        backgroundRenderer.sprite = currentSceneData.background;
        otherCharacter.SetData(currentSceneData.otherCharacter);
    }

    private void StartDecisionTimer(DialogueNodeSO currentDialogue, DialogueLine currentLine)
    {
        decisionsUIManager.Setup(currentDialogue.GetPossibleEmotions());
        
        //ResetDecision();
        decisionTimer = currentLine.dialogueTime;
        
        // Start timer, wait for player decision

    }

    private void ResetDecision()
    {
        decisionTimer = 0;
        playerChose = false;
    }

    public void MoveToNextDialogue()
    {
        // Get next dialogue according to choice
        dialogueController.NextDialogue(playerDecision);
        
        // Reset player choice
        playerDecision = default;
        playerChose = false;
        decisionTimer = 0;
        
        nextButton.SetActive(false);
    }

    public void OnPlayerChoseHandler(Emotion decision)
    {
        if (playerChose) return;
        
        playerChose = true;
        playerDecision = decision;

        decisionsUIManager.Hide();
        StartCoroutine(WriteDialogueLineRoutine(dialogueController.CurrentLine, dialogueController.CurrentLineIndex, true));
    }
    
    private void WriteDialogueLineInstantly(DialogueLine newLine, int index)
    {
        // To write line that needs a decision, before deciding and rewriting it with animation and audio
        var bubble = bubblesUIManager.GetBubbleTarget(newLine.isPlayerLine, index, Emotion.Undefined);
    }
    
    private IEnumerator WriteDialogueLineRoutine(DialogueLine newLine, int index, bool isDecisionLine = false)
    {
        var bubble = bubblesUIManager.GetBubbleTarget(newLine.isPlayerLine, index, isDecisionLine ? playerDecision : newLine.emotion);
        isWritingDialogue = true;

        char[] line = newLine.dialogueID.ToCharArray();
        
        // TODO: Wait for bubble anim to show before start writing
        
        for (int i = 0; i < line.Length; i++)
        {
            bubble.text += line[i];
            yield return new WaitForSeconds(newLine.typeSpeed);
        }
        
        yield return new WaitForSeconds(newLine.dialogueTime);

        isWritingDialogue = false;
        
        dialogueController.NextLine();
    }
}