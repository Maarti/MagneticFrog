using System.Collections;
using UnityEngine;

public class BubbleSpawner : AbstractSpawner {

    [SerializeField] GameObject blueBubblePrefab;
    [SerializeField] GameObject redBubblePrefab;
    [SerializeField] Color redColor;
    [SerializeField] Color blueColor;
    [SerializeField] SoundController bubbleSoundCtrlr;

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
        // bubble.GetComponent<BubbleController>().SetColor(blueColor);        
        BubbleController bubbleCtrlr = bubble.GetComponent<BubbleController>();
        bubbleCtrlr.type = ElementType.Blue;
        bubbleCtrlr.bubbleSoundCtrlr = bubbleSoundCtrlr;
        return bubble;
    }

    public GameObject SpawnRedBubble() {
        Vector3 pos = transform.position;
        pos.x = Random.Range(minPosX, maxPosX);
        GameObject bubble = Instantiate(redBubblePrefab, pos, Quaternion.identity);
        // bubble.GetComponent<BubbleController>().SetColor(redColor);
        BubbleController bubbleCtrlr = bubble.GetComponent<BubbleController>();
        bubbleCtrlr.type = ElementType.Red;
        bubbleCtrlr.bubbleSoundCtrlr = bubbleSoundCtrlr;
        return bubble;
    }

    public override void StartBurst(int quantity, float timeInSeconds, BurstType burstType) {
        StartCoroutine(Burst(quantity, timeInSeconds, burstType));
    }

    protected override IEnumerator Burst(int quantity, float timeInSeconds, BurstType burstType) {
        if (quantity <= 0) yield break;
        float rate = timeInSeconds / quantity;
        WaitForSeconds waitingTime = new WaitForSeconds(rate);
        int nbSpawned = 0;
        while (nbSpawned < quantity) {
            switch (burstType) {
                case BurstType.BlueBubble:
                    SpawnBlueBubble();
                    break;
                case BurstType.RedBubble:
                    SpawnRedBubble();
                    break;
                default:
                    if (Random.value > .5f)
                        SpawnBlueBubble();
                    else
                        SpawnRedBubble();
                    break;
            }
            nbSpawned++;
            yield return waitingTime;
        }
    }

}
