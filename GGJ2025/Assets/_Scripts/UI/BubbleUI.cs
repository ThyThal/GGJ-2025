using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BubbleUI : MonoBehaviour
{
    [SerializeField] private float showTime = .25f;
    [SerializeField] private float hideTime = .25f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Emotion currentEmotion;
    [SerializeField] public List<EmotionPrefab> emotionPrefabs;  // List of EmotionPrefab
    
    private bool isShowing;
    public float ShowTime => showTime;
    public float HideTime => hideTime;
    public bool IsShowing => isShowing;

    public Emotion GetEmotion()
    {
        return currentEmotion;
    }

    [SerializeField] private GameObject currentPrefab;
    [SerializeField] private BubbleComponents currentBubbleComponents;

    public void SetEmotion(Emotion emotion)
    {
        currentEmotion = emotion;
    }
    
    public BubbleComponents GetCurrentBubbleComponents()
    {
        // If currentPrefab is null, find the prefab that matches the current emotion
        if (currentPrefab == null)
        {
            // Find the prefab corresponding to the current emotion
            var emotionPrefab = emotionPrefabs.Find(ep => ep.emotion == currentEmotion);

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

    public void Show(string forceText = null)
    {
        // Find the prefab corresponding to the current emotion
        var emotionPrefab = emotionPrefabs.Find(ep => ep.emotion == currentEmotion);

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
            currentBubbleComponents = GetCurrentBubbleComponents();
        }

        currentPrefab.SetActive(true);

        if (forceText != null)
        {
            Debug.LogWarning("Pre setting text: " + forceText);
            currentBubbleComponents.textComponent.text = forceText;
        }
        //gameObject.SetActive(true);
        isShowing = true;
        LeanTween.scale(currentPrefab, Vector3.one, showTime).setEaseOutElastic();
    }

    public void Hide()
    {
        if (currentPrefab == null) return;
        LeanTween.scale(currentPrefab, Vector3.zero, hideTime).setEaseInCubic().setOnComplete(TurnOff);
    }

    public void TurnOff()
    {
        // Deactivate the current emotion prefab (but keep the main BubbleUI active)
        if (currentPrefab != null)
        {
            currentPrefab.transform.localScale = Vector3.zero;
            currentPrefab.SetActive(false);
        }

        // Clear the text of the current bubble if it exists
        if (currentBubbleComponents != null && currentBubbleComponents.textComponent != null)
        {
            currentBubbleComponents.textComponent.text = string.Empty;
        }

        isShowing = false;
        currentPrefab = null;
    }
}

[Serializable]
public struct EmotionPrefab
{
    public Emotion emotion;
    public GameObject prefab;
}