using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BranchLoader : MonoBehaviour
{
    [SerializeField] CharacterBranch[] branches;
    [SerializeField] DialogueController dialogueController;
    void Start()
    {
        LoadBranch(GameProgression.Instance.SelectedCharacter);
    }

    public void LoadBranch(CharacterDataSO characterData)
    {
        CharacterBranch branchToStart = branches.FirstOrDefault(b => b.characterData == characterData);
        
        if(branchToStart != null) dialogueController.StartDialogues(branchToStart.initialNode);
        else
        {
            Debug.LogError("Could not find character branch to load.");
            return;
        }
    }
}

[Serializable]
public class CharacterBranch
{
    public CharacterDataSO characterData;
    public DialogueNodeSO initialNode;
}
