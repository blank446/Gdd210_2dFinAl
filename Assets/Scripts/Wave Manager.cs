using UnityEngine;
using TMPro;
using System;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; // Text for time UI element
    [SerializeField] private TextMeshProUGUI waveText; // Text for wave status UI element

    [SerializeField] private float waveTime; // Time for each wave. Could become an array of times if we want to change the waves
    [SerializeField] private float restTime; // Time between each wave

    private float remainingTime; // Time left before next event
    private float waveNum; // Wave number
    private Boolean isRest = false; // Determines whether it is time for a wave or a rest period

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 
        }

            SetTimerText();
    }

    void ChangeWaveStatus() // Changes wave status when the timer ends
    {

        SetWaveText();
    }

    void SetTimerText() // Sets text of timer
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void SetWaveText() // Sets text of wave status
    {

    }

    float GetWaveNum() // Returns waveNum
    {
        return waveNum;
    }
}
