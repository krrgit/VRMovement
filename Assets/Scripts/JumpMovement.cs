using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMovement : MonoBehaviour
{
    [SerializeField] private HMDInputController input;
    [SerializeField] private CharacterController cc;
    [SerializeField] private float fullJumpThreshold = 0.5f;
    [SerializeField] private float fastFallThreshold = -0.5f;
    [SerializeField] private float jumpMultiplier = 5f;
    [SerializeField] private float fastFallGravity = 18.6f;
    [SerializeField] private int maxJumps = 2;

    public Vector3 vertVelocity;
    private float curVertSign;
    private float curMaxVertVel;
    
    private bool bufferJump;
    private bool isFastFall;
    private int curJumps;


    // Update is called once per frame
    void FixedUpdate()
    {
        Jump();
        MaxDirVel();
        FastFall();
        Gravity();
        LandCheck();
    }
    
    // Get the max velocity in a given direction
    void MaxDirVel()
    {

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
        if (!cc.isGrounded && !bufferJump) return;
        
        // Check headset moves fast enough to trigger jump action
        if (input.VertVelocity > fullJumpThreshold)
        {
            bufferJump = true;
        }
        
        // Once headset velocity is slower than max, trigger jump
        if (bufferJump && Mathf.Abs(input.VertVelocity) < curMaxVertVel)
        {
            vertVelocity = curMaxVertVel * jumpMultiplier * Time.deltaTime * Vector3.up;
            print("Jump");
            bufferJump = false;
            ++curJumps;
        }
    }

    void FastFall()
    {
        if (cc.isGrounded) return;
        
        // Check if headset moves fast enough to trigger fastfall
        if (input.VertVelocity < fastFallThreshold)
        {
            isFastFall = true;
        }
    }

    void Gravity()
    {
        if (!cc.isGrounded)
            vertVelocity -= (isFastFall ? fastFallGravity :  9.8f) * Time.fixedDeltaTime * Time.fixedDeltaTime * Vector3.up;

        cc.Move(vertVelocity);
    }

    void LandCheck()
    {
        if (cc.isGrounded && vertVelocity.y < 0)
        {
            print("Land");
            vertVelocity.y = -Mathf.Epsilon;
            isFastFall = false;
            curJumps = 0;
        }
    }
}
