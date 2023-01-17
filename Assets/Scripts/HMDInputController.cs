using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HMDInputController : MonoBehaviour
{
    [SerializeField] private CapsuleCollider hmdIdleZone;
    [SerializeField] private Transform hmd;
    [SerializeField] private Vector3 initDir;
    [SerializeField] private Vector3 adjustDir;
    [SerializeField] private Vector3 direction;
    [SerializeField] float vertVel;
    private bool moveInputActive;
    bool prevMoveInputActive;


    private bool doublePressCheck;
    
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
        GetTilt();
        ResetNeutralCheck();
        
        prevMoveInputActive = moveInputActive;
    }

    void SetNeutral()
    {
        hmdIdleZone.transform.localPosition = hmd.localPosition;
    }

    void GetTilt()
    {
        if (!moveInputActive) return;
        var vector = (hmd.localPosition - hmdIdleZone.transform.localPosition) / hmdIdleZone.radius;
        vector.y = 0;

        var proj = Vector3.Project(vector, direction);
        var horz = vector - proj;
        horz *= 0.5f;

        vector = proj + horz;

        direction = Vector3.ClampMagnitude(vector, 1.0f);
        direction = transform.TransformVector(direction);
    }

    void ResetNeutralCheck()
    {
        // First Press
        if (prevMoveInputActive == false && moveInputActive)
        {
            if (!doublePressCheck)
            {
                StartCoroutine(IResetCheck());
            }
        }
    }

    IEnumerator IResetCheck()
    {
        doublePressCheck = true;
        print("First Press");
        yield return new WaitForSeconds(0.1f);
        
        float timer = 1f;
        while (timer > 0)
        {
            // Second Press
            if (prevMoveInputActive == false && moveInputActive)
            {
                print("Reset Neutral");
                SetNeutral();
                break;
            }
            yield return new WaitForEndOfFrame();
            timer -= Time.deltaTime;
        }

        doublePressCheck = false;
    }
    
    // 1. Get Button Input
    void GetInput()
    {
        moveInputActive = InputData.Data.GetRightTriggerButton();
        vertVel = InputData.Data.GetHMDVelocity().y;
    }


}
