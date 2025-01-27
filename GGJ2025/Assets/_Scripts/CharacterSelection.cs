using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSelection : MonoBehaviour
{
    bool isSelected = false;

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
        else
        {
            
        }
    }

    public IEnumerator LoadScene(int sceneIndex)
    {
        yield return new WaitForSeconds(FadeController.Instance.FadeOut());
        SceneManager.LoadScene(sceneIndex);
    }
}
