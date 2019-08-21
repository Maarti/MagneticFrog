using System.Collections;
using UnityEngine;

public class RockSpawner : AbstractSpawner {

    [SerializeField] GameObject rockPrefab1;
    [SerializeField] GameObject rockPrefab2;

    protected override void UpdateIsSpwaningDuringThisLevel() {
        isSpwaningDuringThisLevel = (levelSettings.rockMinWait >= 0 && levelSettings.rockMaxWait > 0);
    }

    protected override IEnumerator SpawningRoutine() {
        WaitForSeconds waitOneSec = new WaitForSeconds(1f);
        while (true) {
            if (isSpwaningDuringThisLevel) {
                yield return new WaitForSeconds(Random.Range(levelSettings.rockMinWait, levelSettings.rockMaxWait));
                SpawnOne(levelSettings.rockMinSize, levelSettings.rockMaxSize, levelSettings.rockMinSpeed, levelSettings.rockMaxSpeed);
            }
            else {
                yield return waitOneSec;
            }
        }
    }

    GameObject SpawnOne(float minScale, float maxScale, float minSpeed, float maxSpeed) {
        // Position
        Vector3 pos = transform.position;
        pos.x = Random.Range(minPosX, maxPosX);

        // Type
        GameObject rock;
        float rand = Random.value;
        if (rand > .5f)
            rock = Instantiate(rockPrefab1, pos, Quaternion.identity);
        else {
            rock = Instantiate(rockPrefab2, pos, Quaternion.identity);
            rock.transform.rotation = Random.rotation;
        }

        // Size
        Vector3 scale = rock.transform.localScale;
        scale *= Random.Range(minScale, maxScale);
        rock.transform.localScale = scale;

        // Speed
        Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();
        rb.gravityScale *= Random.Range(minSpeed, maxSpeed);

        return rock;
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
            SpawnOne(levelSettings.rockMinSize, levelSettings.rockMaxSize, levelSettings.rockMinSpeed, levelSettings.rockMaxSpeed);
            nbSpawned++;
            yield return waitingTime;
        }
    }



}
