using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CameraController : MonoBehaviour {

    [SerializeField] GameObject player;
    // [SerializeField] float smoothingInterpolateTime = 0.1f;
    Vector3 initialPosition;
    float initialSize;
    Animator anim;
    Camera cameraComp;

    void Awake() {
        anim = GetComponent<Animator>();
        initialPosition = transform.position;
        cameraComp = GetComponent<Camera>();
        initialSize = cameraComp.orthographicSize;
    }

    void OnEnable() {
        Init();
    }

    void Init() {
        InitPosition();
        anim.enabled = false;
    }

    public void InitPosition() {
        transform.position = initialPosition;
        cameraComp.orthographicSize = initialSize;
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
        anim.enabled = true;
        anim.SetTrigger("goToMenu");
    }

    public void GoToPlayer() {
        anim.enabled = true;
        anim.SetTrigger("goToPlayer");
    }

}
