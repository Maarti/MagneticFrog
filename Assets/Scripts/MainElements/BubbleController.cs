using UnityEngine;

public class BubbleController : MonoBehaviour {

    public SoundController bubbleSoundCtrlr;
    [SerializeField] float oxygenAmount = 8f;
    Renderer render;

    void Start() {
        Destroy(gameObject, 15f);
    }

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
        bubbleSoundCtrlr.Play();
        Destroy(this.gameObject);
    }

    public void SetColor(Color color) {
        if (render != null)
            render.material.SetColor("_Color", color);
    }
}
