using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class OxygenController : MonoBehaviour {

    [SerializeField] PlayerController playerCtrlr;
    public float oxygenMax = 20f;
    float oxygen;
    public float Oxygen {
        get { return oxygen; }
        set {
            oxygen = Mathf.Clamp(value, 0f, oxygenMax);
            oxygenBar.value = oxygen;
        }
    }
    [SerializeField] Slider oxygenBar;

    public void Init() {
        oxygenBar.minValue = 0f;
        oxygenBar.maxValue = oxygenBar.value = Oxygen = oxygenMax;
    }

    void Update() {
        Oxygen -= Time.deltaTime;
        if (Oxygen <= 0)
            playerCtrlr.Die();
    }

    public void AddOxygen(float oxygenAmount) {
        Oxygen += oxygenAmount;
    }
}
