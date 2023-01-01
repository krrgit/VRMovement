using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMDInputController : MonoBehaviour
{
    [SerializeField] private SphereCollider hmdIdleZone;
    [SerializeField] private Transform hmd;
    [SerializeField] private Vector3 initDir;
    [SerializeField] private Vector3 adjustDir;
    [SerializeField] private Vector3 direction;
    [SerializeField] float vertVel;
    private bool moveInputActive;
    bool prevMoveInputActive;
    

    public Vector3 HMDVelocity
    {
        get { return InputData.Data.GetHMDVelocity(); }
    }

    public float VertVelocity
    {
        get { return vertVel;  }
    }
    public Vector3 HorzDirection
    {
        get
        {
            var d = direction;
            d.y = 0;
            return d;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        GetInput();
        
    }

    private void FixedUpdate()
    {
        Adjust();
        UpdateTrigger();
    }

    // Avoid Issues with moving up and down
    void UpdateTriggerYPos()
    {
        var pos = transform.localPosition;
        pos.y = hmd.localPosition.y;
        transform.localPosition = pos;
    }

    // 1. Get Button Input
    void GetInput()
    {
        moveInputActive = InputData.Data.GetRightButton();
        vertVel = InputData.Data.GetHMDVelocity().y;  
    }

    void UpdateTrigger()
    {
        hmdIdleZone.enabled = moveInputActive;
        
        if (moveInputActive != prevMoveInputActive)
        {
            ResetZonePosition();
            prevMoveInputActive = moveInputActive;
        }

        if (!moveInputActive)
        {
            initDir = adjustDir = direction = Vector3.zero;
        }
        
        UpdateTriggerYPos();
    }

    // 2. Set the position of the zone to the position of the hmd
    void ResetZonePosition()
    {
        hmdIdleZone.transform.position = hmd.position;
    }

    // 3. Move head outside of trigger to input
    private void OnTriggerExit(Collider other)
    {
        print("Head Exit Trigger");
        // Input Direction
        initDir = other.transform.position - transform.position;
        initDir.y = 0;
        initDir = initDir.normalized;
        ResetZonePosition();
    }
    
    
    // 4. Adjust input when hmd moves inside the trigger
    void Adjust()
    {
        if (!moveInputActive) return;
        adjustDir = (hmd.position - hmdIdleZone.transform.position) / hmdIdleZone.radius;
        adjustDir.y = 0;
        direction = Vector3.ClampMagnitude(initDir + adjustDir, 1.0f);
    }
}
