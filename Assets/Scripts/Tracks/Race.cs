using System;
using System.Collections.Generic;
using UnityEngine;

public enum RaceTypes
{
    PureSpeed = 0,
    Battle = 1,
    Crazy = 2
}
public class Race
{
    private int _trackID;

    #region Public Properties
    public List<Racer> racers;
    public string DisplayName { get; set; }
    public int TotalLaps { get; }
    public int CurrentLap { get; private set; }
    public float StartDelay { get; }

    private string _sceneName = String.Empty;
    public string SceneName { get { return _sceneName; } }
    public RaceTypes RaceType { get; }
    public DateTime StartTime { get; private set; }
    public Vector3 StartingPosition
    {
        get
        {
            return GetStartingPositionByTrackID(_trackID);
        }
    }
    public Vector3 GetFinishPosition()
    {
        return GetFinishPositionByTrackID(_trackID);
    }

    public bool IsLapComplete
    {
        get
        {
            foreach (Racer r in this.racers)
            {
                if (r.finishTime == new DateTime()) { return false; }
            }

            return true;
        }
    }
    public bool isRaceComplete
    {
        get
        {
            return (CurrentLap > TotalLaps);
        }
    }

    #endregion

    #region Constructors
    public Race()
    {
        racers = new List<Racer>();
    }
    public Race(int trackID, RaceTypes raceTypeID, int lapCount, float startDelay)
    {
        _trackID = trackID;
        SetSceneNameByTrackID();
        RaceType = raceTypeID;
        TotalLaps = lapCount;
        StartDelay = startDelay;

        CurrentLap = 1; // NOTE: this is 1-based, not 0-based
    }
    #endregion

    #region Public Methods
    internal void SetStartTime(DateTime dt)
    {
        StartTime = dt;
    }

    public void AdanceLap()
    {
        CurrentLap++;
    }
    public void ResetRacersFinishTime()
    {
        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].finishTime = new DateTime();
        }
    }
    internal void ResetFinishTimes()
    {
        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].finishTime = new DateTime();
        }
    }
    internal void AddPlayer(Racer r)
    {
        if (racers == null) { racers = new List<Racer>(); }

        racers.Add(r);
    }
    internal void AddPlayer()
    {
        if (racers == null) { racers = new List<Racer>(); }

        racers.Add(Racer.CreateRandomRacer());
    }
    internal void SetFinishTime(DateTime t, GameObject racer)
    {
        //string name = string.Empty;
        //if (racer.GetComponent<PlayerSphereController>() != null) { name = racer.GetComponent<PlayerSphereController>().SphereName; }
        //else if (racer.GetComponent<AISphereController>() != null) { name = racer.GetComponent<AISphereController>().SphereName; }

        for (int i = 0; i < racers.Count; i++)
        {
            if(racers[i].Sphere == racer)
            {
                racers[i].finishTime = t;
            }
            // if this is the one we're looking for
            //if (racers[i].Name.Equals(name))
            //{
            //    racers[i].finishTime = t;
            //}
        }
    }
    internal void RemoveSlowestPlayers(int removeCount)
    {
        // assumption: the racers have already been sorted by their previous lap time
        for (int i = racers.Count - 1; i >= 0 && removeCount > 0; i--)
        {
            racers[i].Sphere.SetActive(false);

            // remove the camera attached to the player, if the player is getting destroyed too.
            if (racers[i].Sphere.GetComponent<PlayerSphereController>() != null)
            {
                GameObject.Destroy(racers[i].Sphere.GetComponent<PlayerSphereController>().playerCameraFocalPoint.gameObject);
            }

            // destroy the game object entirely
            GameObject.Destroy(racers[i].Sphere);

            // and finally, remove this racer from the group
            racers.RemoveAt(i);

            removeCount--;
        }
    }
    internal bool IsPlayerAmongRacers()
    {
        foreach (Racer r in racers)
        {
            if (r.Sphere.GetComponent<PlayerSphereController>() != null) { return true; }
        }

        return false;
    }
    #endregion

    #region Private Functions
    private void SetSceneNameByTrackID()
    {
        switch(_trackID)
        {
            case 0:
                _sceneName = "TestTrack";
                break;
        }
    }
    private Vector3 GetStartingPositionByTrackID(int trackID)
    {
        switch (trackID)
        {
            case 0:// "TestTrack"
            default:
                // search the game object for 'starting point'?
                return new Vector3(0f, 3f, -14f); // note: this is the combination of both the parent start area, and the start point
        }
    }
    private Vector3 GetFinishPositionByTrackID(int trackID)
    {
        switch (trackID)
        {
            case 0:// "Track01"
            default:
                return new Vector3(3f, 1f, 17f);
        }
    }
    #endregion
}