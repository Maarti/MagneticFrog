using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(MagnetController))]
[RequireComponent(typeof(JumpController))]
public class PlayerController : MonoBehaviour {

    [SerializeField] MagnetController magnetCtrlr;
    [SerializeField] JumpController jumpController;
    [SerializeField] MeterCounter meterCounter;

    [Header("Oxygen")]
    public float oxygenMax = 20f;
    private float oxygen;
    public float Oxygen {
        get { return oxygen; }
        set {
            oxygen = Mathf.Clamp(value, 0f, oxygenMax);
            oxygenBar.value = oxygen;
        }
    }
    [SerializeField] Slider oxygenBar;
    
    Vector3 initialPosition;
    Rigidbody2D rb;
    float screenMid;                      // Middle of the screen width in pixels correct

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        this.enabled = false;
    }

    // This script is enabled when level start
    void OnEnable() {
        magnetCtrlr.enabled = true;
        jumpController.enabled = true;
        Init();
    }

    void OnDisable() {
        magnetCtrlr.enabled = false;
        jumpController.enabled = false;
    }

    // Called on game start
    public void Init() {
        transform.position = initialPosition;
        InitOxygenBar();
        magnetCtrlr.Init();
        jumpController.Init();
    }

    void Update() {
        UpdateOxygen();
    }

    void InitOxygenBar() {
        //  oxygenBar = GameObject.Find("OxygenBar").GetComponent<Slider>();
        oxygenBar.minValue = 0f;
        oxygenBar.maxValue = oxygenBar.value = Oxygen = oxygenMax;
    }

    void UpdateOxygen() {
        Oxygen -= Time.deltaTime;
        if (Oxygen <= 0)
            Die();
    }

    public void AddOxygen(float oxygenAmount) {
        Oxygen += oxygenAmount;
    }

    public void Die() {
        GameController.gc.TriggerGameOver(meterCounter.Value);
        gameObject.SetActive(false);
    }


}
