using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public float StartDelay;

    [SerializeField] private GameObject StartWall;
    //[SerializeField] private float startWallDropDistance;

    // Start is called before the first frame update
    void Start()
    {
        // what should the track do when it starts

        StartCoroutine(DropStartWall());
    }

    private IEnumerator DropStartWall()
    {
        // wait for the delay
        yield return new WaitForSeconds(StartDelay);

        StartWall.transform.Translate(Vector3.down * StartWall.transform.localScale.y);

        GameManager.Instance.currentRace.StartTime = DateTime.Now;
    }

}
