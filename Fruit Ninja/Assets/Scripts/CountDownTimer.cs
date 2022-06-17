using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public float startingTime = 90f;
    public float currentTime = 0f;
    public string min_str {get; private set;}
    public string sec_str {get; private set;}
    public string centisec_str {get; private set;}
    
    int min;
    int sec;
    int centisec;

    bool running;

    private void Start() {
        running = true;
        currentTime = startingTime;
    }
    
    private void Update() {
        if (currentTime > 0 && running) {
            currentTime -= 1 * Time.deltaTime;
            min = (int)(currentTime / 60);
            sec = (int)(currentTime % 60);
            centisec = (int)((currentTime - (int)currentTime) * 100);
        }
        if (currentTime < 0) {
            min = 0;
            sec = 0;
            centisec = 0;
        }
        
        if (min < 10) {
            min_str = "0" + min.ToString();
        }
        else {
            min_str = min.ToString();
        }
        if (sec < 10) {
            sec_str = "0" + sec.ToString();
        }
        else {
            sec_str = sec.ToString();
        }
        if (centisec < 10) {
            centisec_str = "0" + centisec.ToString();
        }
        else {
            centisec_str = centisec.ToString();
        }
    }

    public void Stop() {
        running = false;
    }
}