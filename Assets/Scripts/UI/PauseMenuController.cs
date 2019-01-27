using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class PauseMenuController : MonoBehaviour {

    [SerializeField] GameController gameCtrlr;
    [SerializeField] AudioSource mainMusic;
    [SerializeField] float pausePitch = .9f;
    public static bool GameIsPaused = false;
    GraphicRaycaster graphicRay;

    void Awake() {
        graphicRay = GetComponent<GraphicRaycaster>();
    }

    void OnEnable() {
        graphicRay.enabled = true;
        Time.timeScale = 0f;
        mainMusic.pitch = pausePitch;
    }

    void OnDisable() {
        Time.timeScale = 1f;
        mainMusic.pitch = 1f;
    }

    public void Pause() {
        gameObject.SetActive(true);
        GameIsPaused = true;
    }

    public void Resume() {
        gameObject.SetActive(false);
    }

    public void Restart() {
        graphicRay.enabled = false;
        Time.timeScale = 1f;
        mainMusic.pitch = 1f;
        gameCtrlr.StopGame();
        gameCtrlr.PlayAgain();
    }

    public void GoToHomeScreen() {
        graphicRay.enabled = false;
        Time.timeScale = 1f;
        mainMusic.pitch = 1f;
        gameCtrlr.StopGame();
        gameCtrlr.GoToHomeScreen();
    }

}
