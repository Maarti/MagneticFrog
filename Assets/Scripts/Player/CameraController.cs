using UnityEngine;

public class CameraController : MonoBehaviour {

    GameObject player;
    [SerializeField]
    float smoothingInterpolateTime = 0.1f;

    void OnEnable() {
        Init(); 
    }

    void Init() {
        Debug.Log("camera init");
        player = GameObject.FindGameObjectWithTag("Player");
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
}
