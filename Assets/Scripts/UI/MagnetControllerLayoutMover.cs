using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class MagnetControllerLayoutMover : MonoBehaviour {

    [SerializeField] GameObject confirmButton;
    [SerializeField] GameObject defaultButton;
    [SerializeField] bool isMoving = false;
    [SerializeField] RectTransform parentRect;

    RectTransform rectTransform;
 /*   float yMin = Screen.height * .1f;
    float yMax = Screen.height * .8f;*/
     float yMin = -550f;
    float yMax = 300f;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable() {
        KeepTrackOfDefaultMagnetControllerLayoutPosition();
        if (ApplicationController.ac.PlayerData.magnetControllerHeight != null)
            MoveLayout(ApplicationController.ac.PlayerData.magnetControllerHeight ?? -400);
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
            // Don't move layout if we are touching the button
            if (!IsPointerOverUIObject(confirmButton, inputController) && !IsPointerOverUIObject(defaultButton, inputController)) {
                Vector2 anchorPos = ScreenPointToAnchorPos(inputController);
                MoveLayout(anchorPos.y);
            }
        }
    }

    Vector2 ScreenPointToAnchorPos(Vector2 screenPoint) {
        Vector2 anchorPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, screenPoint, null, out anchorPos);
        Debug.Log("ScreenPos=" + screenPoint + "   AnchorPos=" + anchorPos + " Parent=" + parentRect, parentRect);
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

    public void SaveLayoutPosition() {
        ApplicationController.ac.SetMagnetControllerLayoutPosition(rectTransform.anchoredPosition.y);
        ApplicationController.ac.Save();
    }

    public void SetPositionToDefault() {
        if (ApplicationController.ac.defaultMagnetControllerHeight != null) {
            MoveLayout(ApplicationController.ac.defaultMagnetControllerHeight ?? -400f);
        }
    }

    /** Keep the default value if not already saved */
    public void KeepTrackOfDefaultMagnetControllerLayoutPosition() {
        if (ApplicationController.ac.defaultMagnetControllerHeight == null) {
            ApplicationController.ac.defaultMagnetControllerHeight = rectTransform.anchoredPosition.y;
            Debug.Log("KeepTrackOfDefaultMagnetControllerLayoutPosition to " + ApplicationController.ac.defaultMagnetControllerHeight);
        }
    }
}
