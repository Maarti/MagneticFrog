using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    void OnEnable() {
        Invoke("DisableSelf", 1f);
    }

    void DisableSelf() {
        gameObject.SetActive(false);
    }
}
