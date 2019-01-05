using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CameraController : MonoBehaviour {

    [SerializeField] GameObject player;
    // [SerializeField] float smoothingInterpolateTime = 0.1f;
    Vector3 initialPosition;
    Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
        initialPosition = transform.position;
    }

    void OnEnable() {
        InitPosition();
    }

    public void InitPosition() {
        transform.position = initialPosition;
    }

    // LateUpdate is called after Update
    void LateUpdate() {
        float yPlayer = player.transform.position.y;
        if (yPlayer > transform.position.y) {
            Vector3 target = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            //transform.position = Vector3.Lerp(transform.position, target, smoothingInterpolateTime);
            transform.position = target;
        }
    }

    public void GoToMenu() {
        anim.SetTrigger("goToMenu");
    }

    public void GoToPlayer() {
        anim.SetTrigger("goToPlayer");
    }

}
