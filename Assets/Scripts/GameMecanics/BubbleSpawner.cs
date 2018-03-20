using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {

    public GameObject blueBubblePrefab, redBubblePrefab;
    public float minPosX = -2f, maxPosX = 2f;


	void Start () {
        StartCoroutine(SpawningRoutine());
	}

    IEnumerator SpawningRoutine() {
        while (true) {
            yield return new WaitForSeconds(2f);
            Vector3 pos = transform.position;
            pos.x = Random.Range(minPosX, maxPosX);
            if (Random.value > .5f)
                Instantiate(blueBubblePrefab, pos, Quaternion.identity);
            else
                Instantiate(redBubblePrefab, pos, Quaternion.identity);
        }
    }
}
