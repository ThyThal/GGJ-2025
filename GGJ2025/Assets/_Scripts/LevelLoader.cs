using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.levelLoader = this;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadLevel(sceneName));
    }
    
    private IEnumerator LoadLevel(string sceneName)
    {
        // Play Animation
        //_animator.SetTrigger("Start");
        yield return new WaitForSeconds(FadeController.Instance.FadeOut());
        GameManager.Instance.HidePause();
        SceneManager.LoadScene(sceneName);
    }
}
