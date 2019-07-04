using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

enum CurrentMenu { Main, Settings, Character }

[RequireComponent(typeof(Animator))]
public class MainMenuController : MonoBehaviour {

    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] GameObject gameTitleCanvas;
    [SerializeField] GameObject scoreSign;
    [SerializeField] AudioSource playClickSound;
    [SerializeField] Animator scoreShellAnim;
    [Header("Game Title")]
    [SerializeField] CanvasGroup gameTitleCanvasGroup;
    [Header("Main Menu")]
    [SerializeField] CanvasGroup mainMenuCanvasGroup;
    [Header("Character Menu")]
    [SerializeField] RectTransform characterMenu;
    [Header("Settings Menu")]
    [SerializeField] CanvasGroup settingsCanvasGroup;
    [Header("Menu Coin")]
    [SerializeField] SoundController coinSound;

    Animator mainMenuAnim;
    bool isStarted = false;
    CurrentMenu currentMenu = CurrentMenu.Main;

    private void Start() {
        mainMenuAnim = GetComponent<Animator>();
        GameController.OnGameOver += OnGameOver;
        isStarted = true;
        OnEnable();
        ApplicationController.OnLoad += RefreshScoreDisplay;
    }

    void OnDestroy() {
        GameController.OnGameOver -= OnGameOver;
    }

    private void OnEnable() {
        if (!isStarted) return;
        RefreshScoreDisplay();
        RefreshMenuCoin();
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
        currentMenu = CurrentMenu.Character;
        characterMenu.localScale = new Vector3(0f, 0f, 1f);
        characterMenu.DOScale(Vector3.one, .25f);
        mainMenuCanvasGroup.DOFade(0f, .25f).OnComplete(() => mainMenuCanvasGroup.transform.parent.gameObject.SetActive(currentMenu == CurrentMenu.Main));
        //   gameTitleCanvasGroup.DOFade(0f, .25f);
        scoreShellAnim.SetBool("isDisplayed", false);
    }

    public void HideCharacterMenu() {
        currentMenu = CurrentMenu.Main;
        characterMenu.localScale = Vector3.one;
        gameTitleCanvasGroup.alpha = 1f;
        mainMenuCanvasGroup.transform.parent.gameObject.SetActive(true);
        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.DOFade(1f, 1f);
        scoreShellAnim.SetBool("isDisplayed", true);
        characterMenu.DOScale(new Vector3(0f, 0f, 1f), .25f).OnComplete(() => characterMenu.transform.parent.gameObject.SetActive(currentMenu == CurrentMenu.Character));
    }

    public void DisplaySettingsMenu() {
        currentMenu = CurrentMenu.Settings;
        gameTitleCanvasGroup.DOFade(0f, .5f);
        mainMenuCanvasGroup.DOFade(0f, .5f).OnComplete(() => mainMenuCanvasGroup.transform.parent.gameObject.SetActive(currentMenu == CurrentMenu.Main));
        settingsCanvasGroup.gameObject.SetActive(true);
        settingsCanvasGroup.alpha = 0f;
        settingsCanvasGroup.DOFade(1f, .5f);
    }

    public void HideSettingsMenu() {
        currentMenu = CurrentMenu.Main;
        gameTitleCanvasGroup.DOFade(1f, .5f);
        mainMenuCanvasGroup.transform.parent.gameObject.SetActive(true);
        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.DOFade(1f, .5f);
        settingsCanvasGroup.DOFade(0f, .5f).OnComplete(() => settingsCanvasGroup.gameObject.SetActive(currentMenu == CurrentMenu.Settings));
    }

    //----------------------
    // Menu Coin
    void RefreshMenuCoin() {
        bool isCoinDisplayed = IsMenuCoinDisplayed();
        scoreShellAnim.SetBool("coinIsDisplayed", isCoinDisplayed);
    }

    bool IsMenuCoinDisplayed() {
        TimeSpan timeSinceLastCoin = DateTime.Now - ApplicationController.ac.PlayerData.lastMenuCoin;
        return timeSinceLastCoin.TotalMinutes > 10f;
    }

    public void CollectMenuCoin() {
        ApplicationController.ac.CollectMenuCoin();
        ApplicationController.ac.UpdateCoins(1);
        scoreShellAnim.SetBool("coinIsDisplayed", false);
        ApplicationController.ac.Save();
        coinSound.Play();
    }

}
