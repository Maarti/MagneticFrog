using System;
using UnityEngine;

public class ScreenTransition : MonoBehaviour {

    Animator anim;
    Action actionAfterFade;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void ScreenFadeThen(Action action, float speed = 1f) {
        gameObject.SetActive(true);
        actionAfterFade = action;
        anim.SetTrigger("fadeOut");
    }

    // Called by the animator
    void FadeOutFinished() {
        if (actionAfterFade != null)
            actionAfterFade();
        anim.SetTrigger("fadeIn");
    }

    // Called by the animator
    void FadeInFinished() {
        gameObject.SetActive(false);
    }

}
