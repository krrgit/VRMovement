using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.ProBuilder.MeshOperations;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;
    [SerializeField] private float timer;
    public float timeoutDur = 15;
    public bool isTracking;
    
    
    public void Tick(bool stop)
    {
        print("tick");
        // Start Timer
        if (!isTracking)
        {
            isTracking = true;
            timer = 0;
        }
        else
        {
            //Stop Timer Check
            if (stop)
            {
                Stop();
            }
        }
    }


    void Update()
    {
        if (!isTracking) return;

        timer += Time.deltaTime;
        Display();
    }

    void Display()
    {
        int min = Mathf.FloorToInt(timer / 60);
        var seconds = timer % 60f;
        display.SetText(min + " : " + seconds.ToString("F2"));
    }

    void Reset()
    {
        timer = 0;
    }

    public void Stop()
    {
        isTracking = false;
    }

    public void Timeout()
    {
        timer = 0;
        Display();
        Stop();
    }
}
