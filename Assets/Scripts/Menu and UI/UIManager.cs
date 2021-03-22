using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public enum GameStates
{
    TITLE,
    INTRO,
    HOSTING,
    JOINING,
    STARTING,
    PLAYING,
    PAUSED,
    REVIEWING
}

public class UIManager : Singleton<UIManager>
{
    private GameStates _gameState = GameStates.TITLE;
    public GameStates CurrentGameState
    {
        get { return _gameState; }
        set { 
            _gameState = value; 
            UpdateUIFromGameState(_gameState); 
        }
    }

    [SerializeField] private GameManager gameManager;

    private float currentTimerValue;

    public const string TIME_FORMAT = "mm\\:ss\\:ff";

    #region Menu Controls
    // main menu
    [SerializeField] private GameObject backgroundCube;
    [SerializeField] private UnityEngine.UI.Button hostButton;
    [SerializeField] private UnityEngine.UI.Button joinButton;
    [SerializeField] private UnityEngine.UI.Button quitButton;

    // host
    [SerializeField] private TMPro.TextMeshProUGUI raceTypeLabel;
    [SerializeField] private TMPro.TMP_Dropdown raceTypeDropDown;
    [SerializeField] private TMPro.TextMeshProUGUI racetrackLabel;
    [SerializeField] private TMPro.TMP_Dropdown racetrackDropDown;
    [SerializeField] private TMPro.TextMeshProUGUI roundCountLabel;
    [SerializeField] private UnityEngine.UI.Slider roundCountSlider;
    [SerializeField] private TMPro.TextMeshProUGUI aiCountLabel;
    [SerializeField] private UnityEngine.UI.Slider aiCountSlider;
    [SerializeField] private TMPro.TextMeshProUGUI allowJoinLabel;
    [SerializeField] private UnityEngine.UI.Toggle allowJoinToggle;
    [SerializeField] private TMPro.TextMeshProUGUI delayStartLabel;
    [SerializeField] private UnityEngine.UI.Slider delayStartSlider;

    // join?

    // my player settings
    [SerializeField] private TMPro.TextMeshProUGUI myNameLabel;
    [SerializeField] private TMPro.TMP_InputField myNameInputField;
    [SerializeField] private TMPro.TextMeshProUGUI myColorLabel;
    [SerializeField] private ColorPicker myColorPicker;
    [SerializeField] private GameObject myPlayerSphere;
    [SerializeField] private TMPro.TextMeshProUGUI myShineLabel;
    [SerializeField] private UnityEngine.UI.Slider myShinySlider;
    [SerializeField] private TMPro.TextMeshProUGUI myReflectLabel;
    [SerializeField] private UnityEngine.UI.Slider myReflectSlider;

    [SerializeField] private UnityEngine.UI.Button backToMainMenuButton;
    [SerializeField] private UnityEngine.UI.Button startButton;

    // playing
    [SerializeField] private TMPro.TextMeshProUGUI TrackHUD_CountDownLabel;
    [SerializeField] private Animator CountDownAnimator;

    // results
    [SerializeField] private TMPro.TextMeshProUGUI resultsTitle;
    [SerializeField] private TMPro.TextMeshProUGUI resultsTrack;
    [SerializeField] private TMPro.TextMeshProUGUI resultsLapXofY;
    [SerializeField] private TMPro.TextMeshProUGUI resultsType;
    [SerializeField] private UnityEngine.UI.ScrollRect resultsGrid;
    [SerializeField] private GameObject resultsRow;
    [SerializeField] private UnityEngine.UI.Button nextButton;

    #endregion

