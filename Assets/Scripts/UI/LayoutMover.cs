using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class LayoutMover : MonoBehaviour {

    public GameObject confirmButton;

    RectTransform layout;
    float yMin = Screen.height * .1f;
    float yMax = Screen.height * .8f;

    private void Awake() {
        layout = GetComponent<RectTransform>();
    }

    void Update() {
        if (Input.touchCount > 0) {
            MoveLayout();
        }
    }

    void MoveLayout() {
        if (!IsPointerOverUIObject(confirmButton)) { // Don't move layout if we are touching the confirm button
            Vector3 newPos = layout.position;
            newPos.y = Mathf.Clamp(Input.GetTouch(0).position.y, yMin, yMax);
            layout.position = newPos;
        }
    }

    bool IsPointerOverUIObject(GameObject go) {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current) {
            position = Input.GetTouch(0).position
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        foreach(RaycastResult res in results) {
            if (res.gameObject == go)
                return true;
        }
        return false;
    }

    public void EnableLayoutMover(bool onOff) {
        this.enabled = onOff;
    }
}
