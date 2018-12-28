using UnityEngine;

public class BestScoreMarker : MonoBehaviour {
    private void OnEnable() {
        if (ApplicationController.ac.PlayerData.bestScore > 0f) {
            Vector3 newPos = transform.position;
            newPos.y = ApplicationController.ac.PlayerData.bestScore+.5f;
            transform.position = newPos;
        }
        else {
            gameObject.SetActive(false);
        }
    }

}
