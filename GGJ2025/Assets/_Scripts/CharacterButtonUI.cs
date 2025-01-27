using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterButtonUI : MonoBehaviour
{
    [SerializeField] private CharacterSelection selection;
    [SerializeField] private CharacterDataSO characterData;
    [SerializeField] private Image characterImage;
    private Image _buttonImage;

    public readonly Color darkenColor = new Color(0.1686275f, 0.1686275f, 0.1686275f);
    private Button _button;
    private void Awake()
    {
        _buttonImage = GetComponent<Image>();
        _button = GetComponent<Button>();
    }
    
    private void Start()
    {
        bool unlocked = GameProgression.Instance.IsCharacterUnlocked(characterData);
        if(!unlocked) DisableButton();
    }
    
    void DisableButton()
    {
        _button.interactable = false;
        _buttonImage.color = darkenColor;
        characterImage.color = darkenColor;
    }

    public void OnClick()
    {
        selection.CharacterSelected(characterData);
    }
}
