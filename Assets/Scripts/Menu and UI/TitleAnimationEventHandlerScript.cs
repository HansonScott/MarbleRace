using UnityEngine;

public class TitleAnimationEventHandlerScript : MonoBehaviour
{
    public void HandleAnimationEnd()
    {
        //Debug.Log("Animation ended.");
        GameManager.Instance.MainMenu();
    }
}
