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
    [SerializeField] private SpriteRenderer backgroundRenderer;
    
    [SerializeField] private CharacterObject playerCharacter;
    [SerializeField] private CharacterObject otherCharacter;    

    [SerializeField] private GameObject nextButton;

    SceneDataSO currentSceneData;

    private bool isWritingDialogue = false;
    private bool playerChose = false;
    private Emotion playerChoice;
    private void OnEnable()
    {
        dialogueController.OnDialogueChanged += DialogueChangedHandler;
        dialogueController.OnDialogueLineChanged += LineChangedHandler;
        dialogueController.OnDialogueFinished += DialogueFinishedHandler;
    }

    private void OnDisable()
    {
        dialogueController.OnDialogueChanged -= DialogueChangedHandler;
        dialogueController.OnDialogueLineChanged -= LineChangedHandler;
        dialogueController.OnDialogueFinished -= DialogueFinishedHandler;
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
        else
        {
            // WriteDecisionLine();
            StartDialogueTimer(dialogueController.CurrentDialogue, newLine);
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

    private void StartDialogueTimer(DialogueNodeSO currentDialogue, DialogueLine currentLine)
    {
        // Start timer, wait for player decision
    }
    

    public void MoveToNextDialogue()
    {
        // Get next dialogue according to choice
        dialogueController.NextDialogue(playerChoice);
        
        // Reset player choice
        playerChoice = default;
        playerChose = false;
        
        nextButton.SetActive(false);
    }

    public void OnPlayerChoseHandler()
    {
        
    }
    
    private void WriteDialogueLine(DialogueLine newLine, int index)
    {
        
    }
    
    private IEnumerator WriteDialogueLineRoutine(DialogueLine newLine, int index)
    {
        var bubble = bubblesUIManager.GetBubbleTarget(newLine.isPlayerLine, index, newLine.emotion);
        isWritingDialogue = true;

        char[] line = newLine.dialogueID.ToCharArray();
        
        for (int i = 0; i < line.Length; i++)
        {
            bubble.text += line[i];
            yield return new WaitForSeconds(0.05f);
        }
        
        yield return new WaitForSeconds(1f);

        isWritingDialogue = false;
        
        dialogueController.NextLine();
    }
}