using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltInteraction : MonoBehaviour
{
    public string handColliderTag = "SpaceHand"; // Tag of the VR hand's collider

    private Rigidbody rb;
    private bool isInHandCollider = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found. Make sure this script is attached to a GameObject with a Rigidbody.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collider is the VR hand's collider
        if (other.CompareTag(handColliderTag))
        {
            isInHandCollider = true;
            UpdateRigidbodyConstraints();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the collider is the VR hand's collider
        if (other.CompareTag(handColliderTag))
        {
            isInHandCollider = false;
            UpdateRigidbodyConstraints();
        }
    }

    void UpdateRigidbodyConstraints()
    {
        if (rb != null)
        {
            // Freeze or unfreeze the rigidbody based on whether it's in the hand collider
            rb.constraints = isInHandCollider ? RigidbodyConstraints.None : RigidbodyConstraints.FreezeAll;
        }
    }
}

