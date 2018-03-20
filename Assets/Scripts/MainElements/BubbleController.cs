using UnityEngine;

public class BubbleController : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            OnHitPlayer();
        }
    }

    void OnHitPlayer() {
        Destroy(this.gameObject);
    }
}
