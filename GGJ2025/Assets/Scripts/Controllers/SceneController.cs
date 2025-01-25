using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    [SerializeField] private GameObject backButton;
    [SerializeField] TimerUI timerUI;
    
    [SerializeField] AudioSource audioSource;

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
        if (!playerChose && decisionTimer > 0)
        {
            decisionTimer -= Time.deltaTime;
            timerUI.UpdateFill(decisionTimer);
            if (decisionTimer <= 0)
            {
                Debug.Log("Player did not decide on time");
                //playerChose = true;
                var possibilities = dialogueController.CurrentDialogue.GetPossibleEmotions();
                OnPlayerChoseHandler(possibilities[Random.Range(0, possibilities.Length)]);
            }
        }
    }

    private void DialogueFinishedHandler(DialogueNodeSO dialogue)
    {
        // Activate Next/Skip button, fade and load next dialogue
        if (dialogue.isInteractionEnd || dialogue.GetNextDialogue() == null) // Ended interaction, lock/unlock character, go back to selection
        {
            if(dialogue.characterToBlock) GameProgression.Instance.TryLockCharacter(dialogue.characterToBlock);
            if(dialogue.characterToUnlock) GameProgression.Instance.TryUnlockCharacter(dialogue.characterToUnlock);
            backButton.SetActive(true);
        }
        else
        {
            nextButton.SetActive(true);
        }
    }
    
    private void LineChangedHandler(DialogueLine newLine)
    {
        var target = newLine.isPlayerLine ? playerCharacter : otherCharacter;
        
        // TODO: Si es decisiva, escribir la linea en UI de una, capaz en gris? y luego de elegir la burbuja escribirla con el sonido correspondiente y tal vez un efectito
        // Write line if not decisive, else, start timer and wait for player to choose bubble
        if (!newLine.isDecisionLine)
        {
            // Update target character's face and voice according to emotion
            target.UpdateEmotion(newLine.emotion);
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

        if (newDialogueSceneData.music)
        {
            audioSource.clip = newDialogueSceneData.music;
            audioSource.Play();
        } 
        backgroundRenderer.sprite = currentSceneData.background;
        otherCharacter.SetData(currentSceneData.otherCharacter);
    }

    private void StartDecisionTimer(DialogueNodeSO currentDialogue, DialogueLine currentLine)
    {
        decisionsUIManager.Show();
        decisionsUIManager.Setup(currentDialogue.GetPossibleEmotions());
        
        //ResetDecision();
        timerUI.UpdateMax(5); // Cambiar a que el max y timer dependan de la line, testeando ahora
        decisionTimer = 5f;
        
        // Start timer, wait for player decision

    }

    private void ResetDecision()
    {
        decisionTimer = 0;
        playerChose = false;
    }

    //TODO: Capaz mover a un SceneController o algo? ya tiene muchas funciones esta clase
    public void BackToCharacterSelection()
    {
        SceneManager.LoadScene(1);
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

        playerCharacter.UpdateEmotion(decision);
        decisionsUIManager.Hide();
        StartCoroutine(WriteDialogueLineRoutine(dialogueController.CurrentLine, dialogueController.CurrentLineIndex, true));
        dialogueController.AddNewLines(dialogueController.CurrentDialogue.GetEmotionLineBranch(decision, dialogueController.CurrentLineIndex).branchLines);
    }
    
    private void WriteDialogueLineInstantly(DialogueLine newLine, int index)
    {
        // To write line that needs a decision, before deciding and rewriting it with animation and audio
        var bubble = bubblesUIManager.GetBubbleTarget(newLine.isPlayerLine, index, Emotion.Undefined);
        bubble.text = newLine.dialogueID;
    }
    
    private IEnumerator WriteDialogueLineRoutine(DialogueLine newLine, int index, bool isDecisionLine = false)
    {
        //TODO: ACHICAR LA OTRA BURBUJA Y OSCURECER
        var bubble = bubblesUIManager.GetBubbleTarget(newLine.isPlayerLine, index, isDecisionLine ? playerDecision : newLine.emotion);
        isWritingDialogue = true;
        bubble.text = string.Empty;
        char[] line = newLine.dialogueID.ToCharArray();
        
        // TODO: Wait for bubble anim to show before start writing
        
        for (int i = 0; i < line.Length; i++)
        {
            bubble.text += line[i];
            yield return new WaitForSeconds(newLine.typeSpeed);
        }
        
        yield return new WaitForSeconds(newLine.dialogueTime);

        isWritingDialogue = false;
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        dialogueController.NextLine();
    }
}