using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLifecycle : MonoBehaviour {

    [SerializeField] GameObject controllerUI, mainMenuPanel, gameTitleText;
    [SerializeField] CameraController cameraCtrlr;
    [SerializeField] BubbleSpawner bubbleSpawner;
    [SerializeField] MineSpawner mineSpawner;

    public void StartGame() {
        controllerUI.SetActive(true);
        cameraCtrlr.enabled = true;
        bubbleSpawner.enabled = true;
        mineSpawner.enabled = true;
        mainMenuPanel.SetActive(false);
        gameTitleText.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().enabled = true;
    }

    public void StopGame() {
        controllerUI.SetActive(false);
        cameraCtrlr.enabled = false;
        bubbleSpawner.enabled = false;
        mineSpawner.enabled = false;
        mainMenuPanel.SetActive(true);
    }
}
