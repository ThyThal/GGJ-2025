using System;
using TMPro;
using UnityEngine;

public class BubbleComponents : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI textComponent;

    private void Awake()
    {
        textComponent.text = String.Empty;
    }
}
