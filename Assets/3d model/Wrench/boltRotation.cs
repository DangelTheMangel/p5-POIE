using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WrenchGame : MonoBehaviour
{
    public Transform jawTransform; // Reference to the jaw's transform
    public Transform[] boltTransforms; // Array of bolt transforms
    public float rotationThreshold = 1200f; // Rotation threshold for 10 degrees
    public float proximityThreshold = 0.2f; // Distance threshold for the jaw of the wrench and bolts to be considered in proximity

    private float[] totalRotations; // Array to track total rotation for each bolt
    private int points = 0; // Counter for points earned
    public int pointsNeededForSceneLoad = 6; // Adjust the number of points needed for scene load

    public Text pointsText; // Reference to the Text component for displaying points
    public Canvas pointsCanvas; // Reference to the Canvas component for positioning
    public AudioClip pointSound; // Sound to play when a point is gained
    public GameObject particlePrefab; // Particle system to instantiate when a point is gained




    void Start()
    {
        // Initialize arrays
        totalRotations = new float[boltTransforms.Length];

        // Initialize UI elements
        if (pointsCanvas != null)
        {
            pointsCanvas.worldCamera = Camera.main;
        }
    }

    void Update()
    {

        for (int i = 0; i < boltTransforms.Length; i++)
        {
            // Check if the bolt has been destroyed
            if (boltTransforms[i] == null)
            {
                continue;
            }

            // Check if the wrench is close enough to the bolt
            if (Vector3.Distance(jawTransform.position, boltTransforms[i].position) <= proximityThreshold)
            {
                // Calculate the angle between the wrench and the bolt
                float rotationDelta = Vector3.Angle(jawTransform.forward, boltTransforms[i].forward);
                Vector3 crossProduct = Vector3.Cross(jawTransform.forward, boltTransforms[i].forward);

                // If the wrench is rotating in the opposite direction, make the rotation delta negative
                if (crossProduct.y < 0)
                {
                    rotationDelta *= -1;
                }

                // Add the absolute value of the rotation delta to the total rotation for this bolt
                totalRotations[i] += Mathf.Abs(rotationDelta);

                // If the total rotation for this bolt is greater than or equal to the rotation threshold, generate a point
                if (totalRotations[i] >= rotationThreshold)
                {
                    GeneratePoint(i);
                }
            }
            else
            {
                // If the wrench is not close enough to the bolt, reset the total rotation for this bolt
                totalRotations[i] = 0f;
            }
        }

        // If the total points are greater than or equal to the points needed for scene load, load the next scene
        if (points >= pointsNeededForSceneLoad)
        {
         StartCoroutine(LoadNextSceneWithDelay());
         pointsText.text = "Good work: " + points;
        }

        IEnumerator LoadNextSceneWithDelay()
        // wait 5 seconds
        {
         yield return new WaitForSeconds(5);
         LoadNextScene();
        }


        // Update the points text
        if (pointsText != null)
        {
            pointsText.text = "Bolts loosened: " + points;
        }

    }

    void GeneratePoint(int boltIndex)
    {
    // Increment the points counter
    points++;

    // Play the point sound
    AudioSource.PlayClipAtPoint(pointSound, transform.position);

    // Instantiate the particle prefab at the position of the bolt
    Instantiate(particlePrefab, boltTransforms[boltIndex].position, Quaternion.identity);

    // Destroy the bolt game object
    Destroy(boltTransforms[boltIndex].gameObject);
    }


    void LoadNextScene()
    {
        // Load the next scene
        SceneManager.LoadScene("Scenes/Movement/Controller with all features");
    }
}
