using System;
using UnityEngine;
using MLAPI;

public class SphereController : NetworkedBehaviour
{
    private float FloorDepthBeforeRestart = 10;

    [SerializeField] protected float speed;
    public string SphereName;
    public DateTime FinishTime;
    protected Rigidbody playerRigidBody;

    // Update is called once per frame
    void FixedUpdate()
    {
        // if fell too far down, start over.
        if(this.gameObject.transform.position.y < (GameManager.Instance.CurrentRace.GetFinishPosition().y - FloorDepthBeforeRestart))
        { this.gameObject.transform.position = GameManager.Instance.CurrentRace.StartingPosition; }
    }
}
