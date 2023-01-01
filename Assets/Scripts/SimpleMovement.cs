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
    [SerializeField] private float speed = 3;
    [SerializeField] private float fullJumpThreshold = 0.5f;
    [SerializeField] private float fastFallThreshold = -0.5f;
    [SerializeField] private float fullJumpInitVel = 4.0f;
    [SerializeField] private float jumpMultiplier = 5f;
    [SerializeField] private float fastFallGravity = 18.6f;
    [SerializeField] private int maxJumps = 2;

    public Vector3 vertVelocity;
    private float curVertSign;
    private float curMaxVertVel;
    
    private bool waitJump;
    private bool isFastFall;

    private int curJumps;

    private bool nextJumpReady;

    private Vector3 airHorzVel;
    
    private void Update()
    {
        MaxDirVel();
    }

    private void FixedUpdate()
    {
        Jump();
        FastFall();
        Gravity();
        Move();
        LandCheck();
    }

    void Move()
    {
        Vector3 value  = vertVelocity;
        if (cc.isGrounded)
        {
            value += Time.deltaTime * speed * speedCurve.Evaluate(input.HorzDirection.magnitude) * input.HorzDirection;
        }
        else
        {
            print("Air movement");
            value += Time.deltaTime * speed * (airHorzVel + input.HorzDirection);
        }

        cc.Move(value);
    }

    void MaxDirVel()
    {
        //curVertSign = input.VertVelocity > 0 ? 1 : input.VertVelocity < 0 ? -1 : curVertSign;
        
        if ((input.VertVelocity > 0 && curVertSign > 0) || (input.VertVelocity < 0 && curVertSign < 0))
        {
            curMaxVertVel = Mathf.Max(Mathf.Abs(input.VertVelocity), curMaxVertVel);
        }
        else
        {
            // Switch Direction
            curMaxVertVel = 0;
            curVertSign = Mathf.Sign(input.VertVelocity);
        }
    }

    void Jump()
    {
        if (curJumps == maxJumps) return;
        
        if (curVertSign < 0)
        {
            nextJumpReady = true;
        }

        if (!nextJumpReady && !cc.isGrounded) return;
        
        // Check we pass threshold for jump
        // If we do, wait until we slow down before we actually do jump 
        if (input.VertVelocity > fullJumpThreshold)
        {
            waitJump = true;
            airHorzVel += input.HMDVelocity;
            airHorzVel.y = 0;
        }

        if (waitJump && Mathf.Abs(input.VertVelocity) < curMaxVertVel)
        {
            vertVelocity = curMaxVertVel * jumpMultiplier * Time.deltaTime * Vector3.up;
            airHorzVel = transform.TransformVector(airHorzVel);
            print("Jump");
            waitJump = false;
            curMaxVertVel = 0;
            ++curJumps;
            nextJumpReady = false;
        }
    }

    void FastFall()
    {
        if (cc.isGrounded) return;
        
        // Check we pass threshold for jump
        // If we do, wait until we slow down before we actually do jump 
        if (input.VertVelocity < fastFallThreshold)
        {
            isFastFall = true;
        }
    }

    void Gravity()
    {
        if (!cc.isGrounded)
            vertVelocity += (isFastFall ? fastFallGravity :  9.8f) * Time.fixedDeltaTime * Time.fixedDeltaTime * Vector3.down;

        //cc.Move(vertVelocity);
    }

    void LandCheck()
    {
        if (cc.isGrounded && vertVelocity.y < 0)
        {
            vertVelocity = Vector3.zero;
            airHorzVel = Vector3.zero;
            isFastFall = false;
            curJumps = 0;
            nextJumpReady = false;
            
        }
    }
    
}
