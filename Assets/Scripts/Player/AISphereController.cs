using System;
using UnityEngine;

public class AISphereController : SphereController
{
    private Vector3 _targetLocation;
    private bool _targetAcquired = false;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // if we've finished, stop moving.
        if (this.FinishTime != DateTime.MinValue) { return; }

        HandleMove();
    }

    private void HandleMove()
    {
        // had to put this here because the assets of the level are not all loaded 
        // at this sphere's start or awake method.
        if (!_targetAcquired) { AcquireTarget(); }

        // move towards the finish area
        Vector3 directionNeeded = (_targetLocation - this.transform.position);
        directionNeeded.Normalize();
        Vector3 intendedMovement = directionNeeded * speed * Time.deltaTime;

        // move this portion to the server?
        playerRigidBody.AddForce(intendedMovement);
    }

    private void AcquireTarget()
    {
        GameObject finishLine = GameObject.FindGameObjectWithTag("Finish");

        // in the very beginning of the level, the finish line is sometimes not there yet, so check first
        if(finishLine == null) { return; }

        _targetLocation = finishLine.transform.position;
        _targetAcquired = true;
    }
}
