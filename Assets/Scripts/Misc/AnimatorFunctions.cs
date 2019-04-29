using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorFunctions : MonoBehaviour {

    [SerializeField] SoundController bubbleSoundCtrlr;
    Animator anim;

    void Awake() {
        anim = GetComponent<Animator>();
    }

    public void SetTrigger(AnimatorTrigger triggerValue) {
        switch (triggerValue) {
            case AnimatorTrigger.Touch:
                anim.SetTrigger("touch");
                break;
        }
    }

    public void PlayBubbleSound() {
        bubbleSoundCtrlr.Play();
    }

}

public enum AnimatorTrigger { Touch };
