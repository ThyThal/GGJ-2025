using UnityEngine;
using UnityEngine.UI;

public class DecisionUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private DecisionsUIManager manager;
    private Emotion emotion;

    public void OnClick()
    {
        manager.PlayerDecided(emotion);
    }
    
    public void Setup(Sprite sprite, Emotion value)
    {
        image.sprite = sprite;
        emotion = value;
    }
}
