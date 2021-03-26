using System;
using System.Collections;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public float StartDelay;

    [SerializeField] private GameObject StartWall;

    void Start()
    {
        StartCoroutine(DropStartWall());
    }

    private IEnumerator DropStartWall()
    {
        // wait for the delay
        yield return new WaitForSeconds(StartDelay);

        StartWall.transform.Translate(Vector3.down * StartWall.transform.localScale.y);

        GameManager.Instance.CurrentRace.SetStartTime(DateTime.Now);
    }

}
