using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class MagnetControllerLayoutMover : MonoBehaviour {

    [SerializeField] GameObject confirmButton;
    [SerializeField] GameObject defaultButton;
    [SerializeField] bool isMoving = false;

    RectTransform rectTransform;
    float yMin = Screen.height * .1f;
    float yMax = Screen.height * .8f;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable() {
        SetDefaultMagnetControllerLayoutPosition();
        if (ApplicationController.ac.PlayerData.magnetControllerHeight > 0f) {
            MoveLayout(ApplicationController.ac.PlayerData.magnetControllerHeight);
        }
    }

    void Update() {
        if (!isMoving || confirmButton == null) return;

        Vector2 inputController;    // Position of the cursor or finger
#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        if (Input.touchCount > 0) {
            inputController = Input.GetTouch(0).position;
            // Don't move layout if we are touching the button
            if (!IsPointerOverUIObject(confirmButton, inputController) && !IsPointerOverUIObject(defaultButton, inputController)) {
                MoveLayout(inputController.y);
            }
        }
#else
        if (Input.GetMouseButtonDown(0)) {
            inputController = Input.mousePosition;
            // Don't move layout if we are clicking on the button
            if (!IsPointerOverUIObject(confirmButton, inputController) && !IsPointerOverUIObject(defaultButton, inputController)) {
                MoveLayout(inputController.y);
            }
        }
#endif
    }

    void MoveLayout(float yPosition) {
        Vector3 newPos = rectTransform.position;
        newPos.y = Mathf.Clamp(yPosition, yMin, yMax);
        rectTransform.position = newPos;
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
        ApplicationController.ac.SetMagnetControllerLayoutPosition(transform.position.y);
        ApplicationController.ac.Save();
    }

    public void SetPositionToDefault() {
        if (ApplicationController.ac.defaultMagnetControllerHeight > 0f) {
            MoveLayout(ApplicationController.ac.defaultMagnetControllerHeight);
        }
    }

    /** Keep the default value if not already saved */
    public void SetDefaultMagnetControllerLayoutPosition() {
        if (ApplicationController.ac.defaultMagnetControllerHeight < 0f) {
            ApplicationController.ac.defaultMagnetControllerHeight = transform.position.y;
            Debug.Log("SetDefaultMagnetControllerLayoutPosition to " + ApplicationController.ac.defaultMagnetControllerHeight);
        }
    }
}
