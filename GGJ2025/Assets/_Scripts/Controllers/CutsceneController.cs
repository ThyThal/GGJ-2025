using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] public List<Image> cutsceneImages;

    public event Action OnCutsceneFinished;
    public void Play(List<SpriteDisplay> cutsceneSprites)
    {
        gameObject.SetActive(true);
        StartCoroutine(DisplaySprites(cutsceneSprites));
    }

    IEnumerator DisplaySprites(List<SpriteDisplay> cutsceneSprites)
    {
        Debug.Log("PLAY CUTSCENE");
        for (int i = 0; i < cutsceneSprites.Count; i++)
        {
            cutsceneImages[i].sprite = cutsceneSprites[i].sprite;
            cutsceneImages[i].gameObject.SetActive(true);
            LeanTween.color((RectTransform) cutsceneImages[i].transform, Color.white, cutsceneSprites[i].displayTime / 4f);
            yield return new WaitForSeconds(cutsceneSprites[i].displayTime);
        }
        
        OnCutsceneFinished?.Invoke();
    }
}