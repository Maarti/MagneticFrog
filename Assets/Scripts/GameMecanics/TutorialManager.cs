using System.Collections;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [Header("UI")]
    [SerializeField] GameObject meterCounter;
    [SerializeField] GameObject oxygenBar;
    [SerializeField] GameObject magnetControllerLayout;
    [SerializeField] GameObject tutorialCanvas;
    [SerializeField] Animator tutorialCanvasAnimator;
    [SerializeField] ScreenTransition screenTransition;
    [Header("Controllers")]
    [SerializeField] GameController gameCtrlr;
    // [SerializeField] PlayerController playerCtrlr;
    [SerializeField] OxygenController oxygenCtrlr;
    [SerializeField] MagnetController magnetCtrlr;
    [SerializeField] JumpController jumpCtrlr;
    [SerializeField] LevelSettingsController levelSettingsCtrlr;
    [Header("Spawners")]
    [SerializeField] BubbleSpawner bubbleSpawner;
    [SerializeField] MineSpawner mineSpawner;
    [Header("Misc")]
    [SerializeField] GameObject tutorialConfiner;

    int nbJump = 0;
    int nbBubbles = 0;
    int state = -1;
    readonly WaitForSeconds wait = new WaitForSeconds(2f);
    Coroutine blueBubblesCoroutine;

    void OnEnable() {
        state = 0;
        nbJump = 0;
        nbBubbles = 0;
        JumpController.OnJump += OnJump;
        OxygenController.OnBubbleCollect += OnBubbleCollect;
        tutorialConfiner.SetActive(true);
        meterCounter.SetActive(false);
        jumpCtrlr.enabled = true;
        jumpCtrlr.Init();
        // Tuto canvas
        tutorialCanvas.SetActive(true);
        // Oxygen
        oxygenBar.SetActive(false);
        oxygenCtrlr.enabled = false;
        // Magnet
        magnetControllerLayout.SetActive(false);
        magnetCtrlr.Init();
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
                if (nbBubbles >= 10) StartMinesTutorial();
                break;

            // Mines
            case 5:
                // waiting for the end of coroutine
                break;

            // End
            case 6:
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
        tutorialCanvasAnimator.SetInteger("state", 1);
    }

    void StartBubblesTutorial() {
        tutorialCanvasAnimator.SetInteger("state", 2);
        oxygenBar.SetActive(true);
        oxygenCtrlr.enabled = true;
        oxygenCtrlr.Init();
        blueBubblesCoroutine = StartCoroutine(SpawnBlueBubbles());
        NextState();
    }

    void StartMagnetTutorial() {
        tutorialCanvasAnimator.SetInteger("state", 3);
        StopCoroutine(blueBubblesCoroutine);
        NextState();
    }

    void StartBubblesMagnetTutorial() {
        magnetControllerLayout.SetActive(true);
        magnetCtrlr.enabled = true;
        magnetCtrlr.Init();
         StartCoroutine(SpawnRedBubbles());
        tutorialCanvasAnimator.SetInteger("state", 4);
        NextState();
    }

    void StartMinesTutorial() {
        tutorialCanvasAnimator.SetInteger("state", 5);
         StartCoroutine(SpawnMines());
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

    IEnumerator SpawnMines() {
        int i = 0;
        while (i < 10) {
            yield return wait;
            MineType color = (i < 4) ? MineType.Blue : MineType.Red;
            mineSpawner.SpawnMine(color, 1f, 1f, .5f, .5f);
            i++;
        }
        NextState();
    }

    public void NextState() {
        state++;
    }

    void EndTutorial() {
        screenTransition.ScreenFadeThen(AfterTutorialEnd);
        StopAllCoroutines();
        Achievement.Unlock(GPGSIds.achievement_fast_learner);
    }

    void AfterTutorialEnd() {
        tutorialCanvasAnimator.SetInteger("state", 0);
        ApplicationController.ac.FinishTutorial();
        gameCtrlr.StopGame();
        gameCtrlr.StartGame();
    }
}

