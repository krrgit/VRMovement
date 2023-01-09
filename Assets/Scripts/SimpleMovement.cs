using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private HMDInputController input;
    [SerializeField] private CharacterController cc;
    [SerializeField] private AnimationCurve speedCurve;
    [Header("Ground")]
    [SerializeField] private float grAccel = 0.1435f;
    [SerializeField] private float grFriction = 0.8f;
    [SerializeField] private float grMaxHorzVel = 3;
    [SerializeField] private float dashThreshold = 1f;
    [Header("Air")]
    [SerializeField] private float airAccel = 0.1435f;
    [SerializeField] private float airFriction = 0.2f;
    [SerializeField] private float airMaxHorzVel = 3;
    [Header("Curent")]
    [SerializeField] private float curAccel;
    [SerializeField] private float curFriction;
    [SerializeField] private float curMaxHorzVel = 6;
    private Vector3 horzAccel;
    private Vector3 horzVel;
        
    // TODO
    // Have movement based on input direction but still have friction when stopping
    
    public void SetHorzVelocity(Vector3 vel)
    {
        horzVel = vel;
    }

    private void FixedUpdate()
    {
        SetCurrentValues();
        Move();
    }

    void SetCurrentValues()
    {
        if (cc.isGrounded)
        {
            curAccel = grAccel;
            curFriction = grFriction;
            curMaxHorzVel = grMaxHorzVel;
        }
        else
        {
            curAccel = airAccel;
            curFriction = airFriction;
            curMaxHorzVel = airMaxHorzVel;
        }

        curMaxHorzVel *= InputData.Data.GetRightTrigger();
    }

    void Move()
    {
        if (cc.isGrounded)
        {
            
            horzAccel = curAccel * InputData.Data.GetRightTrigger() * Time.fixedDeltaTime * input.HorzDirection;
            horzVel = Vector3.ClampMagnitude(horzVel, curMaxHorzVel - horzAccel.magnitude);
            horzVel += horzAccel;
            
            ClampHorzVel();
            
            // Direction Input
            if (InputData.Data.GetRightTriggerButton())
            {
                horzVel = input.HorzDirection * horzVel.magnitude;
            }
            
            // Dash
            if (horzVel.magnitude < 1 && InputData.Data.GetHMDVelocity().magnitude > dashThreshold)
            {
                horzVel = curMaxHorzVel * 1.1f * horzVel.normalized;
            }
        }
        Friction();
        
        cc.Move(horzVel * Time.fixedDeltaTime);
    }

    void ClampHorzVel()
    {
        if (horzVel.magnitude > curMaxHorzVel)
        {
            horzVel = Vector3.ClampMagnitude(horzVel, horzVel.magnitude - Mathf.Min(horzVel.magnitude - curMaxHorzVel, grFriction));
            horzAccel = Vector3.zero;
        } 
        else
        {
            horzVel = Vector3.ClampMagnitude(horzVel, curMaxHorzVel - horzAccel.magnitude);
        }
    }

    void Friction()
    {
        if (input.HorzDirection.magnitude > 0) return;

        horzVel = Vector3.ClampMagnitude(horzVel, horzVel.magnitude -
                                                  (Mathf.Min(horzVel.magnitude, curFriction) * Time.fixedDeltaTime));
    }

}
