using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
public class MainMenuController : MonoBehaviour {

    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] GameObject gameTitleCanvas;
    [SerializeField] GameObject scoreSign;
    [SerializeField] AudioSource playClickSound;
    [Header("Game Title")]
    [SerializeField] CanvasGroup gameTitleCanvasGroup;
    [Header("Main Menu")]
    [SerializeField] CanvasGroup mainMenuCanvasGroup;
    [Header("Character Menu")]
    [SerializeField] RectTransform characterMenu;
    [Header("Settings Menu")]
    [SerializeField] CanvasGroup settingsCanvasGroup;

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

    public void DisplayCharacterMenu() {
        characterMenu.localScale = new Vector3(0f, 0f, 1f);
        characterMenu.DOScale(Vector3.one, .25f);
        mainMenuCanvasGroup.DOFade(0f, .25f).OnComplete(()=>mainMenuCanvasGroup.transform.parent.gameObject.SetActive(false));
     //   gameTitleCanvasGroup.DOFade(0f, .25f);
    }

    public void HideCharacterMenu() {
        characterMenu.localScale = Vector3.one;
        gameTitleCanvasGroup.alpha = 1f;
        mainMenuCanvasGroup.transform.parent.gameObject.SetActive(true);
        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.DOFade(1f, 1f);
        characterMenu.DOScale(new Vector3(0f, 0f, 1f), .25f).OnComplete(() => characterMenu.transform.parent.gameObject.SetActive(false));
    }

    public void DisplaySettingsMenu() {        
        gameTitleCanvasGroup.DOFade(0f, .5f);
        mainMenuCanvasGroup.DOFade(0f, .5f).OnComplete(() => mainMenuCanvasGroup.transform.parent.gameObject.SetActive(false));
        settingsCanvasGroup.gameObject.SetActive(true);
        settingsCanvasGroup.alpha = 0f;
        settingsCanvasGroup.DOFade(1f, .5f);
    }

    public void HideSettingsMenu() {        
        gameTitleCanvasGroup.DOFade(1f, .5f);
        mainMenuCanvasGroup.transform.parent.gameObject.SetActive(true);
        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.DOFade(1f, .5f);
        settingsCanvasGroup.DOFade(0f, .5f).OnComplete(() => settingsCanvasGroup.gameObject.SetActive(false));
        
    }



}
