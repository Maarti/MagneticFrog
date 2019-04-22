using UnityEngine;

public class TutorialMagnetUIController : MonoBehaviour {

    [SerializeField] Animator tutoMagnetAnim;
    [SerializeField] RectTransform magnetCtrlrLayout;

    void Update() {
        float pointerY;     // Y position of the cursor or touch

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0) {
            pointerY = Input.GetTouch(0).position.y;
            ManageMagnetState(pointerY);  
        }
#else
        pointerY = Input.mousePosition.y;
        ManageMagnetState(pointerY);
#endif
    }

    // Change magnet state according to pointer position
    void ManageMagnetState(float pointerY) {
        Debug.Log(pointerY + "  " + magnetCtrlrLayout.position.y);
        if (pointerY < magnetCtrlrLayout.position.y)
            tutoMagnetAnim.SetBool("isRed", false);
        else
            tutoMagnetAnim.SetBool("isRed", true);            
    }
}
