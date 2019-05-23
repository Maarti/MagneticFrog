using UnityEngine;

public class CoinController : MonoBehaviour {

    public SoundController coinSoundCtrlr;
    public int value = 1;
    public Camera cam;
    public RectTransform coinIndicator;
    public Animator coinIndicatorAnim;

    void Start() {
        DisplayCoinIndicator();
        Destroy(gameObject, 10f);
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            OnHitPlayer(collision.gameObject);
        }
    }

    void OnHitPlayer(GameObject player) {
        ApplicationController.ac.UpdateCoins(value);
        coinSoundCtrlr.Play();
        Destroy(gameObject);
        Achievement.Unlock(GPGSIds.achievement_coin_collector_1);
        Achievement.Increment(GPGSIds.achievement_coin_collector_20, 1);
        Achievement.Increment(GPGSIds.achievement_coin_collector_50, 1);
        Achievement.Increment(GPGSIds.achievement_coin_collector_100, 1);
        Achievement.Increment(GPGSIds.achievement_coin_collector_200, 1);
        Achievement.Increment(GPGSIds.achievement_coin_collector_500, 1);
        Achievement.Increment(GPGSIds.achievement_coin_collector_1000, 1);
    }

    void DisplayCoinIndicator() {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        Vector3 newIndicatorPos = coinIndicator.position;
        newIndicatorPos.x = screenPos.x;
        coinIndicator.position = newIndicatorPos;
        coinIndicatorAnim.SetTrigger("shine");
    }
}
