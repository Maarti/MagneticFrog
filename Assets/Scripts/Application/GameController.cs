using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController gc;
    [Header("Game Over Screen")]
    [SerializeField] Canvas goCanvas;
    [SerializeField] Text goScoreText;
    [SerializeField] Text goBestScoreText;

    void Awake() {
        if (gc != this) gc = this;
        PlayerController.OnGameOver += OnGameOver;
    }

    public void OnGameOver(int newScore) {
        DisplayGameOverScreen(newScore);
        ApplicationController.ac.recordNewScore(newScore);
        ApplicationController.ac.Save();
    }

    void DisplayGameOverScreen(int score) {
        goScoreText.text = score.ToString();
        goBestScoreText.text = ApplicationController.ac.PlayerData.bestScore.ToString();        
        goCanvas.gameObject.SetActive(true);
    }

    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
