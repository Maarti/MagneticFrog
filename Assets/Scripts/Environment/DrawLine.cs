using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour {

    [Tooltip("The objects to draw the line between")]
    [SerializeField] GameObject[] targets;
    private LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = targets.Length;
        SetPositions();
    }

    void LateUpdate() {
        SetPositions();
    }

    void SetPositions() {
        for (int i = 0; i < targets.Length; i++) {
            lineRenderer.SetPosition(i, targets[i].transform.position);
        }
    }
}
