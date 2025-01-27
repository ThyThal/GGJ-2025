using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] public List<Image> cutsceneImages;
    [SerializeField] private AudioSource audioSource;

    public event Action OnCutsceneFinished;
    public void Play(List<SpriteDisplay> cutsceneSprites, GameEvents eventType)
    {
        gameObject.SetActive(true);
        StartCoroutine(DisplaySprites(cutsceneSprites, eventType));
    }

    IEnumerator DisplaySprites(List<SpriteDisplay> cutsceneSprites, GameEvents eventType)
    {
        Debug.Log("PLAY CUTSCENE");
        for (int i = 0; i < cutsceneSprites.Count; i++)
        {
            cutsceneImages[i].sprite = cutsceneSprites[i].sprite;
            cutsceneImages[i].gameObject.SetActive(true);
            audioSource.PlayOneShot(cutsceneSprites[i].audioClip);
            LeanTween.color((RectTransform) cutsceneImages[i].transform, Color.white, cutsceneSprites[i].displayTime / 4f);
            yield return new WaitForSeconds(cutsceneSprites[i].displayTime);
        }

        if (eventType == GameEvents.Intro)
        {
            var canvas = GetComponent<CanvasGroup>();
            LeanTween.alphaCanvas(canvas, 0, 0.25f);
            yield return new WaitForSeconds(0f);
        }
        
        OnCutsceneFinished?.Invoke();
    }
}