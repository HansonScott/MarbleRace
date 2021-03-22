using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private UIManager uiManager;

    [SerializeField] private PlayerSphereController playerPrefab;
    [SerializeField] private AISphereController aiPrefab;
    [SerializeField] private PlayerCameraController playerCameraControllerPrefab;
    private FinishLineHandler finishLinePrefab;

    public Race currentRace;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    internal Race CreateNewRace(int trackID, RaceTypes raceTypeID, int lapCount, float aICount, float startDelay)
    {
        Race r = new Race(trackID, raceTypeID, lapCount, startDelay);

        for (int i = 0; i < aICount; i++)
        {
            r.AddPlayer();
        }

        currentRace = r;
        return r;
    }

    internal void StartRace()
    {
        currentRace.ResetFinishTimes();

        // scene manager to load and show this race
        AsyncOperation levelLoading = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
        currentRace.SceneName, UnityEngine.SceneManagement.LoadSceneMode.Additive);

        // this will finish the loading of the level and all subsequent details
        levelLoading.completed += LevelLoading_completed;
    }

    private void LevelLoading_completed(AsyncOperation obj)
    {
        // now that the scene has loaded, then we can add players to it
        Scene levelScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(currentRace.SceneName);

        // set this scene as active, so we can add players to it.
        UnityEngine.SceneManagement.SceneManager.SetActiveScene(levelScene);

        // reference all the game objects, for the collision adjustment depending on track type
        List<GameObject> playersToAdjust = new List<GameObject>();

        // place all players in starting area of the race
        for (int i = 0; i < currentRace.racers.Count; i++)
        {
            Racer r = currentRace.racers[i];

            if (r.isUserControlled)
            {
                //PlayerSphereController player = (PlayerSphereController)UnityEditor.PrefabUtility.InstantiatePrefab(playerPrefab, levelScene);
                PlayerSphereController player = GameObject.Instantiate<PlayerSphereController>(playerPrefab);
                player.gameObject.SetActive(true);
                r.Sphere = player.gameObject;
                player.SphereName = r.Name;

                // link references to camera, both ways
                // before adding players, we need to add the player camera
                PlayerCameraController cam = GameObject.Instantiate<PlayerCameraController>(playerCameraControllerPrefab);
                player.playerCameraFocalPoint = cam;
                cam.player = player.gameObject;

                // apply our player settings
                r.ApplyAppearanceToGameObject(player.gameObject);

                // set location to start location
                SetStartingPosition(player, currentRace.StartingPosition, i);

                // store temporarily for adjustments as a group
                playersToAdjust.Add(player.gameObject);
            }
            else
            {
                //AISphereController player = (AISphereController)UnityEditor.PrefabUtility.InstantiatePrefab(aiPrefab, levelScene);
                AISphereController player = GameObject.Instantiate<AISphereController>(aiPrefab);
                r.ApplyAppearanceToGameObject(player.gameObject);
                player.gameObject.SetActive(true);
                r.Sphere = player.gameObject;

                player.SphereName = r.Name;

                // set location to start location
                SetStartingPosition(player, currentRace.StartingPosition, i);

                // store temporarily for adjustments as a group
                playersToAdjust.Add(player.gameObject);
            }
        } // end for

        if(currentRace.RaceType == RaceTypes.PureSpeed)
        {
            // for the pure speed race type, disable all collisions between players
            for(int i = 0; i < playersToAdjust.Count; i++)
            {
                for (int j = 0; j < playersToAdjust.Count; j++)
                {
                    if(i == j) { continue; } //don't bother with self

                    Physics.IgnoreCollision(playersToAdjust[i].GetComponent<Collider>(),
                                            playersToAdjust[j].GetComponent<Collider>(), true);
                }
            }
        }

        // hook up the finish line event trigger
        finishLinePrefab = GameObject.FindObjectOfType<FinishLineHandler>();
        finishLinePrefab.onPlayerFinished.AddListener(FinishLineHit);

        // hook up the start delay timer
        TrackManager tm = GameObject.FindObjectOfType<TrackManager>();
        tm.StartDelay = currentRace.StartDelay;

        UIManager.Instance.CurrentGameState = GameStates.STARTING;

        // start the UI countdown once everthing has loaded.
        UIManager.Instance.StartCountDown(currentRace.StartDelay);
    }

    private void SetStartingPosition(SphereController player, Vector3 trackStartingPosition, int positionOrder)
    {
        Vector3 actualStartingPosition = trackStartingPosition;

        if (currentRace.RaceType != RaceTypes.PureSpeed)
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

    private void FinishLineHit(GameObject go)
    {
        SphereController sc = go.GetComponent<SphereController>();
        if(sc != null)
        {
            currentRace.SetFinishTime(DateTime.Now, go);
            sc.FinishTime = DateTime.Now;

            Debug.Log(sc.SphereName + " finished in " +
                (sc.FinishTime - currentRace.StartTime).ToString(UIManager.TIME_FORMAT));
        }

        if (currentRace.IsLapComplete)
        {
            UIManager.Instance.CurrentGameState = GameStates.REVIEWING;

            // sort the racers by their finishTime
            currentRace.racers.Sort((r1, r2) => r1.finishTime.CompareTo(r2.finishTime));

            UIManager.Instance.PopulateResultsScreen();
        }

        // race completion is handled in the 'next button' functionality
    }

    public void NextButtonPressed()
    {
        // advance the lap
        currentRace.AdanceLap();

        // clear out results grid
        UIManager.Instance.ClearResultsScreen();

        // unload the level, regardless of the outcome
        UnloadCurrentLevel();

        // if we're not done, then figure out next lap
        if (!currentRace.isRaceComplete)
        {
            // calculate players to remove from the list
            int remove = CalculateRemoval(currentRace.TotalLaps, currentRace.CurrentLap, currentRace.racers.Count);
            currentRace.RemoveSlowestPlayers(remove);

            if (currentRace.PlayerAmongRacers())
            {
                // restart the scene for the next lap
                StartRace();
                return;
            }
            else
            {
                Debug.Log("Player has been eliminated.  Try again next time!");
            }
        }

        // then we have failed or it's the end of this race entirely.
        ClearCurrentRace();
        UIManager.Instance.MainMenu();
    }
    private void UnloadCurrentLevel()
    {
        // unload the previous track
        AsyncOperation levelUnLoading = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentRace.SceneName);

        // this will finish the unloading of the level and all subsequent details
        levelUnLoading.completed += LevelUnLoading_completed;
    }

    private void LevelUnLoading_completed(AsyncOperation obj)
    {
    }

    private void ClearCurrentRace()
    {
        this.currentRace = null;
    }

    private int CalculateRemoval(int totalLaps, int currentLap, int playerCount)
    {
        // theory - linearly remove some on each lap until 5 are left on last lap
        //int lapsLeft = totalLaps - currentLap;
        //int playersToRemove = (playerCount - 5);
        //int playersToRemoveThisLap = playersToRemove / lapsLeft;
        //int result = playersToRemoveThisLap;
        //return result;

        return ((playerCount - 5) / (totalLaps + 1 - currentLap));
    }
}