    private void UpdateUIFromGameState(GameStates gs)
    {
        switch(gs)
        {
            case GameStates.TITLE:
                break;
            case GameStates.INTRO:
                backgroundCube.SetActive(true);

                hostButton.gameObject.SetActive(true);
                joinButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(true);

                raceTypeLabel.gameObject.SetActive(false);
                raceTypeDropDown.gameObject.SetActive(false);
                racetrackLabel.gameObject.SetActive(false);
                racetrackDropDown.gameObject.SetActive(false);
                roundCountLabel.gameObject.SetActive(false);
                roundCountSlider.gameObject.SetActive(false);
                aiCountLabel.gameObject.SetActive(false);
                aiCountSlider.gameObject.SetActive(false);
                allowJoinLabel.gameObject.SetActive(false);
                allowJoinToggle.gameObject.SetActive(false);
                delayStartLabel.gameObject.SetActive(false);
                delayStartSlider.gameObject.SetActive(false);

                myNameLabel.gameObject.SetActive(false);
                myNameInputField.gameObject.SetActive(false);
                myColorLabel.gameObject.SetActive(false);
                myColorPicker.gameObject.SetActive(false);
                myPlayerSphere.gameObject.SetActive(false);
                myPlayerSphere.SetActive(false);
                myShineLabel.gameObject.SetActive(false);
                myShinySlider.gameObject.SetActive(false);
                myReflectLabel.gameObject.SetActive(false);
                myReflectSlider.gameObject.SetActive(false);

                backToMainMenuButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(false);

                TrackHUD_CountDownLabel.gameObject.SetActive(false);

                resultsTitle.gameObject.SetActive(false);
                resultsTrack.gameObject.SetActive(false);
                resultsLapXofY.gameObject.SetActive(false);
                resultsType.gameObject.SetActive(false);
                resultsGrid.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);

                break;
            case GameStates.HOSTING:
                backgroundCube.SetActive(true);

                hostButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);

                raceTypeLabel.gameObject.SetActive(true);
                raceTypeDropDown.gameObject.SetActive(true);
                racetrackLabel.gameObject.SetActive(true);
                racetrackDropDown.gameObject.SetActive(true);
                roundCountLabel.gameObject.SetActive(true);
                roundCountSlider.gameObject.SetActive(true);
                aiCountLabel.gameObject.SetActive(true);
                aiCountSlider.gameObject.SetActive(true);
                allowJoinLabel.gameObject.SetActive(true);
                allowJoinToggle.gameObject.SetActive(true);
                delayStartLabel.gameObject.SetActive(true);
                delayStartSlider.gameObject.SetActive(true);

                myNameLabel.gameObject.SetActive(true);
                myNameInputField.gameObject.SetActive(true);
                myColorLabel.gameObject.SetActive(true);
                myColorPicker.gameObject.SetActive(true);
                myPlayerSphere.gameObject.SetActive(true);
                myPlayerSphere.SetActive(true);
                myShineLabel.gameObject.SetActive(true);
                myShinySlider.gameObject.SetActive(true);
                myReflectLabel.gameObject.SetActive(true);
                myReflectSlider.gameObject.SetActive(true);

                backToMainMenuButton.gameObject.SetActive(true);
                startButton.gameObject.SetActive(true);

                TrackHUD_CountDownLabel.gameObject.SetActive(false);

