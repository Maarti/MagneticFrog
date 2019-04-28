using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class MagnetControllerLayoutMover : MonoBehaviour {

    [SerializeField] GameObject confirmButton;
    [SerializeField] GameObject defaultButton;
    // [SerializeField] GameObject alphaSliderBackground;
    // [SerializeField] GameObject alphaSliderHandle;
    [Tooltip("Area around the slider, preventing to move the layout while interacting with the alpha slider")]
    [SerializeField] GameObject alphaPanel;
    [SerializeField] bool isMoving = false;
    [SerializeField] RectTransform parentRect;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Slider alphaSlider;
    [SerializeField] Animator anim;

    RectTransform rectTransform;
    float yMin = -550f;
    float yMax = 300f;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable() {
        KeepTrackOfDefaultMagnetControllerLayoutPosition();
        if (ApplicationController.ac.PlayerData.magnetControllerHeight != null)
            MoveLayout(ApplicationController.ac.PlayerData.magnetControllerHeight ?? -400);
        if (ApplicationController.ac.PlayerData.magnetControllerAlpha != null) {
            SetLayoutAlpha(ApplicationController.ac.PlayerData.magnetControllerAlpha ?? .5f);
            InitAlphaSliderValue();
        }
        if (isMoving && anim != null)
            anim.SetBool("handIsShowing", true);
    }

    void Update() {
        if (!isMoving || confirmButton == null) return;

        Vector2 inputController;    // Position of the cursor or finger
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0) {
            inputController = Input.GetTouch(0).position;
#else
        if (Input.GetMouseButtonDown(0)) {
            inputController = Input.mousePosition;
#endif
            // Don't move layout if we are touching the buttons
            if (!IsPointerOverUIObject(confirmButton, inputController) && !IsPointerOverUIObject(defaultButton, inputController)
                /*&& !IsPointerOverUIObject(alphaSliderBackground, inputController) && !IsPointerOverUIObject(alphaSliderHandle, inputController)*/
                && !IsPointerOverUIObject(alphaPanel, inputController)) {
                Vector2 anchorPos = ScreenPointToAnchorPos(inputController);
                MoveLayout(anchorPos.y);
                if (anim.GetBool("handIsShowing"))
                    anim.SetBool("handIsShowing", false);
            }
        }
    }

    Vector2 ScreenPointToAnchorPos(Vector2 screenPoint) {
        Vector2 anchorPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPoint, null, out anchorPos);
        // Debug.Log("ScreenPos=" + screenPoint + "   AnchorPos=" + anchorPos + " Parent=" + parentRect, parentRect);
        return anchorPos;
    }

    void MoveLayout(float yAnchoredPosition) {
        Vector3 newPos = rectTransform.anchoredPosition;
        newPos.y = Mathf.Clamp(yAnchoredPosition, yMin, yMax);
        rectTransform.anchoredPosition = newPos;
    }

    bool IsPointerOverUIObject(GameObject go, Vector2 pointerPosition) {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current) {
            position = pointerPosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach (RaycastResult res in results) {
            if (res.gameObject == go)
                return true;
        }
        return false;
    }

    public void SaveLayoutPositionAndAlpha() {
        ApplicationController.ac.SetMagnetControllerLayoutPositionAndAlpha(rectTransform.anchoredPosition.y, canvasGroup.alpha);
        ApplicationController.ac.Save();
    }

    public void SetPositionToDefault() {
        if (ApplicationController.ac.defaultMagnetControllerHeight != null) {
            MoveLayout(ApplicationController.ac.defaultMagnetControllerHeight ?? -400f);
        }
    }

    public void SetAlphaToDefault() {
        SetLayoutAlpha(0.8f);
        InitAlphaSliderValue();
    }

    /** Keep the default value if not already saved */
    public void KeepTrackOfDefaultMagnetControllerLayoutPosition() {
        if (ApplicationController.ac.defaultMagnetControllerHeight == null) {
            ApplicationController.ac.defaultMagnetControllerHeight = rectTransform.anchoredPosition.y;
            // Debug.Log("KeepTrackOfDefaultMagnetControllerLayoutPosition to " + ApplicationController.ac.defaultMagnetControllerHeight);
        }
    }

    public void OnSliderValueChange() {
        SetLayoutAlpha(alphaSlider.value);
    }

    private void SetLayoutAlpha(float alpha) {
        if (canvasGroup != null)
            canvasGroup.alpha = Mathf.Clamp(alpha, 0f, 1f);
    }

    private void InitAlphaSliderValue() {
        if (alphaSlider != null)
            alphaSlider.value = canvasGroup.alpha;
    }

}
