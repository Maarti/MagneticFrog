using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject blueBubblePrefab, redBubblePrefab;
    public float minPosX = -2f, maxPosX = 2f;
    public float minWait = 1.5f, maxwait = 3f;


    void OnEnable () {
        StartCoroutine(SpawningRoutine());
	}

    private void OnDisable() {
        StopAllCoroutines();
    }

    IEnumerator SpawningRoutine() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(minWait, maxwait));
            Vector3 pos = transform.position;
            pos.x = Random.Range(minPosX, maxPosX);
            if (Random.value > .5f)
                Instantiate(blueBubblePrefab, pos, Quaternion.identity);
            else
                Instantiate(redBubblePrefab, pos, Quaternion.identity);
        }
    }

   
}
