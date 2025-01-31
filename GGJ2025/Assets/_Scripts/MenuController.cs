using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class MenuController : MonoBehaviour
{
    [SerializeField] private LevelLoader levelLoader;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource pressSound = null;
    [SerializeField] private AudioClip soundIn = null;
    [SerializeField] private AudioClip soundOut = null;

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
        GameManager.Instance.isIngame = true; 
        GameManager.Instance.SetPause(false);
        GameManager.Instance.levelLoader.LoadScene(GameManager.Instance.gameScene);
        
    }
    public void OnClickCredits() 
    {
        PlaySound(soundIn);

        animator.SetTrigger("ShowCredits"); 
    }
    public void OnClickBack() 
    {
        PlaySound(soundOut);
        
        animator.SetTrigger("HideCredits"); 
    }
    public void OnClickHelp() 
    {
;       
    }

    public void OnClickExit() 
    {
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
