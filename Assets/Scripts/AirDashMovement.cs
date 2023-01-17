using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AirDashState
{
    Idle,
    Buffer,
    Accel,
    SlowDown
}
public class AirDashMovement : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private JumpMovement jm;
    [SerializeField] private SimpleMovement sm;

    [SerializeField] private AirDashState state;
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float threshold = 5;
    [SerializeField] float airFriction = 0.04f;
    [SerializeField] private bool isAirdashing;
    [SerializeField] private Vector3 velocity;

    [SerializeField] private float triggerRadius = 0.1f;
    
    private bool isAccelerating;

    private bool startPosSet;
    private Vector3 handStartPos;
    private Vector3 direction;
    
    
    // TODO:
    // Have direction based on vector from starting hand position when button is initially pressed to end position
    // Still use hand velocity as speed
    // Fix bug where releasing button doesn't carry over the velocity to the simple movement component
    
    // Update is called once per frame
    void Update()
    {
        Check();
        StopConditions();
        Move();
    }
    

    void Check()
    {
        if (!InputData.Data.GetRightGripButton()) return;
        if (cc.isGrounded) return;

        //1. Set Hand Start Position
        if (!startPosSet && InputData.Data.GetRightGripButton())
        {
            handStartPos = InputData.Data.GetRHPosition();
            startPosSet = true;
        }

        //2. Trigger Air Dash
        var dist = Vector3.Distance(InputData.Data.GetRHPosition(), handStartPos);
        if (!isAirdashing && dist > triggerRadius && InputData.Data.GetRightHandVelocity().magnitude > threshold)
        {
            isAirdashing = true;
            velocity = Time.fixedDeltaTime * speedMultiplier * -InputData.Data.GetRightHandVelocity();
            state = AirDashState.Buffer;
            print("Buffer State");
        }
        
        if (state == AirDashState.Buffer || state == AirDashState.Accel)
        {
            var dot = Vector3.Dot(velocity, -InputData.Data.GetRightHandVelocity());
            // 3. Add Velocity while increasing speed of controller
            if (Mathf.Abs(InputData.Data.GetRightHandVelocity().magnitude) > threshold)
            {
                print("Accel State");
                state = AirDashState.Accel;
                velocity += Time.fixedDeltaTime * speedMultiplier * transform.TransformVector(-InputData.Data.GetRightHandVelocity());
            }
            // 4. Slow Down
            else if (state == AirDashState.Accel)
            {
                print("SlowDown State");
                state = AirDashState.SlowDown;
            }
        }
    }

    // void Check()
    // {
    //     if (!InputData.Data.GetRightGripButton()) return;
    //     if (cc.isGrounded) return;
    //     
    //     // Trigger Air Dash
    //     if (!isAirdashing && Mathf.Abs(InputData.Data.GetRightHandVelocity().magnitude) > threshold)
    //     {
    //         isAirdashing = true;
    //         velocity = Time.fixedDeltaTime * speedMultiplier * -InputData.Data.GetRightHandVelocity();
    //         state = AirDashState.Buffer;
    //     }
    //
    //     if (state == AirDashState.Buffer || state == AirDashState.Accel)
    //     {
    //         var dot = Vector3.Dot(velocity, -InputData.Data.GetRightHandVelocity());
    //         // Accel
    //         if (Mathf.Abs(InputData.Data.GetRightHandVelocity().magnitude) > threshold)
    //         {
    //             state = AirDashState.Accel;
    //             velocity += Time.fixedDeltaTime * speedMultiplier * transform.TransformVector(-InputData.Data.GetRightHandVelocity());
    //         }
    //         else if (state == AirDashState.Accel)
    //         {
    //             state = AirDashState.SlowDown;
    //         }
    //     }
    //     
    // }

    void Move()
    {
        if (!isAirdashing) return;
        
        cc.Move(velocity * Time.fixedDeltaTime);
    }

    void StopConditions()
    {
        if (!isAirdashing) return;
        
        // Stop Air Dash
        if (!InputData.Data.GetRightGripButton() || velocity.magnitude <= airFriction * Time.fixedDeltaTime)
        {
            ContinueOtherMoveComponents();
            return;
        }
        
        // Slow Down Air Dash
        if (state == AirDashState.SlowDown && velocity.magnitude > airFriction)
        {
            print("SlowDown State");
            velocity += airFriction * Time.fixedDeltaTime * -velocity.normalized;
            StopOtherMoveComponents();
        }
    }

    void StopOtherMoveComponents()
    {
        sm.SetHorzVelocity(Vector3.zero);
        jm.ToggleGravity(false);
    }

    void ContinueOtherMoveComponents()
    {
        jm.SetVelocity(velocity.y);
        velocity.y = 0;
        sm.SetHorzVelocity(velocity);
        jm.ToggleGravity(true);
        
        isAirdashing = false;
        velocity = Vector3.zero;

        state = AirDashState.Idle;
        print("Idle State");

    }
}
