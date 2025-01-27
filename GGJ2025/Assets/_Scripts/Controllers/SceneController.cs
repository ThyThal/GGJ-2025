using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneController : MonoBehaviour
{
    /*[SerializeField] private CharacterObject characterPrefab;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Vector3 otherCharacterPosition;*/
    
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private BubblesUIManager bubblesUIManager;
    [SerializeField] private DecisionsUIManager decisionsUIManager;
    [SerializeField] private GameUIManager gameUIManager;
    [SerializeField] SpecialEvents specialEvents;
    [SerializeField] private SpriteRenderer backgroundRenderer;
    [SerializeField] private Image barRenderer;
    [SerializeField] private Image timerFill;
    [SerializeField] private CharacterObject playerCharacter;
    [SerializeField] private CharacterObject otherCharacter;    

    [SerializeField] TimerUI timerUI;
    
    [SerializeField] AudioSource audioSource;

    SceneDataSO currentSceneData;

    private float decisionTimer;

    private bool isWritingDialogue = false;
    bool skippedDialogue = false;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TrySkipDialogue();
        }
        
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
        if (dialogue.isInteractionEnd && dialogue.ending != GameEvents.Intro && dialogue.ending != GameEvents.None)
        {
            specialEvents.PlayEvent(dialogue.ending);
            return;
        }
        // Activate Next/Skip button, fade and load next dialogue
        if (dialogue.isInteractionEnd || dialogue.GetNextDialogue(playerDecision) == null) // Ended interaction, lock/unlock character, go back to selection
        {
            if(dialogue.characterToBlock) 
            {
                GameProgression.Instance.TryLockCharacter(dialogue.characterToBlock);
                // Avoid making both sounds at once
                if(!dialogue.characterToUnlock) GameManager.Instance.audioManager.PlayLockCharacter();
            }
            if (dialogue.characterToUnlock)
            {
                GameProgression.Instance.TryUnlockCharacter(dialogue.characterToUnlock);
                GameManager.Instance.audioManager.PlayUnlockCharacter();
            }

            if (GameProgression.Instance.TotalUnlockedCharacters == 0)
            {
                // BAD ENDING
                specialEvents.PlayEvent(GameEvents.FinalBurbujaExplota);
                return;
            }
            
            gameUIManager.ShowBackButton(true);
        }
        else
        {
            gameUIManager.ShowNextButton(true);
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
            target.UpdateEmotion(newLine.emotion, true);
            StartCoroutine(WriteDialogueLineRoutine(newLine));
        }
        else // Start decision process
        {
            // WriteDecisionLine();
            StartCoroutine(WriteDecisionDialogue(newLine));
            StartDecisionTimer(dialogueController.CurrentDialogue, newLine);
        }
    }
    private void DialogueChangedHandler(DialogueNodeSO newDialogue)
    {
        StartCoroutine(DialogueChangeCoroutine(newDialogue));
    }

    IEnumerator DialogueChangeCoroutine(DialogueNodeSO newDialogue)
    {
        if (newDialogue.sceneData != currentSceneData)
        {
            UpdateScene(newDialogue.sceneData);
        }
        
        bubblesUIManager.HideAllBubbles();
        
        // Checking IsActive to be able to use this SceneController even in cutscenes where there are no characters, or only one
        if(playerCharacter.IsActive) playerCharacter.UpdateEmotion(newDialogue.initialPlayerEmotion);
        if(otherCharacter.IsActive) otherCharacter.UpdateEmotion(newDialogue.initialOtherEmotion);

        // ...Fade In
        yield return new WaitForSeconds(FadeController.Instance.TryFadeIn());
        
        // ...Move character in, if existent
        if (newDialogue.sceneData.otherCharacter) yield return new WaitForSeconds(UpdateCharacter(newDialogue.sceneData.otherCharacter));
        
        if (dialogueController.CurrentDialogue.TotalLines == 0) yield break;
        
        // Force start of new dialog's lines
        LineChangedHandler(dialogueController.CurrentLine);
        

        // TODO: Capaz el tipo de reacción debería estar definida en cáda línea de dialogo, y también la velocidad en la que se escribe
    }

    private void UpdateScene(SceneDataSO newDialogueSceneData)
    {
        currentSceneData = newDialogueSceneData;
        PlaySceneSong(newDialogueSceneData);
        
        backgroundRenderer.sprite = currentSceneData.background;
        barRenderer.sprite = currentSceneData.bar;
        timerFill.color = currentSceneData.color;

        // If new scene has character data, and it is different from current, update data and leave scene
        if (currentSceneData.otherCharacter)
        {
            if (otherCharacter.CurrentData != currentSceneData.otherCharacter)
            {
                otherCharacter.ForceLeaveScene();
                //otherCharacter.SetData(currentSceneData.otherCharacter);
            }
        }
        else otherCharacter.ForceLeaveScene();
        
    }

    float UpdateCharacter(CharacterDataSO characterData)
    {
        if (otherCharacter.CurrentData != characterData) otherCharacter.SetData(characterData);
        else return 0;
            
        // Enter scene if unactive
        if (!otherCharacter.IsActive)
        {
            otherCharacter.EnterScene();
            return 1.5f; // Hardcoded: character entry time
        }
        else return 0; // Should never reach this
    }

    private void StartDecisionTimer(DialogueNodeSO currentDialogue, DialogueLine currentLine)
    {
        decisionsUIManager.Show();
        decisionsUIManager.Setup(currentDialogue.GetPossibleEmotions());
        
        //ResetDecision();
        timerUI.UpdateMax(currentLine.decisionTime); // Cambiar a que el max y timer dependan de la line, testeando ahora
        decisionTimer = currentLine.decisionTime;
        
        // Start timer, wait for player decision

    }

    private void PlaySceneSong(SceneDataSO sceneData)
    {
        if (audioSource.clip == sceneData.music || audioSource.isPlaying) return;
        audioSource.clip = sceneData.music;
        audioSource.Play();
    }

    private void StopSceneSong()
    {
        audioSource.Stop();
    }

    private void ResetDecision()
    {
        decisionTimer = 0;
        playerChose = false;
    }

    //TODO: Capaz mover a un SceneController o algo? ya tiene muchas funciones esta clase
    public void BackToCharacterSelection()
    {
        StopSceneSong();
        SceneManager.LoadScene(1);
    }

    public void BackToMenu()
    {
        StopSceneSong();
        SceneManager.LoadScene(0);
    }

    public void OnClickNextButton()
    {
        StartCoroutine(MoveToNextDialogue());
    }
    public IEnumerator MoveToNextDialogue()
    {
        // TODO: Make character leave scene here probably
        gameUIManager.ShowNextButton(false);
        
        // Only fade out if something in scene changes
        if(dialogueController.PeekNextDialogue(playerDecision)?.sceneData != currentSceneData ) yield return new WaitForSeconds(FadeController.Instance.FadeOut());
        
        // Get next dialogue according to choice
        dialogueController.NextDialogue(playerDecision);
        
        // Reset player choice
        playerDecision = default;
        playerChose = false;
        decisionTimer = 0;
        
    }

    public void OnPlayerChoseHandler(Emotion decision)
    {
        if (playerChose) return;
        
        playerChose = true;
        playerDecision = decision;

        playerCharacter.UpdateEmotion(decision, true);
        decisionsUIManager.Hide();
        StartCoroutine(WriteDialogueLineRoutine(dialogueController.CurrentLine, true));
        
        var branch = dialogueController.CurrentDialogue
            .GetEmotionLineBranch(decision, dialogueController.CurrentLineIndex).branchLines;
        if(branch is not null) dialogueController.AddNewLines(branch);
    }
    
    private IEnumerator WriteDecisionDialogue(DialogueLine newLine)
    {
        // TODO: Maybe do Coroutine as well, and animate Undefined bubble
        // To write line that needs a decision, before deciding and rewriting it with animation and audio
        var targetBubble = bubblesUIManager.GetTargetBubble(newLine.isPlayerLine);
        // Send Line as a parameter to force writing before bubble animation
        yield return bubblesUIManager.AnimateBubble(targetBubble, Emotion.Thinking, newLine.dialogueID);
        //targetBubble.GetCurrentBubbleComponents().textComponent.text = newLine.dialogueID;
    }
    
    private IEnumerator WriteDialogueLineRoutine(DialogueLine newLine, bool isDecisionLine = false)
    {
        // Debug: Log the beginning of the method and the parameters
        Debug.Log($"WriteDialogueLineRoutine started. Player Line: {newLine.isPlayerLine}, Decision Line: {isDecisionLine}");

        var targetBubble = bubblesUIManager.GetTargetBubble(newLine.isPlayerLine);
        
        // Debug: Log the selected bubble
        if (targetBubble == null)
        {
            Debug.LogError("Target bubble is null.");
        }
        else
        {
            Debug.Log($"Target bubble for {newLine.isPlayerLine} is: {targetBubble.gameObject.name}");
        }

        targetBubble.gameObject.SetActive(true);

        // Clear the existing text in the bubble
        var bubbleComponents = targetBubble.GetCurrentBubbleComponents();
        if (bubbleComponents == null)
        {
            Debug.LogError("BubbleComponents not found in the target bubble.");
        }
        else
        {
            bubbleComponents.textComponent.text = string.Empty;
        }

        char[] line = newLine.dialogueID.ToCharArray();
        
        // Animate the bubble with the appropriate emotion
        yield return bubblesUIManager.AnimateBubble(targetBubble, newLine.isDecisionLine ? playerDecision : newLine.emotion);

        var textComp = targetBubble.GetCurrentBubbleComponents().textComponent;
        textComp.text = string.Empty;
        isWritingDialogue = true;
        for (int i = 0; i < line.Length; i++)
        {
            textComp.text += line[i];
            // Play audio based on the emotion of the line
            dialogueController.writingAudio.PlayEmotionAudio(newLine.emotion);
            yield return new WaitForSeconds(newLine.typeSpeed);

            if (skippedDialogue)
            {
                // If skipped, write dialogue and break for
                textComp.text = newLine.dialogueID;
                break;
            }
        }
        
        skippedDialogue = false;
        isWritingDialogue = false;

        yield return new WaitForSeconds(newLine.dialogueTime);
        
        // Debug: Log end of dialogue line writing
        Debug.Log("Dialogue line writing complete.");

        // Move to the next line
        dialogueController.NextLine();
    }

    public void TrySkipDialogue()
    {
         if(isWritingDialogue) skippedDialogue = true;
    }
}