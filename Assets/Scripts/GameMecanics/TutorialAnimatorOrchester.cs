using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TutorialAnimatorOrchester : MonoBehaviour {

    [SerializeField] GameObject tutorialMagnetAnimation;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject magnetTutoConfirmButton;
    [SerializeField] JumpController jumpCtrlr;
    [SerializeField] TutorialManager tutorialManager;
    [SerializeField] Animator tutoMagnetAnim;
    [SerializeField] GameObject[] tutos;
    Animator anim;
    bool clickedOnRedMagnet = false;
    bool clickedOnBlueMagnet = false;

    void Awake() {
        anim = GetComponent<Animator>();
        clickedOnRedMagnet = false;
        clickedOnBlueMagnet = false;
        magnetTutoConfirmButton.SetActive(false);
    }

    void OnDisable() {
        // Be sure that all children are disabled (because when animator is disabled, objects are kept in their current state)
        foreach(GameObject tuto in tutos) {
            tuto.SetActive(false);
        }
    }

    public void PauseGame() {
        Time.timeScale = 0f;
    }

    public void UnpauseGame() {
        Time.timeScale = 1f;
    }

    public void TriggerTouchAnimation() {
        anim.SetTrigger("touch");
    }

    public void EnableMagnetAnimation() {
        tutorialMagnetAnimation.SetActive(true);
    }

    public void DisableMagnetAnimation() {
        tutorialMagnetAnimation.SetActive(false);
    }

    public void SetAnimatorState(int state) {
        anim.SetInteger("state", state);
    }

    public void HideGameUI() {
        gameUI.SetActive(false);
    }

    public void ShowGameUI() {
        gameUI.SetActive(true);
    }

    public void DisableJump() {
        jumpCtrlr.enabled = false;
    }

    public void EnableJump() {
        jumpCtrlr.enabled = true;
    }

    public void TutorialManagerNextState() {
        tutorialManager.NextState();
    }

    public void SetMagnetAnimToBlue() {
        tutoMagnetAnim.SetBool("isRed", false);        
        clickedOnBlueMagnet = true;
        CheckIfConfirmButtonShouldBeDisplayed();
    }

    public void SetMagnetAnimToRed() {
        tutoMagnetAnim.SetBool("isRed", true);
        clickedOnRedMagnet = true;
        CheckIfConfirmButtonShouldBeDisplayed();
    }

    public void CheckIfConfirmButtonShouldBeDisplayed() {
        if (clickedOnBlueMagnet && clickedOnRedMagnet)
            magnetTutoConfirmButton.SetActive(true);
    }

}
