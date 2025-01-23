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

    public void SetData(CharacterDataSO data, BubbleType initialType = BubbleType.Normal)
    {
        this.data = data;
        
        bodySpriteRenderer.sprite = data.bodySprite;
        faceSpriteRenderer.sprite = data.GetFace(initialType);
        ChangeVoice(initialType);
    }

    public void ChangeVoice(BubbleType type = BubbleType.Normal)
    {
        currentVoice = data.GetVoice(type);
    }

    public void ChangeFace(BubbleType type = BubbleType.Normal)
    {
        faceSpriteRenderer.sprite = data.GetFace(type);
    }
}