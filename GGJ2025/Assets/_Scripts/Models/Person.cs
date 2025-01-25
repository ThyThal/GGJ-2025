using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : MonoBehaviour
{
    [SerializeField] CharacterDataSO characterDataSO;

    private void Awake()
    {
        GetComponentInChildren<CharacterObject>()?.SetData(characterDataSO);
    }
}
