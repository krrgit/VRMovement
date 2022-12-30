using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 startPos;
    
    private bool moveInputActive;
    private bool prevMoveInputActive;

    private Vector3 headTiltInput;

    private Vector3 horzVel;

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

    void Move()
    {
        if (moveInputActive)
        { 
            horzVel = headTiltInput;
            horzVel.y = 0;

            transform.position += Time.deltaTime * horzVel;
        }
    }

    void GetInput()
    {
        GetLeftButton();

        if (moveInputActive)
        {
            GetHeadTilt();
        }
    }


    void GetLeftButton()
    {
        moveInputActive = InputData.Data.GetRightButton();

        if (prevMoveInputActive != moveInputActive)
        {
            startPos = InputData.Data.GetHMDPosition();
        }
        prevMoveInputActive = moveInputActive;
    }

    void GetHeadTilt()
    {
        headTiltInput = (InputData.Data.GetHMDPosition() - startPos);
    }
}
