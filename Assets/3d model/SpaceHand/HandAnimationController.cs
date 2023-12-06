using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandAnimationController : MonoBehaviour
{

    public InputActionProperty pinchAnimation;
    public InputActionProperty grabAnimation;

    public Animator handAnimation;

    void Start()
    {
        
    }

    void Update()
    {
        float pinchValue = pinchAnimation.action.ReadValue<float>();
        handAnimation.SetFloat("Pinch", pinchValue);

        float grabValue = grabAnimation.action.ReadValue<float>();
        handAnimation.SetFloat("Grab", grabValue);
    }
}
