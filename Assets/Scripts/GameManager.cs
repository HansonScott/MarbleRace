using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkedVar;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerSphereController playerPrefab;
    [SerializeField] private AISphereController aiPrefab;
    [SerializeField] private PlayerCameraController playerCameraControllerPrefab;
    private FinishLineHandler finishLinePrefab;

    private GameStates _gameState = GameStates.TITLE;
    public GameStates CurrentGameState
    {
        get { return _gameState; }
    }
    public void SetCurrentGameState(GameStates s)
    {
        _gameState = s;
        UIManager.Instance.UpdateUIFromGameState(_gameState);
    }

    public Race CurrentRace { get; private set; }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    #region State changes from menu
    public void MainMenu()
    {
        SetCurrentGameState(GameStates.INTRO);
    }
    public void HostGame()
    {
        SetCurrentGameState(GameStates.HOSTING);

        // populate the host's IP address
        UIManager.Instance.SetIPValue(GetHostIPAddress());
    }
    public void JoinGame()
    {
        SetCurrentGameState(GameStates.JOINING);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        bool isHosting = (CurrentGameState == GameStates.HOSTING);

        // start the network host (join?)
        if (isHosting)
        {
            // start network host
            // To Do: need to shut this down when the race/scene ends.
            NetworkingManager.Singleton.StartHost();

            SetCurrentGameState(GameStates.STARTING); // SERVER
        }
        else
        {
            // capture network IP address

            // set the IP into the network UNetTransport.connectAddress variable

            // start network client
            NetworkingManager.Singleton.StartClient();

            // state should get pulled from server from this point on...
        }

        CreateNewRace(); // SERVER
        StartNewRaceScene(); // SERVER and CLIENT
    }

    private void CreateNewRace()
    {
        // RUNS ON CLIENT AND SERVER (need to decouple)

        // gather general race info
        RaceInfo ri = UIManager.Instance.GetRaceInfo(); // CLIENT
        Race thisRace = GameManager.Instance.CreateNewRaceFromUIInfo(ri); //SERVER

        // gather settings and add this player
        PlayerInfo pi = UIManager.Instance.GetPlayerInfo(); // CLIENT
        Racer thisRacer = new Racer(pi.Name, pi.Color, pi.Shine, pi.Reflect, true); // CLIENT
        thisRace.AddPlayer(thisRacer); // SERVER

        GameManager.Instance.CurrentRace = thisRace; // SERVER
    }
    #endregion

    private string GetHostIPAddress()
    {
        // runs on CLIENT

        IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
        return ipHostEntry.AddressList[ipHostEntry.AddressList.Length - 1].ToString(); // get the last one (?)
    }

    #region Starting a new race
    [ServerRPC]
    internal Race CreateNewRaceFromUIInfo(RaceInfo info)
    {
        Race r = new Race(info.TrackID, info.RaceTypeID, info.LapCount, info.StartDelay); // SERVER

        for (int i = 0; i < info.AICount; i++) // SERVER
        {
            r.AddPlayer(); // SERVER
        }

        CurrentRace = r; // SERVER
        return r; // SERVER
    }
    internal void StartNewRaceScene()
    {
        CurrentRace.ResetFinishTimes(); // SERVER

        // scene manager to load and show this race
        AsyncOperation levelLoading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync( // CLIENT
        CurrentRace.SceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive); // CLIENT

        // this will finish the loading of the level and all subsequent details
        levelLoading.completed += LevelLoading_completed; // CLIENT
    }
    private void LevelLoading_completed(AsyncOperation obj)
    {
        // now that the scene has loaded, then we can add players to it
        Scene levelScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(CurrentRace.SceneName); // SERVER

        // set this scene as active, so we can add players to it.
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(levelScene); // SERVER

        List<GameObject> playersToAdjust = AddPlayerObjectsToRaceScene(); // SERVER
        AdjustRaceBasedOnRaceType(playersToAdjust); // SERVER

        // hook up the finish line event trigger
        finishLinePrefab = GameObject.FindObjectOfType<FinishLineHandler>(); // SERVER
        finishLinePrefab.onPlayerFinished.AddListener(FinishLineHit); // SERVER

        // hook up the start delay timer
        TrackManager tm = GameObject.FindObjectOfType<TrackManager>(); // SERVER
        tm.StartDelay = CurrentRace.StartDelay; // SERVER and CLIENT

        SetCurrentGameState(GameStates.STARTING); // SERVER and CLIENT

        // start the UI countdown once everthing has loaded.
        UIManager.Instance.StartCountDown(CurrentRace.StartDelay); // CLIENT
    }

    [ServerRPC]
    private void AdjustRaceBasedOnRaceType(List<GameObject> playersToAdjust)
    {
        if (CurrentRace.RaceType == RaceTypes.PureSpeed)
        {
            // for the pure speed race type, disable all collisions between players
            for (int i = 0; i < playersToAdjust.Count; i++)
            {
                for (int j = 0; j < playersToAdjust.Count; j++)
                {
                    if (i == j) { continue; } //don't bother with self

                    Physics.IgnoreCollision(playersToAdjust[i].GetComponent<Collider>(),
                                            playersToAdjust[j].GetComponent<Collider>(), true);
                }
            }
        }
    }

    [ServerRPC]
    private List<GameObject> AddPlayerObjectsToRaceScene()
    {
        // reference all the game objects, for the collision adjustment depending on track type
        List<GameObject> playersToAdjust = new List<GameObject>();

        // place all players in starting area of the race
        for (int i = 0; i < CurrentRace.racers.Count; i++)
        {
            Racer r = CurrentRace.racers[i];

            if (r.isUserControlled)
            {
                //PlayerSphereController player = (PlayerSphereController)UnityEditor.PrefabUtility.InstantiatePrefab(playerPrefab, levelScene);
                PlayerSphereController player = GameObject.Instantiate<PlayerSphereController>(playerPrefab);
                // NOTE: add the server version of this too (keyword: spawn)
                player.gameObject.SetActive(true); // SERVER
                r.Sphere = player.gameObject; // SERVER
                player.SphereName = r.Name; // SERVER

                // link references to camera, both ways
                // before adding players, we need to add the player camera
                PlayerCameraController cam = GameObject.Instantiate<PlayerCameraController>(playerCameraControllerPrefab);  // CLIENT
                player.playerCameraFocalPoint = cam; // CLIENT
                cam.Player = player.gameObject; // CLIENT

                // apply our player settings
                r.ApplyAppearanceToGameObject(player.gameObject); // SERVER

                // set location to start location
                SetStartingPosition(player, CurrentRace.StartingPosition, i); // SERVER

                // store temporarily for adjustments as a group
                playersToAdjust.Add(player.gameObject); // SERVER
            }
            else
            {
                //AISphereController player = (AISphereController)UnityEditor.PrefabUtility.InstantiatePrefab(aiPrefab, levelScene);
                AISphereController player = GameObject.Instantiate<AISphereController>(aiPrefab); // SERVER
                // NOTE: add the server version of this too (keyword: spawn)
                r.ApplyAppearanceToGameObject(player.gameObject); // SERVER
                player.gameObject.SetActive(true); // SERVER
                r.Sphere = player.gameObject; // SERVER

                player.SphereName = r.Name; // SERVER

                // set location to start location
                SetStartingPosition(player, CurrentRace.StartingPosition, i); // SERVER

                // store temporarily for adjustments as a group
                playersToAdjust.Add(player.gameObject); // SERVER
            }
        } // end for

        return playersToAdjust;
    }

    [ServerRPC]
    private void SetStartingPosition(SphereController player, Vector3 trackStartingPosition, int positionOrder)
    {
        Vector3 actualStartingPosition = trackStartingPosition;

        if (CurrentRace.RaceType != RaceTypes.PureSpeed)
        {
            // defaults in first place
            if (positionOrder > 0)
            {
                float sphereWidth = 1;

                while (positionOrder > 10)
                {
                    // wrap around to the next row
                    actualStartingPosition.z -= sphereWidth;
                    positionOrder -= 10;
                }

                float xMove = (sphereWidth * ((positionOrder + 1) / 2));
                if (positionOrder % 2 == 1) { xMove = -xMove; } // switch sides every other postion
                actualStartingPosition.x += xMove;
            }
        }
        player.transform.position = actualStartingPosition;
    }
    #endregion

    #region Finishing a Lap/Race
    [ServerRPC]
    private void FinishLineHit(GameObject go)
    {
        SphereController sc = go.GetComponent<SphereController>();
        if(sc != null)
        {
            CurrentRace.SetFinishTime(DateTime.Now, go);
            sc.FinishTime = DateTime.Now;

            Debug.Log(sc.SphereName + " finished in " +
                (sc.FinishTime - CurrentRace.StartTime).ToString(UIManager.TIME_FORMAT));
        }

        if (CurrentRace.IsLapComplete)
        {
            SetCurrentGameState(GameStates.REVIEWING);

            // sort the racers by their finishTime
            CurrentRace.racers.Sort((r1, r2) => r1.finishTime.CompareTo(r2.finishTime));

            UIManager.Instance.PopulateResultsScreen();
        }

        // race completion is handled in the 'next button' functionality
    }

    public void NextButtonPressed()
    {
        // advance the lap
        CurrentRace.AdanceLap();

        // clear out results grid
        UIManager.Instance.ClearResultsScreen();

        // unload the level, regardless of the outcome
        UnloadCurrentLevel();

        // if we're not done, then figure out next lap
        if (!CurrentRace.isRaceComplete)
        {
            // calculate players to remove from the list
            int remove = CalculateRemoval(CurrentRace.TotalLaps, CurrentRace.CurrentLap, CurrentRace.racers.Count);
            CurrentRace.RemoveSlowestPlayers(remove);

            if (CurrentRace.IsPlayerAmongRacers())
            {
                // restart the scene for the next lap
                StartNewRaceScene();
                return;
            }
            else
            {
                Debug.Log("Player has been eliminated.  Try again next time!");
            }
        }

        // then we have failed or it's the end of this race entirely.
        ClearCurrentRace();
        MainMenu();
    }
    private void UnloadCurrentLevel()
    {
        // unload the previous track
        AsyncOperation levelUnLoading = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(CurrentRace.SceneName);

        // this will finish the unloading of the level and all subsequent details
        levelUnLoading.completed += LevelUnLoading_completed;
    }

    private void LevelUnLoading_completed(AsyncOperation obj)
    {
    }

    private void ClearCurrentRace()
    {
        this.CurrentRace = null;
    }

    public int CalculateRemoval(int totalLaps, int currentLap, int playerCount)
    {
        // theory - linearly remove some on each lap until 5 are left on last lap
        //int lapsLeft = totalLaps - currentLap;
        //int playersToRemove = (playerCount - 5);
        //int playersToRemoveThisLap = playersToRemove / lapsLeft;
        //int result = playersToRemoveThisLap;
        //return result;
        
        // catch zero denominator
        if((totalLaps + 1 - currentLap) == 0) { return 0; }

        int result = ((playerCount - 5) / (totalLaps + 1 - currentLap));

        if(result < 0) { result = 0; }

        return result;
    }
    #endregion
}
