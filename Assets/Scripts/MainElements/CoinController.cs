using UnityEngine;

public class CoinController : MonoBehaviour {

    public SoundController coinSoundCtrlr;
    public int value = 1;

    void Start() {
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            OnHitPlayer(collision.gameObject);
        }
    }

    void OnHitPlayer(GameObject player) {
        ApplicationController.ac.UpateCoins(value);
        coinSoundCtrlr.Play();
        Destroy(this.gameObject);
    }
}
