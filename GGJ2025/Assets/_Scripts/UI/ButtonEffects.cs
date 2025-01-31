using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    // Start is called before the first frame update
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;
        GameManager.Instance.AudioManager.PlayHoverButton();
        LeanTween.scale(gameObject, Vector3.one * 1.1f, .25f).setEaseInOutBack();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) return;
        LeanTween.scale(gameObject, Vector3.one, .25f).setEaseInOutBack();
    }

    public void OnClick()
    {
        GameManager.Instance.AudioManager.PlayClickButton();
    }
}
