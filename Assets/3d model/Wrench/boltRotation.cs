using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchGame : MonoBehaviour
{
    public Transform wrenchTransform; // Reference to the wrench's transform
    public Transform[] boltTransforms; // Array of bolt transforms
    public float rotationThreshold = 100f; // Rotation threshold for 10 degrees
    public float proximityThreshold = 0.2f; // Distance threshold for the wrench and bolts to be considered in proximity

    private float[] totalRotations; // Array to track total rotation for each bolt
    private bool[] pointsGenerated; // Array to track whether points have been generated for each bolt

    void Start()
    {
        // Initialize arrays
        totalRotations = new float[boltTransforms.Length];
        pointsGenerated = new bool[boltTransforms.Length];
    }

    void Update()
    {
        // Iterate through each bolt
        for (int i = 0; i < boltTransforms.Length; i++)
        {
            // Check if the wrench and bolt are in proximity
            if (Vector3.Distance(wrenchTransform.position, boltTransforms[i].position) <= proximityThreshold)
            {
                // Calculate the rotation delta between the wrench and the bolt
                float rotationDelta = Vector3.Angle(wrenchTransform.forward, boltTransforms[i].forward);

                // Check if the wrench is rotating clockwise or counterclockwise
                Vector3 crossProduct = Vector3.Cross(wrenchTransform.forward, boltTransforms[i].forward);
                if (crossProduct.y < 0)
                {
                    rotationDelta *= -1; // Reverse the rotation if it's counterclockwise
                }

                // Update the total rotation for the current bolt
                totalRotations[i] += Mathf.Abs(rotationDelta);

                // Check if enough rotation has occurred for the current bolt
                if (totalRotations[i] >= rotationThreshold && !pointsGenerated[i])
                {
                    // Generate a point for the current bolt
                    GeneratePoint(i);

                    // Set a flag to ensure points are not generated multiple times for the current bolt
                    pointsGenerated[i] = true;
                }
            }
            else
            {
                // Reset the rotation tracking when the wrench and bolt are not in proximity for the current bolt
                totalRotations[i] = 0f;
                pointsGenerated[i] = false;
            }
        }
    }

    void GeneratePoint(int boltIndex)
    {
        // Add your logic here to generate a point when enough rotation occurs for the specified bolt
        Debug.Log($"10 degrees of rotation completed for bolt {boltIndex + 1}! Point generated!");

        // Instantiate a particle effect at the position of the bolt
        InstantiateParticleEffect(boltTransforms[boltIndex].position);

        // You can add additional logic here for scoring or other actions
    }

    void InstantiateParticleEffect(Vector3 position)
    {
        // Replace "YourParticlePrefab" with the actual particle effect prefab you want to use
        GameObject particleEffect = Instantiate(Resources.Load<GameObject>("ParticlePrefab"), position, Quaternion.identity);

        // Destroy the particle effect after a certain duration (adjust as needed)
        Destroy(particleEffect, 2f);
    }
}


