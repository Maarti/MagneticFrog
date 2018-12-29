using System.Collections;
using UnityEngine;

public class MineSpawner : AbstractSpawner {

    [SerializeField] GameObject blueMinePrefab, redMinePrefab;

    protected override void UpdateIsSpwaningDuringThisLevel() {
        isSpwaningDuringThisLevel = (levelSettings.mineMinWait >= 0 && levelSettings.mineMaxWait > 0);
    }

    protected override IEnumerator SpawningRoutine() {
        WaitForSeconds waitOneSec = new WaitForSeconds(1f);
        while (true) {
            if (isSpwaningDuringThisLevel) {
                yield return new WaitForSeconds(Random.Range(levelSettings.mineMinWait, levelSettings.mineMaxWait));
                
                // Position
                Vector3 pos = transform.position;
                pos.x = Random.Range(minPosX, maxPosX);
                
                // Type
                GameObject mine;
                if (Random.value > .5f)
                    mine = Instantiate(blueMinePrefab, pos, Quaternion.identity);
                else
                    mine = Instantiate(redMinePrefab, pos, Quaternion.identity);
                
                // Size
                Vector3 scale = mine.transform.localScale;
                scale *= Random.Range(levelSettings.mineMinSize, levelSettings.mineMaxSize);
                mine.transform.localScale = scale;

                // Speed
                Rigidbody2D rb = mine.GetComponent<Rigidbody2D>();
                rb.gravityScale *= Random.Range(levelSettings.mineMinSpeed, levelSettings.mineMaxSpeed);

            }
            else {
                yield return waitOneSec;
            }
        }
    }


}
