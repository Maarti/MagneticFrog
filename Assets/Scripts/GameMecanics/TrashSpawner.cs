using System.Collections;
using UnityEngine;

public class TrashSpawner : MonoBehaviour {

    [SerializeField] GameObject rockPrefab1;
    [SerializeField] GameObject rockPrefab2;
    [SerializeField] float minPosX = -2;
    [SerializeField] float maxPosX = 2;

    protected  IEnumerator SpawningRoutine() {
        WaitForSeconds waitOneSec = new WaitForSeconds(1f);
        while (true) {
            yield return new WaitForSeconds(1f);
            SpawnOne(1f, 1f, 1f, 1f);
        }
    }

    void Start() {
        StartCoroutine(SpawningRoutine());
    }

    GameObject SpawnOne(float minScale, float maxScale, float minSpeed, float maxSpeed) {
        // Position
        Vector3 pos = transform.position;
        pos.x = Random.Range(minPosX, maxPosX);

        // Type
        GameObject rock;
        float rand = Random.value;
        if (rand > .5f) {
            rock = Instantiate(rockPrefab1, pos, Quaternion.identity);
            rock.transform.rotation = Random.rotation;
        }
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

        // Rotation
        rb.AddTorque(Random.Range(-1f,1f));

        return rock;
    }
    


}
