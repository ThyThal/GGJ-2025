using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Header("Scene Names")]
    [SerializeField] public string menuScene = "Main Menu";
    [SerializeField] public string gameScene = "Main Game";
    [SerializeField] public LevelLoader levelLoader;
    [SerializeField] public PauseMenuController pauseMenu;
    [SerializeField] private AudioManager audioManager;
    public AudioManager AudioManager => audioManager;

    private AudioSource audioSrc;

    public static GameManager Instance;

    public bool hasWon = false;
    public bool isIngame = false;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Menu Scene")
        {
            PauseGame();
        }
    
    }
    
    public void PauseGame ()
    {
        if (!isPaused)
        {
            ShowPauseMenu();
            isPaused = true;
            Input.ResetInputAxes();
            Time.timeScale = 0;
        }

        else
        {
            Time.timeScale = 1;
            isPaused = false;
            HidePauseMenu();
        }

    }
    public bool IsPaused()
    {
        return isPaused;
    }

    private void ShowPauseMenu()
    {
        pauseMenu.ShowPauseMenu();
    }

    public void HidePauseMenu()
    {
        pauseMenu.HidePauseMenu();
    }

    public void SetPause(bool value)
    {

        isPaused = value;
    }


    public void LoadMenu()
    {
        levelLoader.LoadScene(menuScene);
    }
}
