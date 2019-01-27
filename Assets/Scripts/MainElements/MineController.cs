using UnityEngine;

public class MineController : MonoBehaviour {

    [HideInInspector] public GameObject explosionVFX;
    [HideInInspector] public SoundController explosionSoundCtrlr;
    [SerializeField] float stunDuration = 1f;
    /*   Renderer render;

       void Awake() {
           render = GetComponentInChildren<Renderer>();
       }*/
    void Start() {
        Destroy(gameObject, 10f);
    }


    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            OnHitPlayer(collision.gameObject);
        }
    }

    void OnHitPlayer(GameObject player) {
        player.GetComponent<JumpController>().Stun(stunDuration);
        TriggerExplosion();
        Destroy(this.gameObject);
    }

    void TriggerExplosion() {
        explosionVFX.transform.position = transform.position;
        explosionVFX.SetActive(true);
        explosionSoundCtrlr.Play();
    }

   /* public void SetColor(Color color) {
        if (render != null)
            render.material.SetColor("_Color", color);
    }*/
}
