using UnityEngine;
using DG.Tweening;

public class BubbleController : MonoBehaviour {

    public SoundController bubbleSoundCtrlr;
    public ElementType type;
    [SerializeField] float oxygenAmount = 8f;
    SpriteRenderer render;

    void Start() {
        Invoke("Destroy", 15f);
    }

    void Awake() {
        render = GetComponentInChildren<SpriteRenderer>();
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
        Achievement.Increment(GPGSIds.achievement_pardon_me_for_breathing_50, 1);
        Achievement.Increment(GPGSIds.achievement_pardon_me_for_breathing_200, 1);
        Achievement.Increment(GPGSIds.achievement_pardon_me_for_breathing_1000, 1);
        Achievement.Increment(GPGSIds.achievement_pardon_me_for_breathing_5000, 1);
    }
    
    void Destroy() {
        render.DOColor(new Color(1, 1, 1, 0), 2f);
        Destroy(gameObject, 2f);
    }
}
