using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterCounter : MonoBehaviour {

    [SerializeField] private Text meterText;

    private void Update() {
        meterText.text = Mathf.RoundToInt(transform.position.y) + "m";
    }

}
