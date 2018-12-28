using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

    [SerializeField] Text bestScoreText;
    [SerializeField] GameObject gameTitleCanvas;
    bool isStarted = false;

    private void Start() {
        isStarted = true;
        OnEnable();
    }

    private void OnEnable() {
        if (!isStarted) return;
        if (ApplicationController.ac.PlayerData.bestScore >= 0) {
            bestScoreText.text = ApplicationController.ac.PlayerData.bestScore.ToString();
            bestScoreText.gameObject.SetActive(true);
        }
        else {
            bestScoreText.gameObject.SetActive(false);
        }
        gameTitleCanvas.SetActive(true);
    }

    private void OnDisable() {
        if (gameTitleCanvas != null)
            gameTitleCanvas.SetActive(false);
    }

}
