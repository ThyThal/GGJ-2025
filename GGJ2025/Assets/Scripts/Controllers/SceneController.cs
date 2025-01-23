using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Unity.Mathematics;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private CharacterObject characterPrefab;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private Vector3 otherCharacterPosition;
    
    [SerializeField] private DialogueController dialogueController;

    [ShowInInspector] private Dictionary<CharacterDataSO, Person> test;

    private bool isWritingDialogue = false;
    private void OnEnable()
    {
        dialogueController.OnDialogueChanged += DialogueChangedHandler;
        dialogueController.OnDialogueLineChanged += LineChangedHandler;
    }
    
    // TODO: A definir: Como se inicializa la escena, como se definen las posiciones
    public void SetupScene(CharacterDataSO playerData, CharacterDataSO otherCharacterData)
    {
        var player = Instantiate(characterPrefab, playerPosition, quaternion.identity);
        player.SetData(playerData);
        
        var otherCharacter = Instantiate(characterPrefab, otherCharacterPosition, quaternion.identity);
        otherCharacter.SetData(otherCharacterData);
    }

    private void LineChangedHandler(DialogueLine newLine)
    {
        // TODO: Si es decisiva, escribir la linea en UI de una, capaz en gris? y luego de elegir la burbuja escribirla con el sonido correspondiente y tal vez un efectito
        if (!newLine.isDecisionLine)
        {
            StartCoroutine(WriteDialogueLine(newLine));
        }
        else
        {
            // WriteDecisionLine();
            StartDialogueTimer();
        }
    }
    private void DialogueChangedHandler(DialogueNodeSO newDialogue)
    {
        // TODO: Updatear la escena con info del nuevo dialogo, nueva reacción del otro personaje, etc
        // TODO: Capaz el tipo de reacción debería estar definida en cáda línea de dialogo, y también la velocidad en la que se escribe
    }

    private void StartDialogueTimer()
    {
        // Start timer, wait for player decision
    }

    
    
    private IEnumerator WriteDialogueLine(DialogueLine newLine)
    {
        isWritingDialogue = true;
        throw new NotImplementedException();
    }
}