using System.Collections;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    [SerializeField] GameObject blueBubblePrefab, redBubblePrefab;
    [SerializeField] float minPosX = -2f, maxPosX = 2f;
    [SerializeField] LevelSettings levelSettings;
    bool isSpwaningDuringThisLevel = true;

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
        WaitForSeconds waitOneSec = new WaitForSeconds(1f);
        while (true) {
            if (isSpwaningDuringThisLevel) {
                yield return new WaitForSeconds(Random.Range(levelSettings.bubbleMinWait, levelSettings.bubbleMaxWait));
                Vector3 pos = transform.position;
                pos.x = Random.Range(minPosX, maxPosX);
                if (Random.value > .5f)
                    Instantiate(blueBubblePrefab, pos, Quaternion.identity);
                else
                    Instantiate(redBubblePrefab, pos, Quaternion.identity);
            }
            else {
                yield return waitOneSec;
            }
        }
    }

    void UpdateLevelSettings(LevelSettings newLevelSettings) {
        levelSettings = newLevelSettings;
        isSpwaningDuringThisLevel = (levelSettings.bubbleMinWait >= 0 && levelSettings.bubbleMaxWait > 0);
    }


}
