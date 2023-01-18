using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum WeaponState
{
    Idle,
    Slash,
    Stab,
    Throw
}
public class AttackDetection : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private GameObject hurtbox;
    [SerializeField] private Vector3 localVelocity;
    [SerializeField] private WeaponState state;
    [SerializeField] private float attackThreshold = 1;

    public UnityEvent StartAttackEvent;
    public UnityEvent StopAttackEvent;

    private Vector3 localPrevPos;

    private float maxDirVel;
    
    // Start is called before the first frame update
    void Start()
    {
        localPrevPos = transform.localPosition;
        StopAttack();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ComputeVelocity();
        ActivationCheck();
        StopAttackCheck();
        
        
        GetCurentMaxVelStep();
    }
    
    // 1. Compute Velocity
    void ComputeVelocity()
    {
        if (hand)
        {
            localVelocity = hand.localPosition - localPrevPos;
            localVelocity /= Time.fixedDeltaTime;
            
            localPrevPos = hand.localPosition;
        }
        else
        {
            localVelocity = transform.localPosition - localPrevPos;
            localVelocity /= Time.fixedDeltaTime;
            
            localPrevPos = transform.localPosition;
        }
    }

    void GetCurentMaxVelStep()
    {
        if (state == WeaponState.Idle) return; 
        maxDirVel = Mathf.Max(maxDirVel, localVelocity.magnitude);
    }
    
    // 2. Activate Hurtbox: Check if velocity exceeds threshold
    void ActivationCheck()
    {
        if (state != WeaponState.Idle) return;
        
        if (localVelocity.magnitude > attackThreshold)
        {
            state = WeaponState.Slash;
            StartAttack();
        } 
    }
    
    // 4. Deactivate Hurtbox: When current velocity step is less than max
    void StopAttackCheck()
    {
        if (state == WeaponState.Idle) return;
        
        // Add something here for throwing

        if (localVelocity.magnitude < attackThreshold)
        {
            StopAttack();
        }
    }

    void StartAttack()
    {
        maxDirVel = localVelocity.magnitude;
        hurtbox.SetActive(true);
        StartAttackEvent?.Invoke();
    }
    
    void StopAttack()
    {
        hurtbox.SetActive(false);
        state = WeaponState.Idle;
        StopAttackEvent?.Invoke();
    }
}
