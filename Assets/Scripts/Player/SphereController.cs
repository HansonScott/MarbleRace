using UnityEngine;
using MLAPI;
using System;

public abstract class SphereController : NetworkedBehaviour
{
    [SerializeField] protected float speed;
    public string SphereName;
    public DateTime FinishTime;
    protected Rigidbody playerRigidBody;
}
