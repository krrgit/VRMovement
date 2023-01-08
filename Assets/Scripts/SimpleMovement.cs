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

        curMaxHorzVel *= InputData.Data.getRightTrigger();
    }

    void Move()
    {
        if (cc.isGrounded)
        {
            horzAccel = curAccel * InputData.Data.getRightTrigger() * Time.fixedDeltaTime * input.HorzDirection;
            horzVel = Vector3.ClampMagnitude(horzVel, curMaxHorzVel - horzAccel.magnitude);
            horzVel += horzAccel;
            
            ClampHorzVel();
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

        horzVel = Vector3.ClampMagnitude(horzVel, horzVel.magnitude - Mathf.Min(horzVel.magnitude, curFriction));

    }

}
