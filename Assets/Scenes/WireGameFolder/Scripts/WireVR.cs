using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WireVR : MonoBehaviour
{
    private LineRenderer line;
    [SerializeField] private string destinationTag;
    [SerializeField] private float snapDistance = 0.1f; // Distance within which the wire snaps to the target
    private XRBaseInteractor interactorHoldingThis;
    private Transform snapDestination;

    private void Awake()
    {
        line = GetComponent<LineRenderer>();
        if (line == null)
        {
            Debug.LogError("LineRenderer component not found on " + gameObject.name);
        }
    }

    public void OnSelectEnter(XRBaseInteractor interactor)
    {
        interactorHoldingThis = interactor;
        // Optional: You can initiate the line here as well
    }

    public void OnSelectExit(XRBaseInteractor interactor)
    {
        if (interactor == interactorHoldingThis)
        {
            // Attempt to snap to the closest destination
            AttemptSnapToDestination();
            interactorHoldingThis = null;
        }
    }

    private void Update()
    {
        if (interactorHoldingThis != null)
        {
            Vector3 interactorPosition = interactorHoldingThis.transform.position;
            line.SetPosition(0, interactorPosition);
            line.SetPosition(1, transform.position);

            // Check for nearby snap points when dragging
            if (snapDestination == null)
            {
                FindSnapDestination(interactorPosition);
            }
        }
    }

    private void FindSnapDestination(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, snapDistance);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag(destinationTag))
            {
                snapDestination = collider.transform;
                break;
            }
        }
    }

    private void AttemptSnapToDestination()
    {
        // If we have a snap destination and it's close enough, snap to it
        if (snapDestination != null && Vector3.Distance(transform.position, snapDestination.position) <= snapDistance)
        {
            line.SetPosition(1, snapDestination.position);
            // Disable the collider if you want to prevent further interaction
            GetComponent<Collider>().enabled = false;
        }
        else
        {
            // Reset the wire or do something else if it didn't snap
            line.SetPosition(1, transform.position);
        }
        // Clear the snap destination after the attempt
        snapDestination = null;
    }
}
