using UnityEngine;
using TMPro;
using System;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText; // Text for time UI element
    [SerializeField] private TextMeshProUGUI waveText; // Text for wave status UI element
    [SerializeField] private TextMeshProUGUI resultsText;

    [SerializeField] private float waveTime; // Time for each wave. Could become an array of times if we want to change the waves
    [SerializeField] private float restTime; // Time between each wave

    [SerializeField] private PlayerHealth player; // PlayerHealth script to heal player after wave ends.

    private float remainingTime; // Time left before next event
    private float waveNum; // Wave number
    private Boolean isRest = false; // Determines whether it is time for a wave or a rest period

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        remainingTime = waveTime;
        waveNum = 1;
        SetWaveText();
    }

    // Update is called once per frame
    private void Update()
    {
        CountDown();
        SetTimerText();
    }
    private void CountDown() //Counts down timer and resets it
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime <= 0)
        {
            ChangeWaveStatus();
            if (isRest)
            {
                player.HealDamage();
                remainingTime = restTime;
            }
            else
            {
                remainingTime = waveTime;
            }
        }
    } 

    private void ChangeWaveStatus() // Changes wave status when the timer ends
    {
        if(isRest)
        {
            waveNum++;
            isRest = false;
            SetWaveText();
        }
        else
        {
            isRest = true;
            SetWaveText();
        }
    }

    private void SetTimerText() // Sets text of timer
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        //Debug.Log(timerText.text);
    }

    private void SetWaveText() // Sets text of wave status and results
    {
        if (!isRest)
        {
            waveText.text = "Wave " + waveNum; //change after ui is set up
        }
        else
        {
            waveText.text = "Next Wave Incoming";
        }
            resultsText.text = "You made it to Wave " + waveNum;
        Debug.Log(waveText.text);
    }

    public float GetWaveStatus() // Returns wave status
    {
        if (isRest)
        {
            return 0;
        }
        return waveNum;
    }
}
