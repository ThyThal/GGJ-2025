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

    protected AudioClip currentVoice;

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
    
    public bool IsActive => gameObject.activeInHierarchy;
}