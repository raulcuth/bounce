using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public delegate void GameDelegate();

    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public Text scoreText;

    enum PageState {
        None,
        Start,
        GameOver,
        Countdown
    }

    private int score = 0;
    private bool _gameOver = false;

    public bool GameOver => _gameOver;

    void SetPageState(PageState state) {
        switch (state) {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;
        }
    }

    public void ConfirmGameOver() {
        //activated when replay button is hit
        SetPageState(PageState.Start);
        scoreText.text = "0";
        OnGameOverConfirmed(); //event
    }

    public void StartGame() {
        //activated when play button is hit
        SetPageState(PageState.Countdown);
    }

    private void Awake() {
        Instance = this;
    }

    private void OnEnable() {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;//subscribe to the events
    }

    private void OnDisable() {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;//unsubscribe from the events
    }

    private void OnCountdownFinished() {
        SetPageState(PageState.None);
        OnGameStarted();
        score = 0;
        _gameOver = false;
    }

    private void OnPlayerDied() {
        _gameOver = true;
        var savedScore = PlayerPrefs.GetInt("HighScore");
        if (score > savedScore) {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(PageState.GameOver);
    }
}