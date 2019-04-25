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

                // Color
                MineType type = (Random.value > .5f) ? MineType.Blue : MineType.Red;
                
                /* GameObject mine;
                MineController mineCtrlr;
                if (Random.value > .5f) {
                    mine = SpawnBlueMine(pos);
                    mineCtrlr = mine.GetComponent<MineController>();
                }
                else {
                    mine = SpawnRedMine(pos);                     
                    mineCtrlr = mine.GetComponent<MineController>();
                }
                // Init
                mineCtrlr.explosionVFX = explosionVFX;
                mineCtrlr.explosionSoundCtrlr = explosionSoundCtrlr;

                // Rotation
                mine.transform.localRotation = Random.rotation;
                
                // Size
                Vector3 scale = mine.transform.localScale;
                scale *= Random.Range(levelSettings.mineMinSize, levelSettings.mineMaxSize);
                mine.transform.localScale = scale;

                // Speed
                Rigidbody2D rb = mine.GetComponent<Rigidbody2D>();
                rb.gravityScale *= Random.Range(levelSettings.mineMinSpeed, levelSettings.mineMaxSpeed);*/

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
        if(type==MineType.Blue)
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

}
