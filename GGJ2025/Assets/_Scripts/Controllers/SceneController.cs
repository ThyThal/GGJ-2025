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
        if (dialogue.isInteractionEnd || dialogue.GetNextDialogue(playerDecision) == null) // Ended interaction, lock/unlock character, go back to selection
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

        if (newDialogueSceneData.music)
        {
            audioSource.clip = newDialogueSceneData.music;
            audioSource.Play();
        } 
        
        
        backgroundRenderer.sprite = currentSceneData.background;

        // If new scene has character data, and it is different than current, update data and leave scene
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
        timerUI.UpdateMax(5); // Cambiar a que el max y timer dependan de la line, testeando ahora
        decisionTimer = currentLine.decisionTime;
        
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

    public void OnClickNextButton()
    {
        StartCoroutine(MoveToNextDialogue());
    }
    public IEnumerator MoveToNextDialogue()
    {
        // TODO: Make character leave scene here probably
        nextButton.SetActive(false);
        
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

        playerCharacter.UpdateEmotion(decision);
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
        yield return bubblesUIManager.AnimateBubble(targetBubble, Emotion.Undefined);
        targetBubble.TextComponent.text = newLine.dialogueID;
    }
    
    private IEnumerator WriteDialogueLineRoutine(DialogueLine newLine, bool isDecisionLine = false)
    {
        //TODO: ACHICAR LA OTRA BURBUJA Y OSCURECER
        var targetBubble = bubblesUIManager.GetTargetBubble(newLine.isPlayerLine);
        isWritingDialogue = true;
        targetBubble.TextComponent.text = string.Empty;
        char[] line = newLine.dialogueID.ToCharArray();
        
        // TODO: WaitForSeconds dictionary?
        yield return bubblesUIManager.AnimateBubble(targetBubble, newLine.isDecisionLine ? playerDecision : newLine.emotion);
        
        for (int i = 0; i < line.Length; i++)
        {
            targetBubble.TextComponent.text += line[i];
            dialogueController.writingAudio.PlayEmotionAudio(newLine.emotion);
            yield return new WaitForSeconds(newLine.typeSpeed);
        }
        
        yield return new WaitForSeconds(newLine.dialogueTime);

        isWritingDialogue = false;
        //yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        dialogueController.NextLine();
    }
}