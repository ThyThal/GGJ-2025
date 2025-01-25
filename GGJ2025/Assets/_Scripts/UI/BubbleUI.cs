using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BubbleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    [SerializeField] private Image image;
    [SerializeField] private float showTime = .25f;
    [SerializeField] private float hideTime = .25f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Emotion CurrentEmotion;
    [SerializeField] public List<EmotionPrefab> emotionPrefabs;  // List of EmotionPrefab

    public TextMeshProUGUI TextComponent => textComponent;
    private bool isShowing;
    public float ShowTime => showTime;
    public float HideTime => hideTime;
    public bool IsShowing => isShowing;
    public void SetEmotion(Emotion emotion)
    {
        CurrentEmotion = emotion;
    }
    
    public Emotion GetEmotion()
    {
        return CurrentEmotion;
    }

    private GameObject currentPrefab;

    public void Show()
    {
        //TODO: Tween animation
        // Find the prefab corresponding to the current emotion
        var emotionPrefab = emotionPrefabs.Find(ep => ep.emotion == CurrentEmotion);

        // If a prefab exists for this emotion
        if (emotionPrefab.prefab != null)
        {
            // Deactivate the previous prefab if it exists
            if (currentPrefab != null)
            {
                currentPrefab.SetActive(false);
            }

            // Set the new prefab and activate it
            currentPrefab = emotionPrefab.prefab;
            currentPrefab.SetActive(true);
        }

        // Show the bubble
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

        // Destroy current prefab if any
        if (currentPrefab != null)
        {
            Destroy(currentPrefab);
        }
    }

    public void SetPrefab(GameObject prefab)
    {
        // If a prefab is set, instantiate it as a child of the current game object
        if (prefab != null && currentPrefab == null)
        {
            currentPrefab = Instantiate(prefab, transform); // Instantiate the prefab under this BubbleUI
        }
    }

    public bool IsPrefabSet(GameObject prefab)
    {
        return currentPrefab != null && currentPrefab == prefab;
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public bool IsSpriteSet(Sprite sprite)
    {
        return image.sprite == sprite;
    }
}

[Serializable]
public struct EmotionPrefab
{
    public Emotion emotion;
    public GameObject prefab;
}