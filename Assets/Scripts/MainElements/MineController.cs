using UnityEngine;

public class MineController : MonoBehaviour {

    [SerializeField] float stunDuration = 1f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            OnHitPlayer(collision.gameObject);
        }
    }

    void OnHitPlayer(GameObject player) {
        player.GetComponent<JumpController>().Stun(stunDuration);
        Destroy(this.gameObject);
    }
}
