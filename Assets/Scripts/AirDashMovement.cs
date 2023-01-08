using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AirDashState
{
    Idle,
    Accel,
    SlowDown
}
public class AirDashMovement : MonoBehaviour
{
    [SerializeField] private CharacterController cc;
    [SerializeField] private JumpMovement jm;
    [SerializeField] private SimpleMovement sm;

    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float threshold = 5;
    [SerializeField] float airFriction = 0.04f;
    [SerializeField] private bool isAirdashing;
    [SerializeField] private Vector3 velocity;
    
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
        
        // Trigger Air Dash
        if (!isAirdashing && Mathf.Abs(InputData.Data.GetRightHandVelocity().magnitude) > threshold)
        {
            isAirdashing = true;
            velocity = Time.fixedDeltaTime * speedMultiplier * -InputData.Data.GetRightHandVelocity();
        }

        if (isAirdashing)
        {
            var dot = Vector3.Dot(velocity, -InputData.Data.GetRightHandVelocity());
            // Accel
            if (Mathf.Abs(InputData.Data.GetRightHandVelocity().magnitude) > threshold)
            {
                velocity += Time.fixedDeltaTime * speedMultiplier * transform.TransformVector(-InputData.Data.GetRightHandVelocity());
            }
        }
        
    }

    void Move()
    {
        if (!isAirdashing) return;
        
        cc.Move(velocity);
    }

    void StopConditions()
    {
        if (!isAirdashing) return;
        
        // Stop Air Dash
        if (!InputData.Data.GetRightGripButton() || velocity.magnitude <= airFriction)
        {
            ContinueOtherMoveComponents();
            return;
        }
        
        // Slow Down Air Dash
        if (velocity.magnitude > airFriction)
        {
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
        velocity.y = 0;
        sm.SetHorzVelocity(velocity);
        jm.ToggleGravity(true);
        
        isAirdashing = false;
        velocity = Vector3.zero;
    }
}
