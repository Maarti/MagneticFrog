using System.Collections;
using UnityEngine;

public class TrashSpawner : AbstractSpawner {

    [SerializeField] GameObject blueCanPrefab;
    [SerializeField] GameObject redCanPrefab;
    [SerializeField] GameObject plasticBottlePrefab;

    protected override void UpdateIsSpwaningDuringThisLevel() {
        isSpwaningDuringThisLevel = (levelSettings.trashMinWait >= 0 && levelSettings.trashMaxWait > 0);
    }

    protected override IEnumerator SpawningRoutine() {
        WaitForSeconds waitOneSec = new WaitForSeconds(1f);
        while (true) {
            if (isSpwaningDuringThisLevel) {
                yield return new WaitForSeconds(Random.Range(levelSettings.trashMinWait, levelSettings.trashMaxWait));
                SpawnOne(null);
            }
            else {
                yield return waitOneSec;
            }
        }
    }

    GameObject SpawnOne(GameObject prefab) {
        // Position
        Vector3 pos = transform.position;
        pos.x = Random.Range(minPosX, maxPosX);

        // Type
        GameObject trash;
        if (prefab == null) {
            trash = (Random.value > .5f) ?
            Instantiate((Random.value > .5f) ? blueCanPrefab : redCanPrefab, pos, Quaternion.identity) :
            Instantiate(plasticBottlePrefab, pos, Quaternion.identity);
        }
        else {
            trash = Instantiate(prefab, pos, Quaternion.identity);
        }
        trash.transform.rotation = Random.rotation;

        // Rotation
        Rigidbody2D rb = trash.GetComponent<Rigidbody2D>();
        rb.AddTorque(Random.Range(-1f, 1f));

        return trash;
    }


    protected override IEnumerator Burst(int quantity, float timeInSeconds, BurstType burstType) {
        if (quantity <= 0) yield break;
        float rate = timeInSeconds / quantity;
        WaitForSeconds waitingTime = new WaitForSeconds(rate);
        int nbSpawned = 0;
        GameObject prefab = burstType == BurstType.Bottle ? plasticBottlePrefab : null;
        while (nbSpawned < quantity) {
            SpawnOne(prefab);
            nbSpawned++;
            yield return waitingTime;
        }
    }



}
