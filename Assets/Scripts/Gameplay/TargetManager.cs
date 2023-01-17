using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private float resetWait = 10;
    public static TargetManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ResetTimer(GameObject target)
    {
        StartCoroutine(Wait(target));
    }
    
    
    IEnumerator Wait(GameObject target)
    {
        target.SetActive(false);
        SFXManager.Instance.HitTarget();
        yield return new WaitForSeconds(resetWait);
        target.SetActive(true);
    }
}
