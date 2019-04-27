using System.Collections;
using UnityEngine;

public class CoinSpawner : AbstractSpawner {

    [SerializeField] GameObject standardCoinPrefab;
    [SerializeField] GameObject blueCoinPrefab;
    [SerializeField] GameObject redCoinPrefab;
    [SerializeField] Camera cam;
    [SerializeField] RectTransform coinIndicator;
    [SerializeField] Animator coinIndicatorAnim;
    public SoundController coinSoundCtrlr;

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
                GameObject coin;
                if (Random.value > 1f) {
                    if (Random.value > .5f)
                        coin = Instantiate(blueCoinPrefab, pos, Quaternion.identity);
                    else
                        coin = Instantiate(redCoinPrefab, pos, Quaternion.identity);
                }
                else
                    coin = Instantiate(standardCoinPrefab, pos, Quaternion.identity);
                CoinController ctrlr = coin.GetComponent<CoinController>();
                ctrlr.coinSoundCtrlr = coinSoundCtrlr;
                ctrlr.cam = cam;
                ctrlr.coinIndicator = coinIndicator;
                ctrlr.coinIndicatorAnim = coinIndicatorAnim;
            }
            else {
                yield return waitOneSec;
            }
        }
    }


}
