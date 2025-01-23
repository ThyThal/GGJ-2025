using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterObject : MonoBehaviour
{
    [SerializeField] protected CharacterDataSO data;
    [SerializeField] protected SpriteRenderer bodySpriteRenderer;
    [SerializeField] protected SpriteRenderer faceSpriteRenderer;
    [SerializeField] protected AudioClip currentVoice;

    public void SetData(CharacterDataSO data, BubbleType initialType = BubbleType.Normal)
    {
        this.data = data;
        
        bodySpriteRenderer.sprite = data.bodySprite;
        faceSpriteRenderer.sprite = data.GetFace(initialType);
        currentVoice = data.GetVoice(initialType);
    }
}