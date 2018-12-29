using System.Collections;
using UnityEngine;

public class BubbleSpawner : AbstractSpawner {

    [SerializeField] GameObject blueBubblePrefab, redBubblePrefab;


    protected override void UpdateIsSpwaningDuringThisLevel() {
        isSpwaningDuringThisLevel = (levelSettings.bubbleMinWait >= 0 && levelSettings.bubbleMaxWait > 0);
    }

    protected override IEnumerator SpawningRoutine() {
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

}
