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
        //if(IsLocalPlayer)
        //{
            HandleMove();
        //}
    }

    private void HandleMove()
    {
        if (GameManager.Instance.CurrentGameState == GameStates.STARTING ||
           GameManager.Instance.CurrentGameState == GameStates.PLAYING)
        {
            // NOTE: we're using the camera's orientation for the direction of the movement, not the sphere!
            Vector3 moveV = playerCameraFocalPoint.transform.forward * Input.GetAxis(InputKeys.VERTICAL) * speed * Time.deltaTime;
            Vector3 moveH = playerCameraFocalPoint.transform.right * Input.GetAxis(InputKeys.HORIZONTAL) * speed * Time.deltaTime;

            // move this to the server?
            playerRigidBody.AddForce(moveV);
            playerRigidBody.AddForce(moveH);
        }
    }
}
