using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PowerStartMainMenu : MonoBehaviour {
    [SerializeField] TMP_Text timerText;
    [SerializeField] GameObject timerIcon;
    bool isStarted = false;

    /*public void OnEnable() {
        if (ShouldPowerStart())
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }*/

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
