using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BubbleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Image image;
    [SerializeField] private float showTime = .25f;
    [SerializeField] private float hideTime = .25f;
    public TextMeshProUGUI TextComponent => textComponent;
    private bool isShowing;
    public float ShowTime => showTime;
    public float HideTime => hideTime;
    
    public bool IsShowing => isShowing;

    public void Show()
    {
        //TODO: Tween animation
        gameObject.SetActive(true);
        isShowing = true;
        LeanTween.scale(gameObject, Vector3.one, showTime).setEaseOutElastic();
    }

    public void Hide()
    {
        LeanTween.scale(gameObject, Vector3.zero, hideTime).setEaseInCubic().setOnComplete(TurnOff);
    }
    
    public void TurnOff()
    {
        isShowing = false;
        gameObject.SetActive(false);
        textComponent.text = string.Empty;
        transform.localScale = Vector3.zero;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
    
    public bool IsSpriteSet(Sprite sprite) => image.sprite == sprite;
}
