using UnityEngine;

public class PowerStart : MonoBehaviour {
    [SerializeField] ScreenTransition screenTransition;
    [SerializeField] PlayerController playerCtrlr;
    bool isStarted = false;

    public void Start() {
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
        gameObject.SetActive(true);
    }

    public void BeginPowerStart() {
        if (ApplicationController.ac.PlayerData.nbPowerStart <= 0) return;
        ApplicationController.ac.UpdatePowerStart(-1);
        gameObject.SetActive(false);
        playerCtrlr.jumpCtrlr.isInvincible = true;
        screenTransition.ScreenFadeThen(EndPowerStart);
    }

    public void EndPowerStart() {
        Invoke("RemovePlayerInvincibility", 3f);
        playerCtrlr.jumpCtrlr.isInvincible = true;
        Vector3 pos = playerCtrlr.transform.position;
        float targetDistance =  ApplicationController.ac.PlayerData.bestScore * Random.Range(.33f,.45f);
        pos.y = Mathf.Max(pos.y,targetDistance);
        playerCtrlr.transform.position = pos;
    }

    bool IsPowerStartAvailable() {
        if (ApplicationController.ac.PlayerData.isPremium) return true;
       // System.TimeSpan timeSinceLastActivation = System.DateTime.Now.Subtract(ApplicationController.ac.PlayerData.bonusesActivationTime);
        return (ApplicationController.ac.PlayerData.bestScore >= 100f) && (ApplicationController.ac.PlayerData.nbPowerStart > 0);
    }

    void RemovePlayerInvincibility() {
        playerCtrlr.jumpCtrlr.isInvincible = false;
    }
}
