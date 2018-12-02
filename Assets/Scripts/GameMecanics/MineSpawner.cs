using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineSpawner : MonoBehaviour {

    public GameObject blueMinePrefab, redMinePrefab;
    public float minPosX = -2f, maxPosX = 2f;
    public float minWait = 2f, maxWait = 4f;
    public float minSize = .8f, maxSize = 1.5f;

	void Start () {
        StartCoroutine(SpawningRoutine());
	}

    IEnumerator SpawningRoutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minWait, maxWait));
            Vector3 pos = transform.position;
            pos.x = Random.Range(minPosX, maxPosX);
            GameObject mine;
            if (Random.value > .5f)
                mine = Instantiate(blueMinePrefab, pos, Quaternion.identity);
            else
                mine = Instantiate(redMinePrefab, pos, Quaternion.identity);
            Vector3 scale = mine.transform.localScale;
            scale *= Random.Range(minSize, maxSize);
            mine.transform.localScale = scale;
        }
    }
}
