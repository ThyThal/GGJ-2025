using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgression : MonoBehaviour
{
    [SerializeField] List <CharacterDataSO> unlockedCharacters = new List<CharacterDataSO>();
    [SerializeField] List <CharacterDataSO> initialCharacters = new List<CharacterDataSO>();
    
    public CharacterDataSO SelectedCharacter {get; private set;}
    public static GameProgression Instance {get; private set;}
    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void TryUnlockCharacter(CharacterDataSO character)
    {
        if (unlockedCharacters.Contains(character))
        {
            Debug.LogError("Character is already unlocked!");
            return;
        }
        
        unlockedCharacters.Add(character);
        Debug.Log("Unlocked " + character.characterName);
    }

    public void TryLockCharacter(CharacterDataSO character)
    {
        if (!unlockedCharacters.Contains(character))
        {
            Debug.LogError("Character wasn't unlocked!");
            return;
        }
        
        unlockedCharacters.Remove(character);
        Debug.Log("Locked " + character.characterName);
    }

    public bool IsCharacterUnlocked(CharacterDataSO character)
    {
        return unlockedCharacters.Contains(character);
    }

    public bool SelectCharacter(CharacterDataSO character)
    {
        if (!unlockedCharacters.Contains(character))
        {
            Debug.LogError("Can't select " + character.characterName + ", not unlocked!");
            return false;
        }

        SelectedCharacter = character;
        return true;
    }

    public void ResetProgress()
    {
        unlockedCharacters = initialCharacters;
        BranchLoader.didIntro = false;
    }
}