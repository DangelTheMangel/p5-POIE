using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// inspried by: https://www.youtube.com/watch?v=5D2bN7xL5us
/// and: https://en.wikipedia.org/wiki/Proportional%E2%80%93integral%E2%80%93derivative_controller
/// </summary>
public class PID_HandController : MonoBehaviour
{
    [SerializeField] float frequency = 50f;
    [SerializeField] float damping = 1f;
    [SerializeField] Transform target;
    [SerializeField] float rotationFrequency = 100f;
    [SerializeField] float rotationDamping = 0.9f;
    [SerializeField] float ClimbForce = 1000f;
    [SerializeField] float climdragForce = 500;
    Vector3 previousPos;
    [SerializeField] 
    Rigidbody playerRb;
    private Rigidbody rb;
    bool isCollidning = false;

    // Start is called before the first frame update
    void Start()
    {
        isCollidning = false;
        transform.position = target.position;
        transform.rotation = target.rotation;
        rb= GetComponent<Rigidbody>();
        rb.maxAngularVelocity = float.PositiveInfinity;
        previousPos = transform.position;
    }

    private void FixedUpdate()
    {
        movement();
        //rotation();
        if(isCollidning)hookslaw();

    }

    private void hookslaw()
    {
        Vector3 displacementFromResting = transform.position - target.position;
        Vector3 force = displacementFromResting * ClimbForce;
        float drag = GetDrag();

        playerRb.AddForce(force, ForceMode.Acceleration);
        playerRb.AddForce(drag * -playerRb.velocity*climdragForce, ForceMode.Acceleration);
    }

    private float GetDrag()
    {
        Vector3 handVel = (target.localPosition - previousPos) / Time.fixedDeltaTime;
        float drag = 1/handVel.magnitude+0.01f;
        drag = drag > 1 ? 1 : drag;
        drag = drag < 0.03f ? 0.03f : drag;
        previousPos = transform.position;
        return drag;
    }

    private void rotation()
    {
        float kp = (6f * rotationFrequency) * (6f * rotationFrequency) * 0.25f;
        float kd = 4.5f * rotationFrequency * rotationDamping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Quaternion q = target.rotation * Quaternion.Inverse(transform.rotation);
        if (q.w < 0) {
            q.x = -q.x;
            q.y = -q.y;
            q.z = -q.z;
            q.w = -q.w;
        }
        q.ToAxisAngle(out Vector3 axis,out float angle);
        axis.Normalize();
        axis *= Mathf.Deg2Rad;
        Vector3 torque = ksg * axis * angle + rb.angularVelocity * kdg;
        rb.AddTorque(torque, ForceMode.Acceleration);

    }

    private void movement()
    {
        float kp = (6f * frequency) * (6f * frequency) * 0.25f;
        float kd = 4.5f * frequency * damping;
        float g = 1 / (1 + kd * Time.fixedDeltaTime + kp * Time.fixedDeltaTime * Time.fixedDeltaTime);
        float ksg = kp * g;
        float kdg = (kd + kp * Time.fixedDeltaTime) * g;
        Vector3 force = (target.position - transform.position) * ksg + (playerRb.velocity - rb.velocity) * kdg;
        rb.AddForce(force,ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        isCollidning= true;
    }

    private void OnCollisionExit(Collision collision)
    {
        isCollidning= false;
    }
}
