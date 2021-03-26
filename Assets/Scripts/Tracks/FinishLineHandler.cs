using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FinishLineEvent : UnityEvent<GameObject> { }

public class FinishLineHandler : MonoBehaviour
{
    public FinishLineEvent onPlayerFinished = new FinishLineEvent();

    private void OnTriggerEnter(Collider other)
    {
        // cant't figure out how to connect to this event
        onPlayerFinished?.Invoke(other.gameObject);

        // this should be called by the listener now
        //Debug.Log("" + other.gameObject.name + " has hit the finish line!");
    }
}