                resultsTitle.gameObject.SetActive(false);
                resultsTrack.gameObject.SetActive(false);
                resultsLapXofY.gameObject.SetActive(false);
                resultsType.gameObject.SetActive(false);
                resultsGrid.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);

                break;
            case GameStates.JOINING:
                backgroundCube.SetActive(true);

                hostButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);

                raceTypeLabel.gameObject.SetActive(false);
                raceTypeDropDown.gameObject.SetActive(false);
                racetrackLabel.gameObject.SetActive(false);
                racetrackDropDown.gameObject.SetActive(false);
                roundCountLabel.gameObject.SetActive(false);
                roundCountSlider.gameObject.SetActive(false);
                aiCountLabel.gameObject.SetActive(false);
                aiCountSlider.gameObject.SetActive(false);
                allowJoinLabel.gameObject.SetActive(false);
                allowJoinToggle.gameObject.SetActive(false);
                delayStartLabel.gameObject.SetActive(false);
                delayStartSlider.gameObject.SetActive(false);

                myNameLabel.gameObject.SetActive(true);
                myNameInputField.gameObject.SetActive(true);
                myColorLabel.gameObject.SetActive(true);
                myColorPicker.gameObject.SetActive(true);
                myPlayerSphere.gameObject.SetActive(true);
                myPlayerSphere.SetActive(true);
                myShineLabel.gameObject.SetActive(true);
                myShinySlider.gameObject.SetActive(true);
                myReflectLabel.gameObject.SetActive(true);
                myReflectSlider.gameObject.SetActive(true);

                backToMainMenuButton.gameObject.SetActive(true);
                startButton.gameObject.SetActive(true);

                TrackHUD_CountDownLabel.gameObject.SetActive(false);

                resultsTitle.gameObject.SetActive(false);
                resultsTrack.gameObject.SetActive(false);
                resultsLapXofY.gameObject.SetActive(false);
                resultsType.gameObject.SetActive(false);
                resultsGrid.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);

                break;
            case GameStates.STARTING:
                backgroundCube.SetActive(false);

                hostButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);

                raceTypeLabel.gameObject.SetActive(false);
                raceTypeDropDown.gameObject.SetActive(false);
                racetrackLabel.gameObject.SetActive(false);
                racetrackDropDown.gameObject.SetActive(false);
                roundCountLabel.gameObject.SetActive(false);
                roundCountSlider.gameObject.SetActive(false);
                aiCountLabel.gameObject.SetActive(false);
                aiCountSlider.gameObject.SetActive(false);
                allowJoinLabel.gameObject.SetActive(false);
                allowJoinToggle.gameObject.SetActive(false);
                delayStartLabel.gameObject.SetActive(false);
                delayStartSlider.gameObject.SetActive(false);

                myNameLabel.gameObject.SetActive(false);
                myNameInputField.gameObject.SetActive(false);
                myColorLabel.gameObject.SetActive(false);
                myColorPicker.gameObject.SetActive(false);
                myPlayerSphere.gameObject.SetActive(false);
                myPlayerSphere.SetActive(false);
                myShineLabel.gameObject.SetActive(false);
                myShinySlider.gameObject.SetActive(false);
                myReflectLabel.gameObject.SetActive(false);
                myReflectSlider.gameObject.SetActive(false);

                backToMainMenuButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(false);

                TrackHUD_CountDownLabel.gameObject.SetActive(true);

                resultsTitle.gameObject.SetActive(false);
                resultsTrack.gameObject.SetActive(false);
                resultsLapXofY.gameObject.SetActive(false);
                resultsType.gameObject.SetActive(false);
                resultsGrid.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);

                break;
            case GameStates.PLAYING:
                backgroundCube.SetActive(false);

                hostButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);

                raceTypeLabel.gameObject.SetActive(false);
                raceTypeDropDown.gameObject.SetActive(false);
                racetrackLabel.gameObject.SetActive(false);
                racetrackDropDown.gameObject.SetActive(false);
                roundCountLabel.gameObject.SetActive(false);
                roundCountSlider.gameObject.SetActive(false);
                aiCountLabel.gameObject.SetActive(false);
                aiCountSlider.gameObject.SetActive(false);
                allowJoinLabel.gameObject.SetActive(false);
                allowJoinToggle.gameObject.SetActive(false);
                delayStartLabel.gameObject.SetActive(false);
                delayStartSlider.gameObject.SetActive(false);

                myNameLabel.gameObject.SetActive(false);
                myNameInputField.gameObject.SetActive(false);
                myColorLabel.gameObject.SetActive(false);
                myColorPicker.gameObject.SetActive(false);
                myPlayerSphere.gameObject.SetActive(false);
                myPlayerSphere.SetActive(false);
                myShineLabel.gameObject.SetActive(false);
                myShinySlider.gameObject.SetActive(false);
                myReflectLabel.gameObject.SetActive(false);
                myReflectSlider.gameObject.SetActive(false);

                backToMainMenuButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(false);

                TrackHUD_CountDownLabel.gameObject.SetActive(true);

                resultsTitle.gameObject.SetActive(false);
                resultsTrack.gameObject.SetActive(false);
                resultsLapXofY.gameObject.SetActive(false);
                resultsType.gameObject.SetActive(false);
                resultsGrid.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(false);

                break;
            case GameStates.PAUSED:
                break;
            case GameStates.REVIEWING:
                backgroundCube.SetActive(true);

                hostButton.gameObject.SetActive(false);
                joinButton.gameObject.SetActive(false);
                quitButton.gameObject.SetActive(false);

                raceTypeLabel.gameObject.SetActive(false);
                raceTypeDropDown.gameObject.SetActive(false);
                racetrackLabel.gameObject.SetActive(false);
                racetrackDropDown.gameObject.SetActive(false);
                roundCountLabel.gameObject.SetActive(false);
                roundCountSlider.gameObject.SetActive(false);
                aiCountLabel.gameObject.SetActive(false);
                aiCountSlider.gameObject.SetActive(false);
                allowJoinLabel.gameObject.SetActive(false);
                allowJoinToggle.gameObject.SetActive(false);
                delayStartLabel.gameObject.SetActive(false);
                delayStartSlider.gameObject.SetActive(false);

                myNameLabel.gameObject.SetActive(false);
                myNameInputField.gameObject.SetActive(false);
                myColorLabel.gameObject.SetActive(false);
                myColorPicker.gameObject.SetActive(false);
                myPlayerSphere.gameObject.SetActive(false);
                myPlayerSphere.SetActive(false);
                myShineLabel.gameObject.SetActive(false);
                myShinySlider.gameObject.SetActive(false);
                myReflectLabel.gameObject.SetActive(false);
                myReflectSlider.gameObject.SetActive(false);

                backToMainMenuButton.gameObject.SetActive(false);
                startButton.gameObject.SetActive(false);

                TrackHUD_CountDownLabel.gameObject.SetActive(false);

                resultsTitle.gameObject.SetActive(true);
                resultsTrack.gameObject.SetActive(true);
                resultsLapXofY.gameObject.SetActive(true);
                resultsType.gameObject.SetActive(true);
                resultsGrid.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(true);

                break;
            default:
                Debug.LogError("Game state unrecognized! Unable to update the UI.");
                break;
        }
    }

    internal void PopulateResultsScreen()
    {
        // populate the track name, type laps
        resultsTrack.text = GameManager.Instance.currentRace.SceneName;
        resultsType.text = GameManager.Instance.currentRace.RaceType.ToString();
        resultsLapXofY.text = "Lap " + GameManager.Instance.currentRace.CurrentLap + " of " + GameManager.Instance.currentRace.TotalLaps;


        int verticalAlignment = 250;
        int rowHeight = -40;
        
        int position = 1;

        int remove = GameManager.Instance.CalculateRemoval(GameManager.Instance.currentRace.TotalLaps, 
                                                            GameManager.Instance.currentRace.CurrentLap + 1, 
                                                            GameManager.Instance.currentRace.racers.Count);

        // for each racer
        for(int i = 0; i < GameManager.Instance.currentRace.racers.Count; i++)
        {
            Racer r = GameManager.Instance.currentRace.racers[i];

            // instantiate a prefab row into the boot scene (same)
            GameObject newRow = Instantiate(resultsRow);

            // assign the row to the grid as a child
            Transform viewport = resultsGrid.transform.GetChild(0);
            Transform content = viewport.GetChild(0);

            newRow.transform.SetParent(content);

            // align the row to be precisely placed below previous ones
            newRow.transform.position = new Vector3(470, verticalAlignment, 0);

            // and set the alignment for the next one
            verticalAlignment += rowHeight;

            // populate the prefab row with position, name, and time.
            Transform t1 = newRow.transform.GetChild(0); // position
            Transform t2 = newRow.transform.GetChild(1); // name
            Transform t3 = newRow.transform.GetChild(2); // time


            TextMeshProUGUI positionLabel = t1.gameObject.GetComponent<TextMeshProUGUI>();
            positionLabel.text = position.ToString();
            position++;

            TextMeshProUGUI nameLabel = t2.gameObject.GetComponent<TextMeshProUGUI>();
            nameLabel.text = r.Name;

            TextMeshProUGUI timeLabel = t3.gameObject.GetComponent<TextMeshProUGUI>();
            timeLabel.text = (r.finishTime - GameManager.Instance.currentRace.StartTime).ToString(TIME_FORMAT);

            bool shouldShowRed = (i >= (GameManager.Instance.currentRace.racers.Count - remove));
            if (shouldShowRed)
            {
                positionLabel.color = Color.red;
                nameLabel.color = Color.red;
                timeLabel.color = Color.red;
            }
        }
    }

    internal void ClearResultsScreen()
    {
        // assign the row to the grid as a child
        Transform viewport = resultsGrid.transform.GetChild(0);
        Transform content = viewport.GetChild(0);
        GameObject[] rows = GameObject.FindGameObjectsWithTag("ResultsGridRow");
        for(int i = 0; i < rows.Length; i++)
        {
            GameObject.Destroy(rows[i]);
        }        
    }

    #region public gameState changes
    public void MainMenu()
    {
        CurrentGameState = GameStates.INTRO;
    }

    public void HostGame()
    {
        CurrentGameState = GameStates.HOSTING;
    }

    public void JoinGame()
    {
        CurrentGameState = GameStates.JOINING;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        CurrentGameState = GameStates.STARTING;

        // load settings for the game
        int trackID = racetrackDropDown.value;
        int raceTypeID = raceTypeDropDown.value;
        int lapCount = (int)roundCountSlider.value;
        float AICount = aiCountSlider.value;
        float delay = delayStartSlider.value;

        Race thisRace = GameManager.Instance.CreateNewRace(trackID, (RaceTypes)Enum.Parse(typeof(RaceTypes), raceTypeID.ToString()), lapCount, AICount, delay);
        GameManager.Instance.currentRace = thisRace;

        // capture this player settings
        Racer thisRacer = new Racer(myNameInputField.text, myColorPicker.color, myShinySlider.value, myReflectSlider.value, true);

        thisRace.AddPlayer(thisRacer);

        GameManager.Instance.StartRace();
    }
    #endregion

    public override void Start()
    {
        // because this is a child class, make sure to call the parent first.
        base.Start();

        // now we can do things at our class level.
        myColorPicker.onColorChanged += MyColorPicker_onColorChanged;
        myShinySlider.onValueChanged.AddListener(delegate { MyShinySlider_ValueChanged(); });
        myReflectSlider.onValueChanged.AddListener(delegate { MyReflectSlider_ValueChanged(); });

        CountDownAnimator.SetBool("ShouldShrink", false);

        LoadPlayerPrefs();
    }

    private void LoadPlayerPrefs()
    {
        // Future: if we know what the player's name and color preferences were, load them.
    }

    #region MyLook event handlers
    // Invoked when the value of the slider changes.
    public void MyShinySlider_ValueChanged()
    {
        myPlayerSphere.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", myShinySlider.value);
    }
    // Invoked when the value of the slider changes.
    public void MyReflectSlider_ValueChanged()
    {
        myPlayerSphere.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", myReflectSlider.value);
    }
    private void MyColorPicker_onColorChanged(Color obj)
    {
        myPlayerSphere.GetComponent<MeshRenderer>().material.color = obj;
    }
    #endregion

    internal void StartCountDown(float timeInSeconds)
    {
        currentTimerValue = timeInSeconds;

        StartCoroutine(UpdateCountdown());
    }

    private IEnumerator UpdateCountdown()
    {
        TrackHUD_CountDownLabel.gameObject.SetActive(true);

        while (currentTimerValue >= 0)
        {
            // update UI label with new value
            TrackHUD_CountDownLabel.text = currentTimerValue.ToString();

            // and wait a second before doing it again.
            currentTimerValue--;

            if(currentTimerValue == 0)
            {
                CountDownAnimator.SetBool("ShouldShrink", true);
            }

            yield return new WaitForSeconds(1);
        }

        CurrentGameState = GameStates.PLAYING;
    }

    private void Update()
    {
        if(CurrentGameState == GameStates.PLAYING)
        {
            TrackHUD_CountDownLabel.text = (DateTime.Now - GameManager.Instance.currentRace.StartTime)
                                            .ToString(TIME_FORMAT);
        }
    }
}
