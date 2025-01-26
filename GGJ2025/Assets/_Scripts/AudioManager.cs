using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip lockSound;
    [SerializeField] private AudioClip unlockSound;

    public void PlayClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayUnlockCharacter()
    {
        PlayClip(unlockSound);   
    }

    public void PlayLockCharacter()
    {
        PlayClip(lockSound);
    }
}
