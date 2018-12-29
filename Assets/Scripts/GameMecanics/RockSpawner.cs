using System.Collections;
using UnityEngine;

public class RockSpawner : AbstractSpawner {

    [SerializeField] GameObject rockPrefab;

    protected override void UpdateIsSpwaningDuringThisLevel() {
        isSpwaningDuringThisLevel = (levelSettings.rockMinWait >= 0 && levelSettings.rockMaxWait > 0);
    }

    protected override IEnumerator SpawningRoutine() {
        WaitForSeconds waitOneSec = new WaitForSeconds(1f);
        while (true) {
            if (isSpwaningDuringThisLevel) {
                yield return new WaitForSeconds(Random.Range(levelSettings.rockMinWait, levelSettings.rockMaxWait));
                
                // Position
                Vector3 pos = transform.position;
                pos.x = Random.Range(minPosX, maxPosX);
                
                // Type
                GameObject rock;
               /* if (Random.value > .5f)
                    rock = Instantiate(rockPrefab, pos, Quaternion.identity);
                else*/
                    rock = Instantiate(rockPrefab, pos, Quaternion.identity);
                
                // Size
                Vector3 scale = rock.transform.localScale;
                scale *= Random.Range(levelSettings.rockMinSize, levelSettings.rockMaxSize);
                rock.transform.localScale = scale;

                // Speed
                Rigidbody2D rb = rock.GetComponent<Rigidbody2D>();
                rb.gravityScale *= Random.Range(levelSettings.rockMinSpeed, levelSettings.rockMaxSpeed);

            }
            else {
                yield return waitOneSec;
            }
        }
    }


}
