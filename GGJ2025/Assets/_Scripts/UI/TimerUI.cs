using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Image fill;

    private float max;
    
    public void UpdateMax(float max)
    {
        this.max = max;
    }
    
    public void UpdateFill(float timeLeft)
    {
        fill.fillAmount = timeLeft / max;
    }
}