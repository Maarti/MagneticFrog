using UnityEngine;

public class MagnetController : MonoBehaviour {

    [SerializeField] RectTransform magnetCtrlrLayout;
    [SerializeField] GameObject redMagnet, blueMagnet;
    [SerializeField] SpriteRenderer magnetSprite;
    bool magnetIsRed = true;                    // True = red (+)  False = blue (-)

    public void Init() {
        SwitchMagnetToRed();
    }

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
        if (pointerY < magnetCtrlrLayout.position.y && magnetIsRed)
            SwitchMagnetToBlue();
        else if (pointerY > magnetCtrlrLayout.position.y && !magnetIsRed)
            SwitchMagnetToRed();
    }

    void SwitchMagnetToRed() {
        redMagnet.SetActive(true);
        blueMagnet.SetActive(false);
        magnetSprite.color = Color.red;
        magnetIsRed = true;
    }

    void SwitchMagnetToBlue() {
        redMagnet.SetActive(false);
        blueMagnet.SetActive(true);
        magnetSprite.color = Color.blue;
        magnetIsRed = false;
    }
}
