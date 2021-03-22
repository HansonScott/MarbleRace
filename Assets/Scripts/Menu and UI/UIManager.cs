using System;
using System.Collections;
using UnityEngine;
using TMPro;

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
    public const string TIME_FORMAT = "mm\\:ss\\:ff";
    private float _currentTimerValue;

    #region Menu Controls and Navigation
    // main menu
    [SerializeField] private GameObject mainMenuCanvas;
    [SerializeField] private GameObject backgroundCube;

    // host Screen
    [SerializeField] private GameObject hostScreen;
    [SerializeField] private TMPro.TMP_Dropdown raceTypeDropDown;
    [SerializeField] private TMPro.TMP_Dropdown racetrackDropDown;
    [SerializeField] private UnityEngine.UI.Slider roundCountSlider;
    [SerializeField] private UnityEngine.UI.Slider aiCountSlider;
    [SerializeField] private TMPro.TextMeshProUGUI hostIPValueLabel;
    [SerializeField] private UnityEngine.UI.Slider delayStartSlider;

    // my player settings
    [SerializeField] private GameObject myPlayerSettings;
    [SerializeField] private TMPro.TMP_InputField myNameInputField;
    [SerializeField] private ColorPicker myColorPicker;
    [SerializeField] private GameObject myPlayerSphere;
    [SerializeField] private UnityEngine.UI.Slider myShinySlider;
    [SerializeField] private UnityEngine.UI.Slider myReflectSlider;

    [SerializeField] private UnityEngine.UI.Button backToMainMenuButton;
    [SerializeField] private UnityEngine.UI.Button startButton;

    // join
    [SerializeField] private GameObject joinScreen;
    [SerializeField] private TMPro.TMP_InputField joinIPField;

    // playing
    [SerializeField] private TMPro.TextMeshProUGUI TrackHUD_CountDownLabel;
    [SerializeField] private Animator CountDownAnimator;

    // results
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private TMPro.TextMeshProUGUI resultsTitle;
    [SerializeField] private TMPro.TextMeshProUGUI resultsTrack;
    [SerializeField] private TMPro.TextMeshProUGUI resultsLapXofY;
    [SerializeField] private TMPro.TextMeshProUGUI resultsType;
    [SerializeField] private UnityEngine.UI.ScrollRect resultsGrid;

    internal RaceInfo GetRaceInfo()
    {
        RaceInfo result = new RaceInfo();
        result.TrackID = racetrackDropDown.value;
        result.RaceTypeID = (RaceTypes)Enum.Parse(typeof(RaceTypes), raceTypeDropDown.value.ToString());
        result.LapCount = (int)roundCountSlider.value;
        result.AICount = aiCountSlider.value;
        result.StartDelay = delayStartSlider.value;

        return result;
    }

    internal PlayerInfo GetPlayerInfo()
    {
        PlayerInfo result = new PlayerInfo();
        result.Name = myNameInputField.text;
        result.Color = myColorPicker.color;
        result.Shine = myShinySlider.value;
        result.Reflect = myReflectSlider.value;

        return result;
    }

    [SerializeField] private GameObject resultsRow;

    internal void SetIPValue(string value)
    {
        hostIPValueLabel.text = value;
    }

    public void UpdateUIFromGameState(GameStates gs)
    {
        switch(gs)
        {
            case GameStates.TITLE:
                backgroundCube.SetActive(false);
                mainMenuCanvas.SetActive(false);
                hostScreen.SetActive(false);
                myPlayerSettings.SetActive(false);
                myPlayerSphere.SetActive(false);
                joinScreen.SetActive(false);
                TrackHUD_CountDownLabel.gameObject.SetActive(false);
                resultsScreen.SetActive(false);
                break;
            case GameStates.INTRO:
                backgroundCube.SetActive(true);
                mainMenuCanvas.SetActive(true);
                hostScreen.SetActive(false);
                myPlayerSettings.SetActive(false);
                myPlayerSphere.SetActive(false);
                joinScreen.SetActive(false);
                TrackHUD_CountDownLabel.gameObject.SetActive(false);
                resultsScreen.SetActive(false);
                break;
            case GameStates.HOSTING:
                backgroundCube.SetActive(true);
                mainMenuCanvas.SetActive(false);
                hostScreen.SetActive(true);
                myPlayerSettings.SetActive(true);
                myPlayerSphere.SetActive(true);
                joinScreen.SetActive(false);
                TrackHUD_CountDownLabel.gameObject.SetActive(false);
                resultsScreen.SetActive(false);
                break;
            case GameStates.JOINING:
                backgroundCube.SetActive(true);
                mainMenuCanvas.SetActive(false);
                hostScreen.SetActive(false);
                myPlayerSettings.SetActive(true);
                myPlayerSphere.SetActive(true);
                joinScreen.SetActive(true);
                TrackHUD_CountDownLabel.gameObject.SetActive(false);
                resultsScreen.SetActive(false);
                break;
            case GameStates.STARTING:
                backgroundCube.SetActive(false);
                mainMenuCanvas.SetActive(false);
                hostScreen.SetActive(false);
                myPlayerSettings.SetActive(false);
                myPlayerSphere.SetActive(false);
                joinScreen.SetActive(false);
                TrackHUD_CountDownLabel.gameObject.SetActive(true);
                resultsScreen.SetActive(false);
                break;
            case GameStates.PLAYING:
                backgroundCube.SetActive(false);
                mainMenuCanvas.SetActive(false);
                hostScreen.SetActive(false);
                myPlayerSettings.SetActive(false);
                myPlayerSphere.SetActive(false);
                joinScreen.SetActive(false);
                TrackHUD_CountDownLabel.gameObject.SetActive(true);
                resultsScreen.SetActive(false);
                break;
            case GameStates.PAUSED:
                break;
            case GameStates.REVIEWING:
                backgroundCube.SetActive(true);
                mainMenuCanvas.SetActive(false);
                hostScreen.SetActive(false);
                myPlayerSettings.SetActive(false);
                myPlayerSphere.SetActive(false);
                joinScreen.SetActive(false);
                TrackHUD_CountDownLabel.gameObject.SetActive(false);
                resultsScreen.SetActive(true);
                break;
            default:
                Debug.LogError("Game state unrecognized! Unable to update the UI.");
                break;
        }
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

    #region During play - HUD
    internal void StartCountDown(float timeInSeconds)
    {
        _currentTimerValue = timeInSeconds;

        StartCoroutine(UpdateCountdown());
    }

    private IEnumerator UpdateCountdown()
    {
        TrackHUD_CountDownLabel.gameObject.SetActive(true);

        while (_currentTimerValue >= 0)
        {
            // update UI label with new value
            TrackHUD_CountDownLabel.text = _currentTimerValue.ToString();

            // and wait a second before doing it again.
            _currentTimerValue--;

            if(_currentTimerValue == 0)
            {
                CountDownAnimator.SetBool("ShouldShrink", true);
            }

            yield return new WaitForSeconds(1);
        }

        GameManager.Instance.SetCurrentGameState(GameStates.PLAYING);
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState == GameStates.PLAYING)
        {
            TrackHUD_CountDownLabel.text = (DateTime.Now - GameManager.Instance.CurrentRace.StartTime)
                                            .ToString(TIME_FORMAT);
        }
    }
    #endregion

    #region Post-play
    internal void PopulateResultsScreen()
    {
        // populate the track name, type laps
        resultsTrack.text = GameManager.Instance.CurrentRace.SceneName;
        resultsType.text = GameManager.Instance.CurrentRace.RaceType.ToString();
        resultsLapXofY.text = "Lap " + GameManager.Instance.CurrentRace.CurrentLap + " of " + GameManager.Instance.CurrentRace.TotalLaps;


        int verticalAlignment = 250;
        int rowHeight = -40;

        int position = 1;

        int remove = GameManager.Instance.CalculateRemoval(GameManager.Instance.CurrentRace.TotalLaps,
                                                            GameManager.Instance.CurrentRace.CurrentLap + 1,
                                                            GameManager.Instance.CurrentRace.racers.Count);

        // for each racer
        for (int i = 0; i < GameManager.Instance.CurrentRace.racers.Count; i++)
        {
            Racer r = GameManager.Instance.CurrentRace.racers[i];

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
            timeLabel.text = (r.finishTime - GameManager.Instance.CurrentRace.StartTime).ToString(TIME_FORMAT);

            bool shouldShowRed = (i >= (GameManager.Instance.CurrentRace.racers.Count - remove));
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
        for (int i = 0; i < rows.Length; i++)
        {
            GameObject.Destroy(rows[i]);
        }
    }

    #endregion
}
