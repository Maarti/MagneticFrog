using UnityEngine;

[RequireComponent(typeof(MagnetController))]
[RequireComponent(typeof(JumpController))]
[RequireComponent(typeof(OxygenController))]
[RequireComponent(typeof(MeterCounter))]
public class PlayerController : MonoBehaviour {

    [SerializeField] MagnetController magnetCtrlr;
    [SerializeField] JumpController jumpCtrlr;
    [SerializeField] OxygenController oxygenCtrlr;
    [SerializeField] MeterCounter meterCounter;

    Vector3 initialPosition;

    void Awake() {
        initialPosition = transform.position;
        enabled = false;
    }

    // This script is enabled when level start
    void OnEnable() {
        magnetCtrlr.enabled = true;
        jumpCtrlr.enabled = true;
        oxygenCtrlr.enabled = true;
        Init();
    }

    void OnDisable() {
        magnetCtrlr.enabled = false;
        jumpCtrlr.enabled = false;
        oxygenCtrlr.enabled = false;
    }

    // Called on game start
    public void Init() {
        transform.position = initialPosition;
        magnetCtrlr.Init();
        jumpCtrlr.Init();
        oxygenCtrlr.Init();
    }

    public void Die() {
        GameController.gc.TriggerGameOver(meterCounter.Value);
      //  gameObject.SetActive(false);
    }

}
