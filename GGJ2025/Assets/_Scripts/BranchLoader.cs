using System;
using System.Linq;
using UnityEngine;

public class BranchLoader : MonoBehaviour
{
    [SerializeField] CharacterBranch[] branches;
    [SerializeField] DialogueController dialogueController;
    [SerializeField] SpecialEvents specialEvents;
    
    public static bool didIntro = false;

    private void OnEnable()
    {
        specialEvents.OnEventEnded += HandleEventEnded;
    }

    private void HandleEventEnded(GameEvents obj)
    {
        if (obj == GameEvents.Intro)
        {
            // FINISHED INTRO, load character selection
            CharacterBranch branchToStart = branches.FirstOrDefault();
        
            if(branchToStart != null) dialogueController.StartDialogues(branchToStart.initialNode);
            else
            {
                Debug.LogError("Could not find character branch to load.");
                return;
            }
        }
    }

    void Start()
    {
        if (!didIntro)
        {
            FadeController.Instance.TryFadeIn();
            specialEvents.PlayEvent(GameEvents.Intro);
            didIntro = true;
            return;
        }
        
        if (!GameProgression.Instance) return;
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
