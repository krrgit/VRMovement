using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MotionbodyData", menuName = "ScriptableObjects/Motionbody", order = 1)]
public class MotionbodySO : ScriptableObject
{
    // PER FRAME
    public float fastWalkSpeed = 1.375f;
    public float grFriction = 0.01f;
    public float dashInitVel = 0.24f;
    public float runAccel = 0.01f;
    public float jumpInitHorzVel = 0.22f;
    public float jumpInitVertVel = 0.532f; // 4.1 Melee units per frame
    public float hopInitVertVel = 0.245f;
    public float grMaxHorzVel = 0.19f;
    public float fallTermVel = -0.4f;
    public float airMobility = 0.01f;
    public float airMaxHorzSpeed = 0.108f;
    public float airFriction = 0.03f;
    public float gravity = 0.022f;

}
