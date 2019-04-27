using UnityEngine;
using UnityEngine.UI;

public class MeterCounter : MonoBehaviour {

    [SerializeField] private Text meterText;
    private bool isCounting = true;
    int _value = 0;
    public int Value {
        get { return _value; }
        private set {
            _value = value;
            meterText.text = value + "m";
        }
    }

    private void OnEnable() {
        GameController.OnGameOver += OnGameOver;
        isCounting = true;
    }

    private void OnDisable() {
        GameController.OnGameOver -= OnGameOver;
    }

    private void Update() {
        if (isCounting) {
            Value = Mathf.RoundToInt(transform.position.y);
        }
    }

    void OnGameOver(int score) {
       isCounting = false;
        Value = score;
    }
}
