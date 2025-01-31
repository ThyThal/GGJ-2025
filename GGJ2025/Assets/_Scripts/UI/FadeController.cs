using UnityEngine;

public class FadeController : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private CanvasGroup canvasGroup;
    public static FadeController Instance {get; private set;}

    private bool IsFaded => canvasGroup.alpha > 0;
    
    private void Awake()
    {
        if(Instance != null && Instance != this) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        
    }

    public float FadeIn()
    {
        canvasGroup.alpha = 1;
        LeanTween.alphaCanvas(canvasGroup, 0, fadeSpeed).setEaseInCubic();
        return fadeSpeed;
    }
    
    public float TryFadeIn()
    {
        if (IsFaded)
        {
            LeanTween.alphaCanvas(canvasGroup, 0, fadeSpeed).setEaseInCubic();
            return fadeSpeed;
        }
        else return 0;
    }

    public float FadeOut()
    {
        canvasGroup.alpha = 0;
        LeanTween.alphaCanvas(canvasGroup, 1, fadeSpeed).setEaseOutCubic();
        return fadeSpeed;
    }

    public void FadeIn(float time)
    {
        canvasGroup.alpha = 1;
        LeanTween.alphaCanvas(canvasGroup, 0, time);
    }

    public void FadeOut(float time)
    {
        canvasGroup.alpha = 0;
        LeanTween.alphaCanvas(canvasGroup, 1, time);
    }
}
