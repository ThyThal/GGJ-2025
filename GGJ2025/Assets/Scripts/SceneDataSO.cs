using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneDataSO", menuName = "Scriptable Objects/SceneDataSO")]
public class SceneDataSO : ScriptableObject
{
    [SerializeField] private string sceneName = "Scene 1";
    [SerializeField] List<CharacterAndPosition> characters = new List<CharacterAndPosition>();
    [SerializeField] private DialogueNodeSO initialDialogue;
    
    [ShowInInspector] private Dictionary<CharacterDataSO, Person> test;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class CharacterAndPosition
{
    public Person character;
    public Vector3 position;
}
