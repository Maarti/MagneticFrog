using UnityEngine;

public class PauseMenuController : MonoBehaviour {

    [SerializeField] GameController gameCtrlr;
    public static bool GameIsPaused = false;

    void OnEnable() {
        Time.timeScale = 0f;
    }

    void OnDisable() {
        Time.timeScale = 1f;
    }

    public void Pause() {
        gameObject.SetActive(true);
        GameIsPaused = true;
    }

    public void Resume() {
        gameObject.SetActive(false);
    }

    public void Restart() {
        gameObject.SetActive(false);
        gameCtrlr.StopGame();
        gameCtrlr.PlayAgain();
    }

    public void GoToHomeScreen() {
        gameObject.SetActive(false);
        gameCtrlr.StopGame();
        gameCtrlr.GoToHomeScreen();
    }

}
