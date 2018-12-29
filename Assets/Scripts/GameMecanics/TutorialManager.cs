using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [Header("UI")]
    [SerializeField] GameObject meterCounter;
    [SerializeField] GameObject oxygenBar;
    [SerializeField] GameObject magnetControllerLayout;
    [Header("Controllers")]
    [SerializeField] OxygenController oxygenCtrlr;
    [SerializeField] MagnetController magnetCtrlr;
    [SerializeField] JumpController jumpCtrlr;
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
        magnetControllerLayout.SetActive(false);
        oxygenBar.SetActive(false);
        oxygenCtrlr.enabled = false;
        magnetCtrlr.enabled = false;
        jumpCtrlr.enabled = true;
    }

    void OnDisable() {
        tutorialConfiner.SetActive(false);
        meterCounter.SetActive(true);
        oxygenBar.SetActive(true);
        magnetControllerLayout.SetActive(true);
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


        }
    }

    void OnJump() {
        nbJump++;
    }

    void OnBubbleCollect() {
        nbBubbles++;
    }

    void StartBubblesTutorial() {
        oxygenBar.SetActive(true);
        oxygenCtrlr.enabled = true;
        oxygenCtrlr.Init();
        blueBubblesCoroutine = StartCoroutine(SpawnBlueBubbles());
        state++;
    }


    void StartMagnetTutorial() {
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
}

