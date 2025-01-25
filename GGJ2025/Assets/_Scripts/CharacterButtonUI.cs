using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButtonUI : MonoBehaviour
{
    [SerializeField] private CharacterSelection selection;
    
    [SerializeField] private CharacterDataSO characterData;
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject lockedIcon;
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        bool unlocked = GameProgression.Instance.IsCharacterUnlocked(characterData);
        _button.interactable = unlocked;
        characterImage.color = unlocked ? Color.white : Color.gray;
        lockedIcon.SetActive(!unlocked);
        
    }

    public void OnClick()
    {
        selection.CharacterSelected(characterData);
    }
}
