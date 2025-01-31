using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip lockSound;
    [SerializeField] private AudioClip unlockSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip hoverSound;

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

    public void PlayClickButton()
    {
        PlayClip(clickSound);
    }

    public void PlayHoverButton()
    {
        PlayClip(hoverSound);
    }
}
