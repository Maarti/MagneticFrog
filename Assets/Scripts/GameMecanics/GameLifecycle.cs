using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLifecycle : MonoBehaviour {

    [SerializeField] GameObject gameUICanvas, mainMenuCanvas, gameTitleCanvas;
    [SerializeField] CameraController cameraCtrlr;
    [SerializeField] BubbleSpawner bubbleSpawner;
    [SerializeField] MineSpawner mineSpawner;

    private void OnEnable() {
        PlayerController.OnGameOver += OnGameOver;
    }

    public void StartGame() {
        gameUICanvas.SetActive(true);
        cameraCtrlr.enabled = true;
        bubbleSpawner.enabled = true;
        mineSpawner.enabled = true;
        mainMenuCanvas.SetActive(false);
        gameTitleCanvas.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = true;
    }

    public void StopGame() {
        gameUICanvas.SetActive(false);
        cameraCtrlr.enabled = false;
        bubbleSpawner.enabled = false;
        mineSpawner.enabled = false;
    }

    public void OnGameOver(int newScore) {
        StopGame();
    }
}
