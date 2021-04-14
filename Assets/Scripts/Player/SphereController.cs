using UnityEngine;
using System;

public abstract class SphereController : MonoBehaviour
{
    [SerializeField] protected float speed;
    public string SphereName;
    public DateTime FinishTime;
    protected Rigidbody playerRigidBody;
}
