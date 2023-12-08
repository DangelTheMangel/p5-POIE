using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomarIsAHillbilly : MonoBehaviour
{
    float Zackie = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object has a tag or specific characteristics (in this case, Object B)
        if (collision.gameObject.CompareTag("LyleBurt"))
        {
            Zackie++;
            // Destroy the collided object (Object B)
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Racism"))
        {
            Destroy(collision.gameObject);
        }
    }
}
