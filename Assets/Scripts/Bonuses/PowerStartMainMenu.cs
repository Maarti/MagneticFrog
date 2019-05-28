using UnityEngine;
using TMPro;
using System;

public class PowerStartMainMenu : MonoBehaviour {
    [SerializeField] TextMeshProUGUI powerStartText;
    [SerializeField] GameObject powerStartButton;

    private void OnEnable() {
        RefreshUI();
    }

    public void RefreshUI() {
        if (ShouldPowerStartBeDisplayed()) {
            powerStartButton.SetActive(true);
            powerStartText.gameObject.SetActive(true);
            powerStartText.text = "x " + ApplicationController.ac.PlayerData.nbPowerStart;
            // TODO switch color to red if zero
        }
        else {
            powerStartButton.SetActive(false);
            powerStartText.gameObject.SetActive(false);
        }
    }

    /*void Update() {
        TimeSpan timeSinceLastActivation = DateTime.Now.Subtract(ApplicationController.ac.PlayerData.bonusesActivationTime);
        if (timeSinceLastActivation.TotalMinutes <= 10f) {
            if (!timerText.gameObject.activeSelf) {
                timerIcon.SetActive(true);
            }
            timerText.text = Math.Ceiling(10f - timeSinceLastActivation.TotalMinutes) + "m";
        }
        else {
            if (timerText.gameObject.activeSelf) {
                timerText.gameObject.SetActive(false);
                timerIcon.SetActive(false);
            }
        }
    }*/

    bool ShouldPowerStartBeDisplayed() {
        return ApplicationController.ac != null && ApplicationController.ac.PlayerData.bestScore >= 100f && !ApplicationController.ac.PlayerData.isPremium;
    }



}
