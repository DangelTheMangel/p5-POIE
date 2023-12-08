using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{

    public GameObject cubePrefab;
    public float spawnInterval = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnPrefab", 0.0f, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            //Instantiate(cubePrefab, transform.position, Quaternion.identity);
        //}
    }

    void SpawnPrefab()
    {
        // Instantiate the prefab at the spawner's position and rotation
        Instantiate(cubePrefab, transform.position, Quaternion.identity);
    }
}
