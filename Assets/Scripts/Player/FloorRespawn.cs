using UnityEngine;

public class FloorRespawn : MonoBehaviour
{
    private float FloorDepthBeforeRestart = 2f;

    // Update is called once per frame
    void FixedUpdate()
    {
        // if fell too far down (below the finishline), start over.
        if (this.gameObject.transform.position.y < 
            (GameManager.Instance.CurrentRace.GetFinishPosition().y - FloorDepthBeforeRestart))
        { this.gameObject.transform.position = GameManager.Instance.CurrentRace.StartingPosition; }
    }
}
