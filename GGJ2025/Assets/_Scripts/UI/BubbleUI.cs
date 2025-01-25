using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BubbleUI : MonoBehaviour
{
    [SerializeField] private float showTime = .25f;
    [SerializeField] private float hideTime = .25f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Emotion CurrentEmotion;
    [SerializeField] public List<EmotionPrefab> emotionPrefabs;  // List of EmotionPrefab
    
    private bool isShowing;
    public float ShowTime => showTime;
    public float HideTime => hideTime;
    public bool IsShowing => isShowing;

    public Emotion GetEmotion()
    {
        return CurrentEmotion;
    }

    private GameObject currentPrefab;
    private BubbleComponents currentBubbleComponents;

    public void SetEmotion(Emotion emotion)
    {
        CurrentEmotion = emotion;
    }
    
    public BubbleComponents GetCurrentBubbleComponents()
    {
        // If currentPrefab is null, find the prefab that matches the current emotion
        if (currentPrefab == null)
        {
            // Find the prefab corresponding to the current emotion
            var emotionPrefab = emotionPrefabs.Find(ep => ep.emotion == CurrentEmotion);

            // Check if the prefab inside EmotionPrefab is not null
            if (emotionPrefab.prefab != null)
            {
                // Set the prefab to the matching one and activate it
                currentPrefab = emotionPrefab.prefab;
                currentPrefab.SetActive(true);
            }
        }

        // Now get the BubbleComponents from the currentPrefab
        if (currentPrefab != null)
        {
            currentBubbleComponents = currentPrefab.GetComponentInChildren<BubbleComponents>();
        }

        return currentBubbleComponents;
    }

    public void Show()
    {
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
        // Deactivate the current emotion prefab (but keep the main BubbleUI active)
        if (currentPrefab != null)
        {
            currentPrefab.SetActive(false);
        }

        // Clear the text of the current bubble if it exists
        if (currentBubbleComponents != null && currentBubbleComponents.textComponent != null)
        {
            currentBubbleComponents.textComponent.text = string.Empty;
        }

        isShowing = false;
        transform.localScale = Vector3.zero;

        // Destroy current prefab if any
        if (currentPrefab != null)
        {
            Destroy(currentPrefab);
        }
    }

    public bool IsPrefabSet(GameObject prefab)
    {
        return currentPrefab != null && currentPrefab == prefab;
    }

    public void SetPrefab(GameObject prefab)
    {
        // If a prefab is set, instantiate it as a child of the current game object
        if (prefab != null && currentPrefab == null)
        {
            currentPrefab = prefab;
            currentPrefab.SetActive(false); // Ensure it starts inactive
        }
    }
}

[Serializable]
public struct EmotionPrefab
{
    public Emotion emotion;
    public GameObject prefab;
}