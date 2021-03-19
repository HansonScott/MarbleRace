using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    public GameObject player;
    [SerializeField] private float turnSpeed;

    private Vector3 previousMousePosition;
    private Vector3 previousPlayerPosition;

    private Vector3 mouseMovement;
    private float diffX, diffY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // start with something
        previousMousePosition = Input.mousePosition;
        previousPlayerPosition = player.transform.position;

        // a one time thing, so we can be relative to the player
        transform.position = player.transform.position;
    }

    void LateUpdate()
    {
        #region move camera to player position
        // move the camera's focal point position with the sphere's position change
        transform.position = transform.position + (player.transform.position - previousPlayerPosition);

        // track player position for next loop
        previousPlayerPosition = player.transform.position;
        #endregion

        #region Horizontal camera Control - using mouse movement (DISCONTINUED)
        //// h rotate camera based on h mouse movement
        //mouseMovement = previousMousePosition - Input.mousePosition;
        //diffX = mouseMovement.x;
        //transform.Rotate(Vector3.up, -diffX * turnSpeed * Time.deltaTime);
        #endregion

        #region Horizontal Control - using mouse down only
        // when the mouse is first pressed down, capture that as the starting point
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = Input.mousePosition;
        }

        // if the moue is 'still' being pressed down, then capture the movement
        if(Input.GetMouseButton(0))
        {
            // h rotate camera based on h mouse movement
            mouseMovement = previousMousePosition - Input.mousePosition;
            diffX = mouseMovement.x;
            transform.Rotate(Vector3.up, -diffX * turnSpeed * Time.deltaTime);
        }
        #endregion

        #region vertical control - using mouse movement (NOT WORKING)
        diffY = mouseMovement.y;
        // nope - this wiggles all over the place, not linear
        //transform.Rotate(transform.right, diffY * turnSpeed * Time.deltaTime);
        // nope - this uses global angle, so if we rotate, then this causes a sideways tilt instead of pitch
        // NOTE: did work until the ball started rolling.
        //transform.Rotate(Vector3.right, diffY * turnSpeed * Time.deltaTime);
        //nope - rolls around with the ball, so camara gets thrown into loops
        // NOTE: almost worked before moving the ball though.
        //transform.localRotation = new Quaternion(diffY * turnSpeed * Time.deltaTime, 
        //                                        player.transform.localRotation.y,
        //                                        player.transform.localRotation.z,
        //                                        player.transform.localRotation.w);

        // almost works!  need to guardrail it, way too sensitive.
        //transform.localRotation = new Quaternion(diffY * turnSpeed,
        //                                            transform.localRotation.y,
        //                                            transform.localRotation.z,
        //                                            transform.localRotation.w);

        // back to jittery.  apparently with the delta time involved.
        //transform.localRotation = new Quaternion(diffY * turnSpeed * Time.deltaTime,
        //                                            transform.localRotation.y,
        //                                            transform.localRotation.z,
        //                                            transform.localRotation.w);

        // it's close, but it still falls to the diagonal after moving around a while.
        //diffY = Math.Min(diffY, 360);
        //diffY = diffY / 360;
        //transform.localRotation = new Quaternion(diffY * turnSpeed,
        //                                    transform.localRotation.y,
        //                                    transform.localRotation.z,
        //                                    transform.localRotation.w);
        #endregion
    }
}
