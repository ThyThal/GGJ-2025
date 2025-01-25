using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] float fadeSpeed = 1f;
    [SerializeField] CanvasGroup canvasGroup;
    public static FadeController Instance {get; private set;}

    public bool IsFaded => canvasGroup.alpha > 0;
    
    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        
    }

    public float FadeIn()
    {
        canvasGroup.alpha = 1;
        LeanTween.alphaCanvas(canvasGroup, 0, fadeSpeed);
        return fadeSpeed;
    }
    
    public float TryFadeIn()
    {
        if (IsFaded)
        {
            LeanTween.alphaCanvas(canvasGroup, 0, fadeSpeed);
            return fadeSpeed;
        }
        else return 0;
    }

    public float FadeOut()
    {
        canvasGroup.alpha = 0;
        LeanTween.alphaCanvas(canvasGroup, 1, fadeSpeed);
        return fadeSpeed;
    }

    public void FadeIn(float time)
    {
        canvasGroup.alpha = 1;
        LeanTween.alphaCanvas(canvasGroup, 0, time);
    }

    public void FadeOut(float time)
    {
        canvasGroup.alpha = 0;
        LeanTween.alphaCanvas(canvasGroup, 1, time);
    }
}
