using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private MotionbodySO so;

    private float curAccel;
    private Vector3 horzAccel;
    private Vector3 horzVel;
    private float curMaxHorzVel;
    
    
    // Inputs
    private Vector3 startPos;
    private bool moveInputActive;
    private bool prevMoveInputActive;
    private Vector3 direction;
    private Vector3 horzDirection;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
    }

    void CalcHorzAccel()
    {
        curMaxHorzVel = so.grMaxHorzVel;
        
        if (horzDirection.magnitude > 0)
        {
            curAccel = so.runAccel;
        }
        else
        {
            curAccel = 0;
        }

        horzAccel = curAccel * horzDirection;
    }

    void CalcHorzVel()
    {
        horzVel += horzAccel;
        horzVel = Vector3.ClampMagnitude(horzVel, curMaxHorzVel);
    }

    void CalcFriction()
    {
        if (horzDirection.magnitude == 0)
        {
            if (horzVel.magnitude > so.grFriction)
            {
                horzVel -= so.grFriction * horzVel.normalized;
            }
            else
            {
                horzVel = Vector3.zero;
            }
        }

    }

    void Move()
    {
        CalcHorzAccel();
        CalcHorzVel();
        CalcFriction();
        rb.position += (Time.fixedDeltaTime) * horzVel;
    }

    void GetInput()
    {
        GetRightButton();
        GetHeadTilt();
    }


    void GetRightButton()
    {
        moveInputActive = InputData.Data.GetRightButton();

        if (prevMoveInputActive != moveInputActive)
        {
            print("button pressed");
            startPos = InputData.Data.GetHMDPosition();
        }
        prevMoveInputActive = moveInputActive;
    }

    void GetHeadTilt()
    {
        if (moveInputActive)
        {
            direction = Vector3.ClampMagnitude(InputData.Data.GetHMDPosition() - startPos,1.0f);
            horzDirection = direction;
            horzDirection.y = 0;
        } else 
        {
            direction = horzDirection = Vector3.zero;
        }
    }
}
