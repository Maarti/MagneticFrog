using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameController gc;
    [Header("Canvas")]
    [SerializeField] GameObject gameUICanvas;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject bestScoreMarker;
    [SerializeField] ScreenTransition screenTransition;
    [Header("Controllers")]
    [SerializeField] PlayerController playerCtrlr;
    [SerializeField] CameraController cameraCtrlr;
    [SerializeField] LevelSettingsController levelSettingsController;
    [SerializeField] BubbleSpawner bubbleSpawner;
    [SerializeField] CoinSpawner coinSpawner;
    [SerializeField] MineSpawner mineSpawner;
    [SerializeField] RockSpawner rockSpawner;
    [SerializeField] TutorialManager tutorialManager;
    [Header("Game Over Screen")]
    [SerializeField] Canvas goCanvas;
    [SerializeField] Text goScoreText;
    [SerializeField] Text goBestScoreText;

    public delegate void GameOver(int score);
    public static event GameOver OnGameOver;
    public delegate void GameStart();
    public static event GameStart OnGameStart;

    void Awake() {
        if (gc != this) gc = this;
    }

    public void StartGame() {
        DestroyMainElements();
        mainMenuCanvas.SetActive(false);
        if (ApplicationController.ac.PlayerData.isTutorialDone) {
            gameUICanvas.SetActive(true);
            bestScoreMarker.SetActive(true);
            // playerCtrlr.gameObject.SetActive(true);
            playerCtrlr.enabled = true;
            levelSettingsController.enabled = true;
            cameraCtrlr.enabled = true;
            bubbleSpawner.enabled = true;
            coinSpawner.enabled = true;
            mineSpawner.enabled = true;
            rockSpawner.enabled = true;
            tutorialManager.enabled = false;
        }
        else {
            StartTutorial();
        }
    }

    void StartTutorial() {
        tutorialManager.enabled = true;
        gameUICanvas.SetActive(true);
        bestScoreMarker.SetActive(false);
        playerCtrlr.enabled = true;
        cameraCtrlr.enabled = true;
    }

    public void StopGame() {
        gameUICanvas.SetActive(false);
        bestScoreMarker.SetActive(false);
        playerCtrlr.enabled = false;
        levelSettingsController.enabled = false;
        cameraCtrlr.enabled = false;
        bubbleSpawner.enabled = false;
        coinSpawner.enabled = false;
        rockSpawner.enabled = false;
        mineSpawner.enabled = false;
        tutorialManager.enabled = false;
    }

    public void TriggerGameOver(int score) {
        if (OnGameOver != null)
            OnGameOver(score);
        StopGame();
        DisplayGameOverScreen(score);
        ApplicationController.ac.RecordNewScore(score);
        ApplicationController.ac.Save();
    }

    void DisplayGameOverScreen(int score) {
        goScoreText.text = score.ToString();
        goBestScoreText.text = Mathf.Max(0, ApplicationController.ac.PlayerData.bestScore).ToString();
        goCanvas.gameObject.SetActive(true);
    }

    public void PlayAgain() {
        screenTransition.ScreenFadeThen(StartGame);
    }

    public void GoToHomeScreen() {
        screenTransition.ScreenFadeThen(DisplayHomeMenu);
    }

    void DisplayHomeMenu() {
        DestroyMainElements();
        playerCtrlr.gameObject.SetActive(true);
        playerCtrlr.Init();
        mainMenuCanvas.SetActive(true);
        cameraCtrlr.InitPosition();
    }

    void DestroyMainElements() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MainElement")) {
            Destroy(obj);
        }
    }

}
