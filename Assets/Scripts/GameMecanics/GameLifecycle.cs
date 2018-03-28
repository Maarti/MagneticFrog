using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLifecycle : MonoBehaviour {

    [SerializeField]
    GameObject controllerUI, mainMenuPanel;
    [SerializeField]
    CameraController cameraCtrlr;
    [SerializeField]
    BubbleSpawner bubbleSpawner;

    public void StartGame() {
        controllerUI.SetActive(true);
        cameraCtrlr.enabled = true;
        bubbleSpawner.enabled = true;
        mainMenuPanel.SetActive(false);
    }

    public void StopGame() {
        controllerUI.SetActive(false);
        cameraCtrlr.enabled = false;
        bubbleSpawner.enabled = false;
        mainMenuPanel.SetActive(true);
    }
}
