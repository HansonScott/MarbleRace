﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSphereController : SphereController
{
    public PlayerCameraController playerCameraFocalPoint;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(UIManager.Instance.CurrentGameState == GameStates.STARTING ||
           UIManager.Instance.CurrentGameState == GameStates.PLAYING)
        {
            // NOTE: we're using the camera's orientation for the movement, not the sphere!
            float vInput = Input.GetAxis("Vertical");
            playerRigidBody.AddForce(playerCameraFocalPoint.transform.forward * vInput * speed * Time.deltaTime);

            float hInput = Input.GetAxis("Horizontal");
            playerRigidBody.AddForce(playerCameraFocalPoint.transform.right * hInput * speed * Time.deltaTime);
        }
    }
}
