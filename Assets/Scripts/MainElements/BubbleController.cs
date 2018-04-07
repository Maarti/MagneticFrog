using UnityEngine;

public class BubbleController : MonoBehaviour {

    [SerializeField]
    float oxygenAmount = 8f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            OnHitPlayer(collision.gameObject);
        }
    }

    void OnHitPlayer(GameObject player) {
        player.GetComponent<PlayerController>().AddOxygen(oxygenAmount);
        Destroy(this.gameObject);
    }
}
