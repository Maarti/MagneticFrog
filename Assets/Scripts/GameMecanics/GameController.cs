using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [Header("Canvas")]
    [SerializeField] GameObject gameUICanvas;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject gameOverCanvas;
    [SerializeField] GameObject pauseUICanvas;
    [SerializeField] GameObject bestScoreMarker;
    [SerializeField] ScreenTransition screenTransition;
    [SerializeField] GraphicRaycaster gameOverRaycaster;
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

    public static GameController gc;
    public delegate void GameOver(int score);
    public static event GameOver OnGameOver;
    public delegate void GameStart();
    public static event GameStart OnGameStart;
    [Header("Particle systm")]
    public ParticleSystem bubbleParticleSystm;

    void Awake() {
        if (gc != this) gc = this;
    }

    public void StartGame() {
        DestroyMainElements();
        mainMenuCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        pauseUICanvas.SetActive(false);
        gameOverRaycaster.enabled = true;
        gameUICanvas.SetActive(true);
        cameraCtrlr.enabled = true;
        playerCtrlr.magnetCtrlr.StartMovingMagnetToFrog();
        AdsController.instance.LoadInterstitial();
        if (ApplicationController.ac.PlayerData.isTutorialDone) {
            playerCtrlr.isPlayingTutorial = false;
            playerCtrlr.enabled = true;
            bestScoreMarker.SetActive(true);
            // playerCtrlr.gameObject.SetActive(true);
            levelSettingsController.enabled = true;
            bubbleSpawner.enabled = true;
            coinSpawner.enabled = true;
            mineSpawner.enabled = true;
            rockSpawner.enabled = true;
            tutorialManager.gameObject.SetActive(false);
        }
        else {
            StartTutorial();
        }
        if (OnGameStart != null)
            OnGameStart();
    }

    void StartTutorial() {
        tutorialManager.gameObject.SetActive(true);
        bestScoreMarker.SetActive(false);
        playerCtrlr.isPlayingTutorial = true;
        playerCtrlr.enabled = true;
        // gameUICanvas.SetActive(true);
        //     cameraCtrlr.enabled = true;
    }

    public void StartGameAndParticleSystm() {
        StartGame();
        RestartBubblesParticlySystm();
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
        tutorialManager.gameObject.SetActive(false);
        playerCtrlr.magnetCtrlr.SetMagnetToMenuState();
    }

    public void TriggerGameOver(int score) {
        ApplicationController.ac.RecordNewScore(score);
        ApplicationController.ac.Save();
        StopGame();
        DisplayGameOverScreen(score);
        if (OnGameOver != null)
            OnGameOver(score);
    }

    void DisplayGameOverScreen(int score) {
        goScoreText.text = score.ToString();
        goBestScoreText.text = Mathf.Max(0, ApplicationController.ac.PlayerData.bestScore).ToString();
        goCanvas.gameObject.SetActive(true);
    }

    public void PlayAgain() {
        AdsController.instance.ShowInterstitial();
        gameOverRaycaster.enabled = false;
        screenTransition.ScreenFadeThen(StartGameAndParticleSystm);
    }

    public void GoToHomeScreen() {
        AdsController.instance.ShowInterstitial();
        gameOverRaycaster.enabled = false;
        screenTransition.ScreenFadeThen(DisplayHomeMenu);
    }

    void DisplayHomeMenu() {
        playerCtrlr.isPlayingTutorial = false;
        DestroyMainElements();
        gameOverCanvas.SetActive(false);
        gameOverRaycaster.enabled = true;
        pauseUICanvas.SetActive(false);
        playerCtrlr.gameObject.SetActive(true);
        playerCtrlr.Init();
        mainMenuCanvas.SetActive(true);
        cameraCtrlr.InitPosition();
        RestartBubblesParticlySystm();
    }

    void DestroyMainElements() {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MainElement")) {
            Destroy(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MainElement/Bubble")) {
            Destroy(obj);
        }
    }

    void RestartBubblesParticlySystm() {
        bubbleParticleSystm.Stop();
        bubbleParticleSystm.Play();
    }

}
