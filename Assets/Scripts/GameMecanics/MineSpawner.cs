using System.Collections;
using UnityEngine;

public class MineSpawner : MonoBehaviour {

    [SerializeField] GameObject blueMinePrefab, redMinePrefab;
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
                yield return new WaitForSeconds(Random.Range(levelSettings.mineMinWait, levelSettings.mineMaxWait));
                Vector3 pos = transform.position;
                pos.x = Random.Range(minPosX, maxPosX);
                GameObject mine;
                if (Random.value > .5f)
                    mine = Instantiate(blueMinePrefab, pos, Quaternion.identity);
                else
                    mine = Instantiate(redMinePrefab, pos, Quaternion.identity);
                Vector3 scale = mine.transform.localScale;
                scale *= Random.Range(levelSettings.mineMinSize, levelSettings.mineMaxSize);
                mine.transform.localScale = scale;
            }
            else {
                yield return waitOneSec;
            }
        }
    }

    void UpdateLevelSettings(LevelSettings newLevelSettings) {
        levelSettings = newLevelSettings;
        isSpwaningDuringThisLevel = (levelSettings.mineMinWait >= 0 && levelSettings.mineMaxWait > 0);
    }

}
