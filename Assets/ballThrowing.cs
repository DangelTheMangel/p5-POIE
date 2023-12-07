using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ballThrowing : MonoBehaviour
{
    [SerializeField]
    public Transform spawnpoint;
    [SerializeField]
    public ThrowingMinigame throwingMinigame;
    [SerializeField]
    Rigidbody rb;
    bool active = true;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor" && active && !throwingMinigame.isdone)
        {
            transform.position = spawnpoint.position;
            rb.velocity = Vector3.zero;
            

        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Floor" && active)
        {
            transform.position = spawnpoint.position;
            
            //rb.velocity = Vector3.zero;


        }
        else if (collision.gameObject.tag == "Target")
        {

            throwingMinigame.addAPoint();
            collision.transform.position = throwingMinigame.getRandimPost();
            this.enabled = false;
            active = false;
            rb.velocity= Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    public void startGame() {
        throwingMinigame.startGame();
    }
}
