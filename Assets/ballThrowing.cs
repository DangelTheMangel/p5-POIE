using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ballThrowing : MonoBehaviour
{
    [SerializeField]
    Transform spawnpoint;
    [SerializeField]
    ThrowingMinigame throwingMinigame;
    [SerializeField]
    Rigidbody rb;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            transform.position = spawnpoint.position;

        }
        else if (collision.gameObject.tag == "Target")
        {
            transform.position = spawnpoint.position;
            throwingMinigame.addAPoint();
            collision.transform.position = throwingMinigame.getRandimPost();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
