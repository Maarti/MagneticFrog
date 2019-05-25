using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerStart : MonoBehaviour {
    [SerializeField] ScreenTransition screenTransition;
    [SerializeField] PlayerController playerCtrlr;
    bool isStarted = false;

    public void Start() {
        Debug.Log("onstart");
        GameController.OnGameStart += Init;
        isStarted = true;
    }

    public void OnEnable() {
        if (!isStarted) Start();
        if (IsPowerStartAvailable())
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void Update() {
        if (playerCtrlr.transform.position.y > 10f)
            gameObject.SetActive(false);
    }

    public void OnDestroy() {
        GameController.OnGameStart -= Init;
    }

    void Init() {
        Debug.Log("init");
        gameObject.SetActive(true);
    }

    public void BeginPowerStart() {
        gameObject.SetActive(false);
        playerCtrlr.jumpCtrlr.isInvincible = true;
        screenTransition.ScreenFadeThen(EndPowerStart);
    }

    public void EndPowerStart() {
        Invoke("RemovePlayerInvincibility", 3f);
        playerCtrlr.jumpCtrlr.isInvincible = true;
        Vector3 pos = playerCtrlr.transform.position;
        float targetDistance = Mathf.Max(pos.y, ApplicationController.ac.PlayerData.bestScore * .33f);
        pos.y = targetDistance;
        playerCtrlr.transform.position = pos;
    }

    bool IsPowerStartAvailable() {
        Debug.Log("is="+ApplicationController.ac.PlayerData.bestScore);
        return ApplicationController.ac.PlayerData.bestScore >= 100f;
    }

    void RemovePlayerInvincibility() {
        playerCtrlr.jumpCtrlr.isInvincible = false;
    }
}
