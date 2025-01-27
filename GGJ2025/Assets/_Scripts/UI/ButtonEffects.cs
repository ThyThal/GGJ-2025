using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button _button;
    private void Awake()
    {
        _button = GetComponent<Button>();
    }
    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        GameManager.Instance.audioManager.PlayHoverButton();
        LeanTween.scale(gameObject, Vector3.one * 1.1f, .25f).setEaseInOutBack();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        LeanTween.scale(gameObject, Vector3.one, .25f).setEaseInOutBack();
    }

    public void OnClick()
    {
        GameManager.Instance.audioManager.PlayClickButton();
    }
}
