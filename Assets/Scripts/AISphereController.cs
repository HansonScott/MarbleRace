using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISphereController : SphereController
{
    private Vector3 targetLocation;
    private bool targetAcquired = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsLocalPlayer)
        {
            HandleMove();
        }
    }

    private void HandleMove()
    {
        // if we've finished, stop processing.
        if (this.FinishTime != DateTime.MinValue) { return; }

        // had to put this here because the assets of the level are not all loaded on start or awake.
        if (!targetAcquired) { AcquireTarget(); }


        // move towards the finish area
        Vector3 directionNeeded = (targetLocation - this.transform.position);

        directionNeeded.Normalize();
        playerRigidBody.AddForce(directionNeeded * speed * Time.deltaTime);
    }

    private void AcquireTarget()
    {
        GameObject finishLine = GameObject.FindGameObjectWithTag("Finish");

        if(finishLine != null)
        {
            targetLocation = finishLine.transform.position;
            targetAcquired = true;
            //Debug.Log("Target acquired: " + targetLocation.ToString());
        }
        else
        {
            //Debug.Log("Target still not acquired on update.");
        }
    }
}
