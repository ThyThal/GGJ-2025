using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class CharacterSelection : MonoBehaviour
{
    bool isSelected = false;

    public void CharacterSelected(CharacterDataSO character)
    {
        if (isSelected) return;
        
        if(GameProgression.Instance.SelectCharacter(character))
        {
            SceneManager.LoadScene(2); // TODO: Cambiar a otro index/string/lo que sea
            isSelected = true;
        }
        else
        {
            
        }
    }
}
