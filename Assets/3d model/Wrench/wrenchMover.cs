using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchMover : MonoBehaviour
{
    public Transform playerHandsTransform; // Reference to the player's hands transform
    public float moveSpeed = 1f; // Speed at which the wrench should move towards the player's hands
    public float moveThreshold = 1f; // Distance threshold for the wrench and player's hands to be considered in proximity

    void Update()
    {
        // If the wrench is not close enough to the player's hands, move it towards the player's hands
        if (Vector3.Distance(transform.position, playerHandsTransform.position) > moveThreshold)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerHandsTransform.position, moveSpeed * Time.deltaTime);
        }
    }
}
