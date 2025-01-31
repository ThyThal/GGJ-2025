using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button buttonResume;
    [SerializeField] private Button buttonMainMenu;
    [SerializeField] private Button buttonHelp;
    [SerializeField] private Button buttonQuit;
    [SerializeField] private Button buttonGoBack;

    [Header("Menu")] 
    [SerializeField] private GameObject pauseButton;
    [SerializeField] public GameObject pauseButtons;
    [SerializeField] public GameObject helpMenu;
    [SerializeField] public GameObject background;
    [SerializeField] public CanvasGroup canvasGroup;
    [SerializeField] private Animator animator;
    
    //Extras
    [SerializeField] public bool pauseMenuActive;
    [SerializeField] public bool helpSubmenu;

    private void Awake()
    {
        if (GameManager.Instance.pauseMenu == null)
        {
            GameManager.Instance.pauseMenu = this;
        }
    }

    void Start()
    {
        if (GameManager.Instance.pauseMenu == null)
        {
            GameManager.Instance.pauseMenu = this;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        buttonResume.onClick.AddListener(OnClickResume);
        buttonHelp.onClick.AddListener(OnClickHelp);
        buttonQuit.onClick.AddListener(OnClickQuit);
        buttonGoBack.onClick.AddListener(OnClickGoBack);
        buttonMainMenu.onClick.AddListener(OnClickMenu);
        pauseButtons.SetActive(false);
        pauseMenuActive = true;
        helpMenu.SetActive(false);
        background.SetActive(false);
        TryEnabledPauseButton(SceneManager.GetActiveScene());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.IsPaused() && SceneManager.GetActiveScene().name != "Menu Scene")
        {
            if (pauseMenuActive)
            {
                OnClickResume();
            } else
            {
                OnClickGoBack();
            }

            Input.ResetInputAxes();
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("Enable Pause for Loaded Scene: " + scene.name);
        TryEnabledPauseButton(scene);
    }

    private void TryEnabledPauseButton(Scene scene)
    {
        if (pauseButton == null) return;
        if (helpSubmenu) return;
        
        //Debug.Log("Enable Pause for Loading Scene: " + scene);
        if (scene.name != "Menu Scene")
        {
            //Debug.Log("Enabled: " + scene);
            pauseButton?.SetActive(true);
            canvasGroup.interactable = true;
        }

        else
        {
            //Debug.Log("Disabled: " + scene);
            canvasGroup.interactable = false;
            HidePauseButton();
        }
    }
    
    public void ShowPauseMenu()
    {
        background.SetActive(true);
        pauseButtons.SetActive(true);
        pauseButton.SetActive(false);
    }
    
    public void HidePauseMenu()
    {
        background.SetActive(false);
        pauseButtons.SetActive(false);
        pauseButton.SetActive(true);
    }
    
    public void HidePauseButton()
    {
        pauseButton.SetActive(false);
    }

    public void OnClickResume() 
    {
        GameManager.Instance.PauseGame();      
    }

    public void OnClickHelp() 
    {
        if (pauseMenuActive)
        {
            pauseMenuActive = false;
            helpSubmenu = true;
            pauseButtons.SetActive(false);
            helpMenu.SetActive(true);
        }
    }

    public void OnClickMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.levelLoader.LoadScene("Menu Scene");
    }

    public void OnClickGoBack() 
    {
        if (!pauseMenuActive)
        {
            pauseMenuActive = true;
            helpSubmenu = false;
            pauseButtons.SetActive(true);
            helpMenu.SetActive(false);
        }
    }

    public void OnClickQuit()
    {
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #endif
        Application.Quit();
    }
}
