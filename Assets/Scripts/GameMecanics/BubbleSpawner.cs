using System.Collections;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject blueBubblePrefab, redBubblePrefab;
    public float minPosX = -2f, maxPosX = 2f;
    public LevelSettings levelSettings;


    void OnEnable() {
        LevelSettingsController.OnLevelSettingsChange += UpdateLevelSettings;
        UpdateLevelSettings(LevelSettingsController.currentLevelSettings);
        StartCoroutine(SpawningRoutine());
    }

    private void OnDisable() {
        LevelSettingsController.OnLevelSettingsChange -= UpdateLevelSettings;
        StopAllCoroutines();
    }

    IEnumerator SpawningRoutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(levelSettings.bubbleMinWait, levelSettings.bubbleMaxWait));
            Vector3 pos = transform.position;
            pos.x = Random.Range(minPosX, maxPosX);
            if (Random.value > .5f)
                Instantiate(blueBubblePrefab, pos, Quaternion.identity);
            else
                Instantiate(redBubblePrefab, pos, Quaternion.identity);
        }
    }

    void UpdateLevelSettings(LevelSettings newLevelSettings) {
        levelSettings = newLevelSettings;
    }


}
