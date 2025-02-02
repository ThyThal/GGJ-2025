﻿using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource pressSound = null;
    [SerializeField] private AudioClip soundIn = null;
    [SerializeField] private AudioClip soundOut = null;
    [SerializeField] private Button buttonPlay;
    [SerializeField] private Button buttonCredits;
    [SerializeField] private Button buttonExit;

    private void Awake()
    {
        pressSound = GameManager.Instance.GetComponent<AudioSource>();
    }

    private void Start()
    {
        FadeController.Instance.TryFadeIn();
        GameProgression.Instance.ResetProgress();
    }
    
    public void OnClickPlay() 
    {
        PlaySound(soundIn);
        buttonCredits.interactable = false;
        buttonPlay.interactable = false;
        buttonExit.interactable = false;
        GameManager.Instance.isIngame = true; 
        GameManager.Instance.SetPause(false);
        GameManager.Instance.levelLoader.LoadScene(GameManager.Instance.gameScene);
        
    }
    public void OnClickCredits() 
    {
        PlaySound(soundIn);
        buttonCredits.interactable = false;
        buttonPlay.interactable = false;
        buttonExit.interactable = false;
        animator.SetTrigger("ShowCredits"); 
    }
    public void OnClickBack() 
    {
        PlaySound(soundOut);
        buttonCredits.interactable = true;
        buttonPlay.interactable = true;
        buttonExit.interactable = true;
        animator.SetTrigger("HideCredits"); 
    }
    public void OnClickHelp() 
    {
;       
    }

    public void OnClickExit() 
    {
        buttonCredits.interactable = false;
        buttonPlay.interactable = false;
        buttonExit.interactable = false;
        
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
        Application.Quit();
    }

    private void PlaySound(AudioClip soundToPlay)
    {
        pressSound.clip = soundToPlay;

        pressSound.Play(); 
    }
}
