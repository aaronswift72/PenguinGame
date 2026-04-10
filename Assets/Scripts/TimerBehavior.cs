using UnityEngine;
using TMPro;
using System;

public class TimerBehavior : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    private float elapsedTime;

    void Update() 
    {
        elapsedTime += Time.deltaTime;
        
        TimeSpan time = TimeSpan.FromSeconds(elapsedTime);
        
        timerText.text = time.ToString(@"mm\:ss\:ff");
    }
}
