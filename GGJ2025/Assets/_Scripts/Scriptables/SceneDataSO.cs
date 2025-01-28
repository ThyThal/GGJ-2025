using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneDataSO", menuName = "Scriptable Objects/SceneDataSO")]
public class SceneDataSO : ScriptableObject
{
    public string sceneName = "Scene 1";
    public Sprite background;
    public Sprite bar;
    public CharacterDataSO otherCharacter;

    public AudioClip music;

    public Color color;
}

[Serializable]
public class CharacterAndPosition
{
    public Person character;
    public Vector3 position;
}
