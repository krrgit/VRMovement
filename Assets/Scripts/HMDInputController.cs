using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HMDInputController : MonoBehaviour
{
    [SerializeField] private CapsuleCollider lastInputZone;
    [SerializeField] private Transform hmd;
    [SerializeField] private Vector3 initDir;
    [SerializeField] private Vector3 adjustDir;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float resetNeutralThreshold = 0.5f;
    [SerializeField] float vertVel;
    private bool moveInputActive;
    bool prevMoveInputActive;


    private bool doublePressCheck;

    private Vector3 startPos;
    
    // Issue with this design
    // Player needs to reset neutral , otherwise sometimes doesn't move in right direcition

    public Vector3 HMDVelocity
    {
        get { return InputData.Data.GetHMDVelocity(); }
    }

    public float VertVelocity
    {
        get { return InputData.Data.GetHMDVelocity().y;;  }
    }
    public Vector3 HorzDirection
    {
        get
        {
            return direction;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        GetTilt();
        ResetNeutralCheck();
        TrackLastActiveHMDPosition();
        
        prevMoveInputActive = moveInputActive;
    }

    // 1. Get Button Input
    void GetInput()
    {
        moveInputActive = InputData.Data.GetRightTriggerButton();
        vertVel = InputData.Data.GetHMDVelocity().y;
    }
    
    // 2. Check if neutral needs to be reset
    void ResetNeutralCheck()
    {
        // On Trigger Up/Down
        if (prevMoveInputActive != moveInputActive)
        {
            var dist = (hmd.localPosition - lastInputZone.transform.localPosition);
            dist.y = 0;
            var thisDir = (hmd.localPosition - startPos);
            thisDir.y = 0;
            
            // Reset if Close to start position || Outside Last Input Zone
            if (thisDir.magnitude <= lastInputZone.radius || dist.magnitude > lastInputZone.radius)
            {
                SetNeutral();
            }
        }
    }

    void SetNeutral()
    {
        startPos = hmd.localPosition;
    }

    // 3.Track the last position of the headset while moving. Used in ResetNeutralCheck()
    void TrackLastActiveHMDPosition()
    {
        if (moveInputActive)
        {
            lastInputZone.transform.localPosition = hmd.localPosition;
        }
    }
    
    
    // 4. Get Tilt Direction
    void GetTilt()
    {
        if (!moveInputActive) return;
        var vector = (hmd.localPosition - startPos) / lastInputZone.radius;
        vector.y = 0;

        var proj = Vector3.Project(vector, direction);
        var horz = vector - proj;
        horz *= 0.67f;

        vector = proj + horz;

        direction = Vector3.ClampMagnitude(vector, 1.0f);
        direction = transform.TransformVector(direction);
    }
}
