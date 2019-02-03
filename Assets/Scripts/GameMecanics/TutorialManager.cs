using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [Header("UI")]
    [SerializeField] GameObject meterCounter;
    [SerializeField] GameObject oxygenBar;
    [SerializeField] GameObject magnetControllerLayout;
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] GameObject uiJumpTuto, uiBubbleTuto, uiMagnetTuto;
    [SerializeField] ScreenTransition screenTransition;
    [Header("Controllers")]
    [SerializeField] GameController gameCtrlr;
    [SerializeField] PlayerController playerCtrlr;
    [SerializeField] OxygenController oxygenCtrlr;
    [SerializeField] MagnetController magnetCtrlr;
    [SerializeField] JumpController jumpCtrlr;
    [SerializeField] LevelSettingsController levelSettingsCtrlr;
    [Header("Spawners")]
    [SerializeField] BubbleSpawner bubbleSpawner;
    [SerializeField] MineSpawner mineSpawner;
    [Header("Misc")]
    [SerializeField] GameObject tutorialConfiner;
    [SerializeField] float minPosX = -2f, maxPosX = 2f;

    int nbJump = 0;
    int nbBubbles = 0;
    int state = -1;
    WaitForSeconds wait = new WaitForSeconds(2f);
    Coroutine blueBubblesCoroutine;
    Coroutine redBubblesCoroutine;
    Coroutine minesCoroutine;

    void OnEnable() {
        state = 0;
        JumpController.OnJump += OnJump;
        nbJump = 0;
        OxygenController.OnBubbleCollect += OnBubbleCollect;
        nbBubbles = 0;
        tutorialConfiner.SetActive(true);
        meterCounter.SetActive(false);
        jumpCtrlr.enabled = true;
        jumpCtrlr.Init();
        // Tuto canvas
        tutorialCanvas.SetActive(true);
        uiJumpTuto.SetActive(false);
        uiBubbleTuto.SetActive(false);
        uiMagnetTuto.SetActive(false);
        // Oxygen
        oxygenBar.SetActive(false);
        oxygenCtrlr.enabled = false;
        // Magnet
        magnetControllerLayout.SetActive(false);
        magnetCtrlr.enabled = false;
        levelSettingsCtrlr.enabled = false;
        StartJumpTutorial();
    }

    void OnDisable() {
        if (tutorialConfiner != null) tutorialConfiner.SetActive(false);
        if (meterCounter != null) meterCounter.SetActive(true);
        if (oxygenBar != null) oxygenBar.SetActive(true);
        if (magnetControllerLayout != null) magnetControllerLayout.SetActive(true);
        if (tutorialCanvas != null) tutorialCanvas.SetActive(false);
        levelSettingsCtrlr.enabled = true;
        StopAllCoroutines();
        JumpController.OnJump -= OnJump;
        OxygenController.OnBubbleCollect -= OnBubbleCollect;
    }

    void Update() {
        switch (state) {
            // Jumping tutorial
            case 0:
                if (nbJump >= 5) StartBubblesTutorial();
                break;

            // Collecting bubbles
            case 1:
                if (nbBubbles >= 5) StartMagnetTutorial();
                break;

            // Switching magnet
            case 2:
                if (nbBubbles >= 10) state++;
                break;

            // End
            case 3:
                EndTutorial();
                state++;
                break;
        }
    }

    void OnJump() {
        nbJump++;
    }

    void OnBubbleCollect() {
        nbBubbles++;
    }

    void StartJumpTutorial() {
        uiJumpTuto.SetActive(true);
    }

    void StartBubblesTutorial() {
        uiJumpTuto.SetActive(false);
        uiBubbleTuto.SetActive(true);
        oxygenBar.SetActive(true);
        oxygenCtrlr.enabled = true;
        oxygenCtrlr.Init();
        blueBubblesCoroutine = StartCoroutine(SpawnBlueBubbles());
        state++;
    }

    void StartMagnetTutorial() {
        uiBubbleTuto.SetActive(false);
        uiMagnetTuto.SetActive(true);
        StopCoroutine(blueBubblesCoroutine);
        magnetControllerLayout.SetActive(true);
        magnetCtrlr.enabled = true;
        magnetCtrlr.Init();
        redBubblesCoroutine = StartCoroutine(SpawnRedBubbles());
        state++;
    }

    IEnumerator SpawnBlueBubbles() {
        while (true) {
            yield return wait;
            bubbleSpawner.SpawnBlueBubble();
        }
    }

    IEnumerator SpawnRedBubbles() {
        while (true) {
            yield return wait;
            bubbleSpawner.SpawnRedBubble();
        }
    }

    void EndTutorial() {
        screenTransition.ScreenFadeThen(AfterTutorialEnd);        
    }

    void AfterTutorialEnd() {
        ApplicationController.ac.FinishTutorial();
        gameCtrlr.StopGame();
        gameCtrlr.StartGame();
    }
}

