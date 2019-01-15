using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class JumpWaitingSlider : MonoBehaviour {

    [SerializeField] GameObject fillArea;
    [SerializeField] JumpController jumpCtrlr;
    Slider slider;

    void Awake() {
        slider = GetComponent<Slider>();
    }

    void OnEnable() {
        Init();
    }

    void Update() {
        UpdateSlider();
    }

    public void Init() {
        slider.minValue = 0f;
        slider.maxValue = slider.value = jumpCtrlr.timeBetweenJumps;
    }

    void UpdateSlider() {
        float val = Mathf.Clamp(slider.maxValue - (Time.time - jumpCtrlr.lastJump), slider.minValue, slider.maxValue);
        if (fillArea.activeSelf && val <= slider.minValue)
            fillArea.SetActive(false);
        else if (!fillArea.activeSelf && val != slider.minValue)
            fillArea.SetActive(true);
        slider.value = val;
    }
}
