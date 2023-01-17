using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToHand : MonoBehaviour
{
    [SerializeField]private Transform hand;
    [SerializeField] private Quaternion rotOffset;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = hand.position;
        transform.rotation = hand.rotation * rotOffset;
    }
}
