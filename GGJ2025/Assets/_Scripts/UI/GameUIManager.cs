using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button menuButton;
    [SerializeField] SpecialEvents events;
    [SerializeField] private float buttonHidePositionY = -6;

    private float initialHeight;

    private void OnEnable()
    {
        events.OnEventEnded += HandleEventEnd;
    }

    private void HandleEventEnd(GameEvents obj)
    {
        if (obj == GameEvents.Intro || obj == GameEvents.None) return;
        
        ShowMenuButton(true);
    }

    private void Start()
    {
        initialHeight = nextButton.transform.position.y;
    }

    public void ShowNextButton(bool show)
    {
        if (show)
        {
            nextButton.gameObject.transform.position = new Vector3(nextButton.gameObject.transform.position.x, buttonHidePositionY, nextButton.gameObject.transform.position.z);
            nextButton.gameObject.SetActive(true);
            LeanTween.moveY(nextButton.gameObject, initialHeight, 0.2f).setEaseOutBack();
        }
        else
        {
            LeanTween.moveY(nextButton.gameObject, buttonHidePositionY, 0.2f).setEaseInQuad().setOnComplete(() => nextButton.gameObject.SetActive(false));
        }
    }

    public void ShowBackButton(bool show)
    {
        if (show)
        {
            backButton.gameObject.transform.position = new Vector3(backButton.gameObject.transform.position.x, buttonHidePositionY, backButton.gameObject.transform.position.z);
            backButton.gameObject.SetActive(true);
            LeanTween.moveY(backButton.gameObject, initialHeight, 0.2f).setEaseOutBack();
        }
        else
        {
            LeanTween.moveY(backButton.gameObject, buttonHidePositionY, 0.2f).setEaseInQuad().setOnComplete(() => backButton.gameObject.SetActive(false));
        }
    }

    public void ShowMenuButton(bool show)
    {
        if (show)
        {
            menuButton.gameObject.transform.position = new Vector3(backButton.gameObject.transform.position.x, buttonHidePositionY, backButton.gameObject.transform.position.z);
            menuButton.gameObject.SetActive(true);
            LeanTween.moveY(menuButton.gameObject, initialHeight, 0.2f).setEaseOutBack();
        }
        else
        {
            LeanTween.moveY(menuButton.gameObject, buttonHidePositionY, 0.2f).setEaseInQuad().setOnComplete(() => backButton.gameObject.SetActive(false));
        }
    }
    
}
