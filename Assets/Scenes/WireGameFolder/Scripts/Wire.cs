using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField] private string destinationTag;
    Vector3 offset;

    private void Start()
    {
        Debug.Log("The start script is working");
        line = GetComponent<LineRenderer>();
        if (line == null)
        {
            Debug.LogError("LineRenderer component not found on " + gameObject.name);
        }
    }

    private void OnMouseDown()
    {
        offset = transform.position - MouseWorldPosition();
        Debug.Log("MouseDown");
    }

    private void OnMouseDrag()
    {
        line.SetPosition(0, MouseWorldPosition() + offset);
        line.SetPosition(1, transform.position);
        Debug.Log("MouseDrag");
    }

    private void OnMouseUp()
    {
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDir = MouseWorldPosition() - Camera.main.transform.position;
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, rayDir, out hitInfo))
        {
            if (hitInfo.transform.CompareTag(destinationTag))
            {
                line.SetPosition(0, hitInfo.transform.position);
                transform.gameObject.GetComponent<Collider>().enabled = false;
                Debug.Log("MouseUp");
            }
            else
            {
                line.SetPosition(0, transform.position);
                Debug.Log("MouseUpElse");
            }
        }
    }

    private Vector3 MouseWorldPosition()
    {
        Debug.Log("MouseWorldPosition");
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
        
    }
}
