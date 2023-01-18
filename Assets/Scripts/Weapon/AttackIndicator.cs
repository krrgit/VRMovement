using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicator : MonoBehaviour
{
    [SerializeField] private Transform weapon;
    [SerializeField]private Transform startPoint;
    [SerializeField]private Transform endPoint;
    [SerializeField] private LineRenderer line;

    private bool isTracking;

    public void StartTracking()
    {
        startPoint.position = weapon.position;
        endPoint.position = weapon.position;
        isTracking = true;
    }

    public void StopTracking()
    {
        isTracking = false;
    }

    private void FixedUpdate()
    {
        if (!isTracking) return;
        
        endPoint.position = weapon.position;
        line.SetPosition(0,startPoint.localPosition);
        line.SetPosition(1,endPoint.localPosition);
    }
}
