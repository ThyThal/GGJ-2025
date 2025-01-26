using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] public List<Image> cutsceneImages;
    
    public void Play(List<SpriteDisplay> cutsceneSprites)
    {
        for (int i = 0; i < cutsceneSprites.Count; i++)
        {
            
        }
    }
}