using UnityEngine;
using DG.Tweening;

public class BubbleController : MonoBehaviour {

    public SoundController bubbleSoundCtrlr;
    public ElementType type;
    [SerializeField] float oxygenAmount = 8f;
    [SerializeField] SpriteRenderer renderer;

    void Start() {
        Invoke("Destroy", 15f);
    }

    void Awake() {
        renderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            OnHitPlayer(collision.gameObject);
        }
    }

    void OnHitPlayer(GameObject player) {
        player.GetComponent<OxygenController>().AddOxygen(oxygenAmount);
        GameController.gc.StartCollectedBubbleAnimation(type, transform.position);
        Destroy(gameObject);
    }
    
    void Destroy() {
        renderer.DOColor(new Color(1, 1, 1, 0), 2f);
        Destroy(gameObject, 2f);
    }
}
