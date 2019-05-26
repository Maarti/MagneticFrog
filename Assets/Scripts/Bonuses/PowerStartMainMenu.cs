using UnityEngine;
using TMPro;
using System;

public class PowerStartMainMenu : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject timerIcon;

    void Update() {
        TimeSpan timeSinceLastActivation = DateTime.Now.Subtract(ApplicationController.ac.PlayerData.bonusesActivationTime);
        if (timeSinceLastActivation.TotalMinutes <= 10f) {
            if (!timerText.gameObject.activeSelf) {
                timerText.gameObject.SetActive(true);
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
    }

    bool ShouldPowerStart() {
        return ApplicationController.ac.PlayerData.bestScore >= 100f;
    }



}
