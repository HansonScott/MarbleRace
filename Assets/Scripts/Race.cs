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
    public List<Racer> racers;
    public string displayName { get; set; }
    private int _trackID;
    private int _lapCount;
    private int _currentLap = 1;

    private float _startDelay;
    public float StartDelay { get { return _startDelay; } }

    private string _sceneName = "Track01";
    public string SceneName { get { return _sceneName; } }
    public Vector3 StartingPosition 
    {
        get
        {

            return GetStartingPositionByTrackID(_trackID);
        } 
    }
    public Vector3 FinishPosition
    {
        get
        {
            return GetFinishPositionByTrackID(_trackID);
        }
    }

    private RaceTypes _raceType;
    public RaceTypes RaceType
    {
        get { return _raceType; }
    }

    public DateTime StartTime;

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
            return (_currentLap > _lapCount);
        }
    }

    public void AdanceLap()
    {
        _currentLap++;
    }

    public void ResetRacersFinishTime()
    {
        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].finishTime = new DateTime();
        }
    }

    private Vector3 GetStartingPositionByTrackID(int trackID)
    {
        switch(trackID)
        {
            case 0:// "Track01"
            default:
                return new Vector3(0f, 78f, -215f); // note: this is the combination of both the parent start area, and the start point
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

    public Race()
    {
        racers = new List<Racer>();
    }

    public Race(int trackID, RaceTypes raceTypeID, int lapCount, float startDelay)
    {
        _trackID = trackID;
        _raceType = raceTypeID;
        _lapCount = lapCount;
        _startDelay = startDelay;
    }

    internal void AddPlayer(Racer r)
    {
        if (racers == null) { racers = new List<Racer>(); }

        racers.Add(r);
    }
    internal void AddPlayer()
    {
        if(racers == null) { racers = new List<Racer>(); }

        racers.Add(Racer.CreateRandomRacer());
    }

    internal void SetFinishTime(DateTime t, GameObject racer)
    {
        for(int i = 0; i < racers.Count; i++)
        {
            // if this is the one we're looking for
            if(racers[i].Name == racer.GetComponent<SphereController>().SphereName)
            {
                racers[i].finishTime = t;
            }
        }
    }
}