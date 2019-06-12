using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundaryController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Player") {
            obj.GetComponent<PlayerController>().Die();
        }
        else if (obj.CompareTag("MainElement"))
            Destroy(obj, .5f); // let some time to get out of the screen
        else if (obj.tag == "MainElement/Bubble") {

        }
        else
            Destroy(obj);
    }
}
