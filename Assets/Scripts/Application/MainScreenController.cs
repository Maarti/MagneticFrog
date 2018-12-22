using UnityEngine;
using UnityEngine.UI;

public class MainScreenController : MonoBehaviour {

    [SerializeField] Text bestScoreText;
    bool isStarted = false;

    private void Start() {
        isStarted = true;
        OnEnable();
    }
    private void OnEnable() {
        if (!isStarted) return;
        bestScoreText.text = ApplicationController.ac.PlayerData.bestScore.ToString();   
    }

}
