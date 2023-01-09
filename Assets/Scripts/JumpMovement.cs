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
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private int maxJumps = 2;
    
    // Vertical Movement Values
    public Vector3 vertVelocity;
    private float curVertSign;
    private float curMaxVertVel;
    
    // Jump Values
    private bool bufferJump;
    private bool isFastFall;
    private int curJumps;


    public bool UseGravity; // Only to be used by outside components


    public void ToggleGravity(bool state)
    {
        UseGravity = state;
        vertVelocity = Vector3.zero;
    }

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
        //if (!cc.isGrounded && !bufferJump) return;
        if (curJumps == maxJumps) return;
        
        // Check headset moves fast enough to trigger jump action
        if (!bufferJump && input.VertVelocity > fullJumpThreshold)
        {
            bufferJump = true;
            //print("buffer");
        }
        
        if (bufferJump)
        {
            // Every frame we move faster than the previous, add velocity
            if (Mathf.Abs(input.VertVelocity) >= curMaxVertVel)
            {
                vertVelocity += (Mathf.Abs(input.VertVelocity) + (gravity * Time.fixedDeltaTime))* jumpMultiplier * Time.fixedDeltaTime * Vector3.up;
                //print("Jumping");
            } // When we stop increasing velocity, stop adding
            else
            {
                //print("buffer stop");
                bufferJump = false;
                ++curJumps;
            }
            
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
        if (!cc.isGrounded && UseGravity)
        {
            vertVelocity -= (isFastFall ? fastFallGravity :  gravity)  * Time.fixedDeltaTime * Time.fixedDeltaTime * Vector3.up;
            cc.Move(vertVelocity);
        }
            
    }

    void LandCheck()
    {
        if (cc.isGrounded && vertVelocity.y < 0)
        {
            vertVelocity = Vector3.zero;
            curJumps = 0;
            isFastFall = false;
        }
        //print(cc.isGrounded ? "GROUNDED" : "NOT GROUNDED");
    }
}
