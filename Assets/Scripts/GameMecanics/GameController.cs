using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameController gc;
    [Header("Canvas")]
    [SerializeField] GameObject gameUICanvas;
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] ScreenTransition screenTransition;
    [Header("Controllers")]
    [SerializeField] PlayerController playerCtrlr;
    [SerializeField] CameraController cameraCtrlr;
    [SerializeField] BubbleSpawner bubbleSpawner;
    [SerializeField] MineSpawner mineSpawner;
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
        gameUICanvas.SetActive(true);
        playerCtrlr.gameObject.SetActive(true);
        playerCtrlr.enabled = true;
        cameraCtrlr.enabled = true;
        bubbleSpawner.enabled = true;
        mineSpawner.enabled = true;
        mainMenuCanvas.SetActive(false);
    }

    public void StopGame() {
        gameUICanvas.SetActive(false);
        playerCtrlr.enabled = false;
        cameraCtrlr.enabled = false;
        bubbleSpawner.enabled = false;
        mineSpawner.enabled = false;
    }

    public void TriggerGameOver(int score) {
        OnGameOver(score);
        StopGame();
        DisplayGameOverScreen(score);
        ApplicationController.ac.recordNewScore(score);
        ApplicationController.ac.Save();
    }

    void DisplayGameOverScreen(int score) {
        goScoreText.text = score.ToString();
        goBestScoreText.text = ApplicationController.ac.PlayerData.bestScore.ToString();
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
        playerCtrlr.InitPlayer();
        mainMenuCanvas.SetActive(true);
        cameraCtrlr.InitPosition();
    }

    void DestroyMainElements() {
        foreach(GameObject obj in GameObject.FindGameObjectsWithTag("MainElement")) {
            Destroy(obj);
        }
    }

}
