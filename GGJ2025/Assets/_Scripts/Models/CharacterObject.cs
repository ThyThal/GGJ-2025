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
    [SerializeField] protected PersonAudio personAudio;

    [SerializeField] protected Transform dialoguePosition;
    [SerializeField] protected Transform leavePosition;
    public CharacterDataSO CurrentData => data;

    private void Awake()
    {
        if (personAudio == null ) personAudio = GetComponentInChildren<PersonAudio>();
        //if (personAudio != null) personAudio.SetCharacterData(data);
    }

    public void SetData(CharacterDataSO data, Emotion initialType = Emotion.Normal)
    {
        this.data = data;
        
        bodySpriteRenderer.sprite = data.bodySprite;
        personAudio.SetCharacterData(data);
        ChangeBody(initialType);
        //ChangeVoice(initialType);
    }

    private void ChangeBody(Emotion initialType)
    {
        bodySpriteRenderer.sprite = data.GetBody(initialType);
    }

    public void UpdateEmotion(Emotion type, bool animate = false)
    {
        ChangeBody(type);
        if(animate) Shake();
        //ChangeVoice(type);
    }
    
    public void EnterScene()
    {
        //transform.parent.position = leavePosition.position;
        Debug.Log(data.characterName + " entered scene");
        transform.parent.gameObject.SetActive(true);
        LeanTween.move(transform.parent.gameObject, dialoguePosition.position, 1.5f).setEaseOutElastic();
    }

    void Shake()
    {
        LeanTween.scale(gameObject, Vector3.one * 1.05f, 0.25f).setEaseInBack().setLoopPingPong(1);
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