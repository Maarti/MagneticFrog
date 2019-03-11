using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class MainMenuController : MonoBehaviour {

    [SerializeField] Text bestScoreText;
    [SerializeField] GameObject gameTitleCanvas;
    [SerializeField] AudioSource playClickSound;
    Animator mainMenuAnim;
    bool isStarted = false;

    private void Start() {
        mainMenuAnim = GetComponent<Animator>();
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

    public void IntroAnimation() {
        mainMenuAnim.SetTrigger("play");
        // StartGame() is called at the end of the animation
    }

    public void StartGame() {
        playClickSound.Play();
        GameController.gc.StartGame();
    }


}
