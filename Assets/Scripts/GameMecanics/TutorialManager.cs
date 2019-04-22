using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [Header("UI")]
    [SerializeField] GameObject meterCounter;
    [SerializeField] GameObject oxygenBar;
    [SerializeField] GameObject magnetControllerLayout;
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] Animator tutorialCanvasAnimator;
    //  [SerializeField] GameObject uiJumpTuto, uiBubbleTuto, uiMagnetTuto;
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
        /*    uiJumpTuto.SetActive(false);
            uiBubbleTuto.SetActive(false);
            uiMagnetTuto.SetActive(false);*/
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

            // Switching magnet UI
            case 2:
                break;

            // Switching magnet UI Ended
            case 3:
                StartBubblesMagnetTutorial();
                break;

            // MagnetBubbles
            case 4:
                if (nbBubbles >= 10) state++;
                break;


            // End
            case 5:
                EndTutorial();
                NextState();
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
        // uiJumpTuto.SetActive(true);
        tutorialCanvasAnimator.SetInteger("state", 1);
    }

    void StartBubblesTutorial() {
        tutorialCanvasAnimator.SetInteger("state", 2);
        // uiJumpTuto.SetActive(false);
        // uiBubbleTuto.SetActive(true);
        oxygenBar.SetActive(true);
        oxygenCtrlr.enabled = true;
        oxygenCtrlr.Init();
        blueBubblesCoroutine = StartCoroutine(SpawnBlueBubbles());
        NextState();
    }

    void StartMagnetTutorial() {
        tutorialCanvasAnimator.SetInteger("state", 3);
        // uiBubbleTuto.SetActive(false);
        // uiMagnetTuto.SetActive(true);
        StopCoroutine(blueBubblesCoroutine);
        /*  magnetControllerLayout.SetActive(true);
          magnetCtrlr.enabled = true;
          magnetCtrlr.Init();
          redBubblesCoroutine = StartCoroutine(SpawnRedBubbles()); */
        NextState();
    }

    void StartBubblesMagnetTutorial() {
        magnetControllerLayout.SetActive(true);
        magnetCtrlr.enabled = true;
        magnetCtrlr.Init();
        redBubblesCoroutine = StartCoroutine(SpawnRedBubbles());
        NextState();
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

    public void NextState() {
        state++;
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

