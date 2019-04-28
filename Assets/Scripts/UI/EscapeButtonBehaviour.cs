using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EscapeButtonBehaviour : MonoBehaviour {
    [Tooltip("If one of the children is active when Escape is pressed, this button will not be triggered")]
    [SerializeField] GameObject[] childrenButtons;    
    GraphicRaycaster graphicRaycaster ;
    Button backButton;

    void Start() {
        backButton = GetComponent<Button>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
    }

    void Update() {
        // If Android back button is pressed
        if (Input.GetKeyDown(KeyCode.Escape) && backButton != null && graphicRaycaster!=null && graphicRaycaster.isActiveAndEnabled) {
            bool activeChildExists = false;
            foreach (GameObject child in childrenButtons) {
                if (child.activeInHierarchy) activeChildExists = true;
            }
            if (!activeChildExists)
                backButton.onClick.Invoke();
        }
    }
}
