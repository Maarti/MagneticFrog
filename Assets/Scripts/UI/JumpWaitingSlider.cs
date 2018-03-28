using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class JumpWaitingSlider : MonoBehaviour {

    [SerializeField]
    GameObject fillArea;
    PlayerController pc;
    Slider slider;

	void Awake () {
        slider = GetComponent<Slider>();
	}

    void Start() {
        pc = GetComponentInParent<PlayerController>();
        InitSlider();
    }
	
	void Update () {
        UpdateSlider();
    }

    void InitSlider() {
        slider.minValue = 0f;
        slider.maxValue = slider.value = pc.timeBetweenJumps;
    }

    void UpdateSlider() {
        float val = Mathf.Clamp(slider.maxValue - (Time.time - pc.lastJump), slider.minValue, slider.maxValue);
        if (fillArea.activeSelf && val <= slider.minValue)
            fillArea.SetActive(false);
        else if(!fillArea.activeSelf && val != slider.minValue)
            fillArea.SetActive(true);
        slider.value = val;
    }
}
