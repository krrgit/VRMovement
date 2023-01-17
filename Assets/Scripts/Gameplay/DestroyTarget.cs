using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTarget : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        TargetManager.Instance.ResetTimer(gameObject);
    }
}
