using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TutorialAnimatorOrchester : MonoBehaviour {

    Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
    }

    public void TriggerTouchAnimation() {
        anim.SetTrigger("touch");
    }
}
