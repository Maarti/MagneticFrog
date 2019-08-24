using System.Collections;
using UnityEngine;

public enum MineType { Blue, Red };

public class MineSpawner : AbstractSpawner {

    [SerializeField] GameObject explosionVFX;
    [SerializeField] SoundController explosionSoundCtrlr;
    [SerializeField] GameObject blueMinePrefab;
    [SerializeField] GameObject redMinePrefab;

    protected override void UpdateIsSpwaningDuringThisLevel() {
        isSpwaningDuringThisLevel = (levelSettings.mineMinWait >= 0 && levelSettings.mineMaxWait > 0);
    }

    protected override IEnumerator SpawningRoutine() {
        WaitForSeconds waitOneSec = new WaitForSeconds(1f);
        while (true) {
            if (isSpwaningDuringThisLevel) {
                yield return new WaitForSeconds(Random.Range(levelSettings.mineMinWait, levelSettings.mineMaxWait));
                MineType type = (Random.value > .5f) ? MineType.Blue : MineType.Red;
                SpawnMine(type, levelSettings.mineMinSize, levelSettings.mineMaxSize, levelSettings.mineMinSpeed, levelSettings.mineMaxSpeed);
                if (transform.position.y >= 800)
                    SpawnMine(type, levelSettings.mineMinSize, levelSettings.mineMaxSize, levelSettings.mineMinSpeed, levelSettings.mineMaxSpeed);
                if (transform.position.y >= 1600)
                    SpawnMine(type, levelSettings.mineMinSize, levelSettings.mineMaxSize, levelSettings.mineMinSpeed, levelSettings.mineMaxSpeed);
                if (transform.position.y >= 2500)
                    SpawnMine(type, levelSettings.mineMinSize, levelSettings.mineMaxSize, levelSettings.mineMinSpeed, levelSettings.mineMaxSpeed);
            }
            else {
                yield return waitOneSec;
            }
        }
    }

    public GameObject SpawnMine(MineType type, float minScale, float maxScale, float minSpeed, float maxSpeed) {
        // Position
        Vector3 pos = transform.position;
        pos.x = Random.Range(minPosX, maxPosX);

        // Init
        GameObject mine;
        if (type == MineType.Blue)
            mine = Instantiate(blueMinePrefab, pos, Quaternion.identity);
        else
            mine = Instantiate(redMinePrefab, pos, Quaternion.identity);
        MineController mineCtrlr = mine.GetComponent<MineController>();
        mineCtrlr.explosionVFX = explosionVFX;
        mineCtrlr.explosionSoundCtrlr = explosionSoundCtrlr;

        // Rotation
        mine.transform.localRotation = Random.rotation;

        // Size
        Vector3 scale = mine.transform.localScale;
        scale *= Random.Range(minScale, maxScale);
        mine.transform.localScale = scale;

        // Speed
        Rigidbody2D rb = mine.GetComponent<Rigidbody2D>();
        rb.gravityScale *= Random.Range(minSpeed, maxSpeed);

        return mine;
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
            MineType type;
            switch (burstType) {
                case BurstType.BlueMine:
                    type = MineType.Blue;
                    break;
                case BurstType.RedMine:
                    type = MineType.Red;
                    break;
                default:
                    type = (Random.value > .5f) ? MineType.Blue : MineType.Red;
                    break;
            }
            SpawnMine(type, levelSettings.mineMinSize, levelSettings.mineMaxSize, levelSettings.mineMinSpeed, levelSettings.mineMaxSpeed);
            nbSpawned++;
            yield return waitingTime;
        }
    }

}
