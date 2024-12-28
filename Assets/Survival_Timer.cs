using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class SurvivalTimer : MonoBehaviour
{
    private float elapsedTime = 0f; 
    [SerializeField]
    private Text timerText; 

    private bool isRunning = true; 

    void Update(){
        if (isRunning){
            elapsedTime += Time.deltaTime; 
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay(){

        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer(){
        isRunning = false; 
    }

    public float GetElapsedTime(){
        return elapsedTime; 
    }
}

