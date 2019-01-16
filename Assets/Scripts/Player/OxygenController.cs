using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerController))]
public class OxygenController : MonoBehaviour {

    public delegate void BubbleCollectDelegate();
    public static event BubbleCollectDelegate OnBubbleCollect;
    [SerializeField] PlayerController playerCtrlr;
    public float oxygenMax = 20f;
    public float oxygenConsumption = 1f;    // oxygen consumption per second
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
        InitBreath();
        oxygenBar.minValue = 0f;
        oxygenBar.maxValue = 35f;
        oxygenBar.value = Oxygen = oxygenMax;
    }

    void Update() {
        Oxygen -= oxygenConsumption * Time.deltaTime;
        if (Oxygen <= 0)
            playerCtrlr.Die();
    }

    public void AddOxygen(float oxygenAmount) {
        Oxygen += oxygenAmount;
        if (OnBubbleCollect != null)
            OnBubbleCollect();
    }

    void InitBreath() {
        switch (ApplicationController.ac.characters[CharacterSelector.currentCharacter].breath) {
            case 0:
                oxygenMax = 20f;
                break;
            case 1:
                oxygenMax = 25f;
                break;
            case 2:
                oxygenMax = 30f;
                break;
            case 3:
                oxygenMax = 35f;
                break;
        }
    }
}
