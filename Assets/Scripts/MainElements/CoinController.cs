using UnityEngine;

public class CoinController : MonoBehaviour {

    public int value = 1;

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            OnHitPlayer(collision.gameObject);
        }
    }

    void OnHitPlayer(GameObject player) {
        ApplicationController.ac.UpateCoins(value);
        Destroy(this.gameObject);
    }
}
