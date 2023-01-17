using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InputArrow : MonoBehaviour
{
    [SerializeField] private HMDInputController input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = input.HorzDirection;
        transform.localScale = new Vector3(1, 1, input.HorzDirection.magnitude);
        var localPos = Camera.main.transform.localPosition;
        localPos.y = 0;
        transform.localPosition = localPos;
    }
}
