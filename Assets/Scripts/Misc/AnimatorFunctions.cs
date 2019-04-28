using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorFunctions : MonoBehaviour {

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

}

public enum AnimatorTrigger { Touch };
