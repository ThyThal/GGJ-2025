using UnityEngine;
using UnityEngine.UI;

public class CharacterButtonUI : MonoBehaviour
{
    [SerializeField] private CharacterSelection selection;
    [SerializeField] private CharacterDataSO characterData;
    [SerializeField] private Image characterImage;
    private Image buttonImage;

    private readonly Color darkenColor = new Color(0.1686275f, 0.1686275f, 0.1686275f);
    private Button button;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
        button = GetComponent<Button>();
    }
    
    private void Start()
    {
        bool unlocked = GameProgression.Instance.IsCharacterUnlocked(characterData);
        if(!unlocked) DisableButton();
    }
    
    void DisableButton()
    {
        button.interactable = false;
        buttonImage.color = darkenColor;
        characterImage.color = darkenColor;
    }

    public void OnClick()
    {
        selection.CharacterSelected(characterData);
    }
}
