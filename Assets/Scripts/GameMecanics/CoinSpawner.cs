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
        CoinController ctrlr;
        GameObject coin;
        Vector3 pos;
        while (true) {
            if (isSpwaningDuringThisLevel) {
                yield return new WaitForSeconds(Random.Range(levelSettings.coinMinWait, levelSettings.coinMaxWait));
                pos = transform.position;
                pos.x = Random.Range(minPosX, maxPosX);
                if (Random.value > 1f) {
                    if (Random.value > .5f)
                        coin = Instantiate(blueCoinPrefab, pos, Quaternion.identity);
                    else
                        coin = Instantiate(redCoinPrefab, pos, Quaternion.identity);
                }
                else
                    coin = Instantiate(standardCoinPrefab, pos, Quaternion.identity);
                ctrlr = coin.GetComponent<CoinController>();
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

    public void StartBurst(int quantity, float timeInSeconds = 0f) {
        StartCoroutine(Burst(quantity, timeInSeconds));
    }

    IEnumerator Burst(int quantity, float timeInSeconds) {
        if (quantity <= 0) yield break;
        Vector3 pos;
        GameObject coin;
        CoinController ctrlr;
        float rate = timeInSeconds / quantity;
        WaitForSeconds waitingTime = new WaitForSeconds(rate);
        int nbSpawned = 0;
        while (nbSpawned < quantity) {
            pos = transform.position;
            pos.x = Random.Range(minPosX, maxPosX);
            coin = Instantiate(standardCoinPrefab, pos, Quaternion.identity);
            ctrlr = coin.GetComponent<CoinController>();
            ctrlr.coinSoundCtrlr = coinSoundCtrlr;
            ctrlr.cam = cam;
            ctrlr.coinIndicator = coinIndicator;
            ctrlr.coinIndicatorAnim = coinIndicatorAnim;
            nbSpawned++;
            yield return waitingTime;
        }

    }


}
