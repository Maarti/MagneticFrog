using System.Collections;
using UnityEngine;

public class BubbleSpawner : AbstractSpawner {

    [SerializeField] GameObject blueBubblePrefab, redBubblePrefab;
    [SerializeField] Color redColor;
    [SerializeField] Color blueColor;

    protected override void UpdateIsSpwaningDuringThisLevel() {
        isSpwaningDuringThisLevel = (levelSettings.bubbleMinWait >= 0 && levelSettings.bubbleMaxWait > 0);
    }

    protected override IEnumerator SpawningRoutine() {
        WaitForSeconds waitOneSec = new WaitForSeconds(1f);
        while (true) {
            if (isSpwaningDuringThisLevel) {
                yield return new WaitForSeconds(Random.Range(levelSettings.bubbleMinWait, levelSettings.bubbleMaxWait));

                if (Random.value > .5f)
                    SpawnBlueBubble();
                else
                    SpawnRedBubble();
            }
            else {
                yield return waitOneSec;
            }
        }
    }

    public GameObject SpawnBlueBubble() {
        Vector3 pos = transform.position;
        pos.x = Random.Range(minPosX, maxPosX);
        GameObject bubble = Instantiate(blueBubblePrefab, pos, Quaternion.identity);
        bubble.GetComponent<BubbleController>().SetColor(blueColor);
        return bubble;
    }

    public GameObject SpawnRedBubble() {
        Vector3 pos = transform.position;
        pos.x = Random.Range(minPosX, maxPosX);
        GameObject bubble = Instantiate(redBubblePrefab, pos, Quaternion.identity);
        bubble.GetComponent<BubbleController>().SetColor(redColor);
        return bubble;
    }

}
