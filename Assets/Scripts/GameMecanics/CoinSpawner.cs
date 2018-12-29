using System.Collections;
using UnityEngine;

public class CoinSpawner : AbstractSpawner {

    [SerializeField] GameObject standardCoinPrefab, blueCoinPrefab, redCoinPrefab;

    protected override void UpdateIsSpwaningDuringThisLevel() {
        isSpwaningDuringThisLevel = (levelSettings.coinMinWait >= 0 && levelSettings.coinMaxWait > 0);
    }

    protected override IEnumerator SpawningRoutine() {
        WaitForSeconds waitOneSec = new WaitForSeconds(1f);
        while (true) {
            if (isSpwaningDuringThisLevel) {
                yield return new WaitForSeconds(Random.Range(levelSettings.coinMinWait, levelSettings.coinMaxWait));
                Vector3 pos = transform.position;
                pos.x = Random.Range(minPosX, maxPosX);
                if (Random.value > 1f) {
                    if (Random.value > .5f)
                        Instantiate(blueCoinPrefab, pos, Quaternion.identity);
                    else
                        Instantiate(redCoinPrefab, pos, Quaternion.identity);
                }
                else
                    Instantiate(standardCoinPrefab, pos, Quaternion.identity);
            }
            else {
                yield return waitOneSec;
            }
        }
    }


}
