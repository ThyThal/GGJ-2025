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
    public TextMeshProUGUI TextComponent => textComponent;

    public void Show()
    {
        //TODO: Tween animation
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
