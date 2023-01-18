using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private float resetWait = 10;
    [SerializeField] private int totalTargets = 7;
    [SerializeField] private TimerDisplay timer;
    public static TargetManager Instance;

    private int targetCount = 0;

    private float timeoutTimer = 0;
    

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ResetTimer(GameObject target)
    {
        StartCoroutine(Wait(target));
    }
    
    
    IEnumerator Wait(GameObject target)
    {
        // Timeout
        timeoutTimer = timer.timeoutDur;
        
        // Timer
        timer.Tick(targetCount + 1 >= totalTargets);
        targetCount = targetCount + 1 >= totalTargets ? 0 : targetCount + 1;
        
        // Reset Target
        target.SetActive(false);
        SFXManager.Instance.HitTarget();
        yield return new WaitForSeconds(resetWait);
        target.SetActive(true);
    
    }


    void Update()
    {
        ResetTimeoutTimer();
    }

    void ResetTimeoutTimer()
    {
        if (timeoutTimer > 0)
        {
            timeoutTimer -= Time.deltaTime;
        }
        else if (targetCount != 0)
        {
            timer.Timeout();
            targetCount = 0;
        }
    }

}
