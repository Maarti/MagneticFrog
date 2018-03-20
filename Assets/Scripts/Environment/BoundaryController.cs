using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundaryController : MonoBehaviour {

    private void OnTriggerExit2D(Collider2D collision) {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Player") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        } else if (obj.tag == "MainElement")
            Destroy(obj);
        //else if (obj.tag == "SecondaryElement")
            //reset
        else
            Destroy(obj);
    }
}
