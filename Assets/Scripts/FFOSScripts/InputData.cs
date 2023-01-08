using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputData : MonoBehaviour
{
    public InputDevice RightController;
    public InputDevice LeftController;
    public InputDevice HMD;

    public static InputData Data;

    void Start() {
        if (Data == null) {
            Data = this;
        } else {
            Destroy(this);
        }
    } 


    void Update()
    {
        if (!RightController.isValid || !LeftController.isValid || !HMD.isValid)
            InitializeInputDevices();
    }
    private void InitializeInputDevices()
    {
        
        if(!RightController.isValid)
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Right, ref RightController);
        if (!LeftController.isValid) 
            InitializeInputDevice(InputDeviceCharacteristics.Controller | InputDeviceCharacteristics.Left, ref LeftController);
        if (!HMD.isValid) 
            InitializeInputDevice(InputDeviceCharacteristics.HeadMounted, ref HMD);

    }

    private void InitializeInputDevice(InputDeviceCharacteristics inputCharacteristics, ref InputDevice inputDevice)
    {
        List<InputDevice> devices = new List<InputDevice>();
        //Call InputDevices to see if it can find any devices with the characteristics we're looking for
        InputDevices.GetDevicesWithCharacteristics(inputCharacteristics, devices);

        //Our hands might not be active and so they will not be generated from the search.
        //We check if any devices are found here to avoid errors.
        if (devices.Count > 0)
        {
            inputDevice = devices[0];
        }
    }

    public bool GetRightButton()
    {
        bool value = false;
        RightController.TryGetFeatureValue(CommonUsages.triggerButton, out value);
        return value;
    }

    public float GetRightTrigger()
    {
        float value = 0.0f;
        RightController.TryGetFeatureValue(CommonUsages.trigger, out value);
        return value;
    }

    public bool GetRightGripButton()
    {
        bool value;
        RightController.TryGetFeatureValue(CommonUsages.gripButton, out value);
        return value;
    }

    public Vector3 GetHMDPosition()
    {
        Vector3 value = Vector3.zero;
        HMD.TryGetFeatureValue(CommonUsages.devicePosition, out value);
        return value;
    }

    public Vector3 GetHMDVelocity()
    {
        Vector3 value = Vector3.zero;
        HMD.TryGetFeatureValue(CommonUsages.deviceVelocity, out value);
        return value;
    }

    public Vector3 GetRightHandVelocity()
    {
        Vector3 value = Vector3.zero;
        RightController.TryGetFeatureValue(CommonUsages.deviceVelocity, out value);
        return value;
    }

}
