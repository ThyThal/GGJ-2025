using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneDataSO", menuName = "Scriptable Objects/SceneDataSO")]
public class SceneDataSO : ScriptableObject
{
    public string sceneName = "Scene 1";
    public Sprite background;
    public CharacterDataSO otherCharacter;
    //public DialogueNodeSO initialDialogue;
}

[Serializable]
public class CharacterAndPosition
{
    public Person character;
    public Vector3 position;
}
