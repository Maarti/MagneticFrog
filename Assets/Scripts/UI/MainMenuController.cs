using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class MainMenuController : MonoBehaviour {

    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] GameObject gameTitleCanvas;
    [SerializeField] GameObject scoreSign;
    [SerializeField] AudioSource playClickSound;
    Animator mainMenuAnim;
    bool isStarted = false;

    private void Start() {
        mainMenuAnim = GetComponent<Animator>();
        GameController.OnGameOver += OnGameOver;
        isStarted = true;
        OnEnable();
    }
    
    void OnDestroy() {
        GameController.OnGameOver -= OnGameOver;
    }

    private void OnEnable() {
        if (!isStarted) return;
        RefreshScoreDisplay();
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
       
    void OnGameOver(int score) {
        RefreshScoreDisplay();
    }

    void RefreshScoreDisplay() {
        if (ApplicationController.ac.PlayerData.bestScore >= 0) {
            bestScoreText.text = ApplicationController.ac.PlayerData.bestScore.ToString() + "m";
            scoreSign.SetActive(true);
        }
        else {
            scoreSign.SetActive(false);
        }
    }



}
