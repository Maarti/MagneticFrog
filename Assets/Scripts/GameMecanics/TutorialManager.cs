using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [Header("UI")]
    [SerializeField] GameObject meterCounter;
    [SerializeField] GameObject oxygenBar;
    [SerializeField] GameObject magnetController;
    [Header("Spawners")]
    [SerializeField] GameObject blueMinePrefab, redMinePrefab;
    [Header("Misc")]
    [SerializeField] GameObject tutorialConfiner;
    [SerializeField] float minPosX = -2f, maxPosX = 2f;


    void OnEnable() {
        tutorialConfiner.SetActive(true);
        meterCounter.SetActive(false);
        oxygenBar.SetActive(false);
        magnetController.SetActive(false);
    }

    void OnDisable() {
        tutorialConfiner.SetActive(false);
        meterCounter.SetActive(true);
        oxygenBar.SetActive(true);
        magnetController.SetActive(true);
        StopAllCoroutines();
    }


    /* IEnumerator SpawningRoutine() {
         WaitForSeconds waitOneSec = new WaitForSeconds(1f);
         while (true) {

             yield return new WaitForSeconds(Random.Range(levelSettings.mineMinWait, levelSettings.mineMaxWait));

             // Position
             Vector3 pos = transform.position;
             pos.x = Random.Range(minPosX, maxPosX);

             // Type
             GameObject mine;
             if (Random.value > .5f)
                 mine = Instantiate(blueMinePrefab, pos, Quaternion.identity);
             else
                 mine = Instantiate(redMinePrefab, pos, Quaternion.identity);

             // Size
             Vector3 scale = mine.transform.localScale;
             scale *= Random.Range(levelSettings.mineMinSize, levelSettings.mineMaxSize);
             mine.transform.localScale = scale;

             // Speed
             Rigidbody2D rb = mine.GetComponent<Rigidbody2D>();
             rb.gravityScale *= Random.Range(levelSettings.mineMinSpeed, levelSettings.mineMaxSpeed);

         }

     }*/
}
