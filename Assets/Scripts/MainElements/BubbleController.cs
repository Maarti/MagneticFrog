using UnityEngine;

public class BubbleController : MonoBehaviour {

    [SerializeField] float oxygenAmount = 8f;
    Renderer render;

    void Awake() {
        render = GetComponentInChildren<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            OnHitPlayer(collision.gameObject);
        }
    }

    void OnHitPlayer(GameObject player) {
        player.GetComponent<OxygenController>().AddOxygen(oxygenAmount);
        Destroy(this.gameObject);
    }

    public void SetColor(Color color) {
        if (render != null)
            render.material.SetColor("_Color", color);
    }
}
