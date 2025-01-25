using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterObject : MonoBehaviour
{
    [SerializeField] protected CharacterDataSO data;
    [SerializeField] protected SpriteRenderer bodySpriteRenderer;
    [SerializeField] protected SpriteRenderer faceSpriteRenderer;
    [SerializeField] protected AudioSource audioSource;

    [SerializeField] protected Transform dialoguePosition;
    [SerializeField] protected Transform leavePosition;

    protected AudioClip currentVoice;
    public CharacterDataSO CurrentData => data;

    public void SetData(CharacterDataSO data, Emotion initialType = Emotion.Normal)
    {
        this.data = data;
        
        bodySpriteRenderer.sprite = data.bodySprite;
        ChangeBody(initialType);
        //ChangeFace(initialType);
        //ChangeVoice(initialType);
    }

    private void ChangeBody(Emotion initialType)
    {
        bodySpriteRenderer.sprite = data.GetBody(initialType);
    }

    public void UpdateEmotion(Emotion type)
    {
        ChangeBody(type);
        //ChangeVoice(type);
        //ChangeFace(type);
    }
    public void ChangeVoice(Emotion type = Emotion.Normal)
    {
        currentVoice = data.GetVoice(type);
    }

    public void ChangeFace(Emotion type = Emotion.Normal)
    {
        faceSpriteRenderer.sprite = data.GetFace(type);
    }

    public void EnterScene()
    {
        //transform.parent.position = leavePosition.position;
        Debug.Log(data.characterName + " entered scene");
        transform.parent.gameObject.SetActive(true);
        LeanTween.move(transform.parent.gameObject, dialoguePosition.position, 1.5f).setEaseOutElastic();
    }
    
    public void LeaveScene()
    {
        //transform.parent.position = dialoguePosition.position;
        LeanTween.move(transform.parent.gameObject, leavePosition.position, 1.5f).setEaseOutElastic()
            .setOnComplete(() => transform.parent.gameObject.SetActive(false));
    }

    public void ForceLeaveScene()
    {
        transform.parent.position = leavePosition.position;
        transform.parent.gameObject.SetActive(false);
    }
    
    public bool IsActive => gameObject.activeInHierarchy;
}