using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    private bool isSelected = false;

    private void Start()
    {
        FadeController.Instance.FadeIn();
    }

    public void CharacterSelected(CharacterDataSO character)
    {
        if (isSelected) return;
        
        if(GameProgression.Instance.SelectCharacter(character))
        {
            StartCoroutine(LoadScene(2)); // TODO: Cambiar a otro index/string/lo que sea
            isSelected = true;
        }
    }

    private IEnumerator LoadScene(int sceneIndex)
    {
        yield return new WaitForSeconds(FadeController.Instance.FadeOut());
        SceneManager.LoadScene(sceneIndex);
    }
}
