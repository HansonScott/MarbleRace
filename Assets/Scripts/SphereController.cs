using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class SphereController : NetworkedBehaviour
{
    [SerializeField] protected float speed;
    public string SphereName;
    public DateTime FinishTime;
    protected Rigidbody playerRigidBody;

    // Update is called once per frame
    void FixedUpdate()
    {
        // if fell too far down, start over.
        if(this.gameObject.transform.position.y < (GameManager.Instance.currentRace.FinishPosition.y - 20))
        { this.gameObject.transform.position = GameManager.Instance.currentRace.StartingPosition; }
    }
}
