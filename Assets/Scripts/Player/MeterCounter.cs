using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterCounter : MonoBehaviour {

    [SerializeField] private Text meterText;
    int _value = 0;
    public int Value {
        get { return _value; }

        private set {
            _value = value;
            meterText.text = value + "m";
        }
    }

    private void Update() {
        Value = Mathf.RoundToInt(transform.position.y);        
    }

}
