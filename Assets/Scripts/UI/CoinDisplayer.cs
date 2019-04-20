using UnityEngine;
using TMPro;

public class CoinDisplayer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI coinText;
    bool isStarted = false;

    private void Start() {
        isStarted = true;
        OnEnable();
    }

    private void OnEnable() {
        if (!isStarted) return;
        coinText.text = ApplicationController.ac.PlayerData.coins.ToString() + " <sprite name=\"coin\">";
    }

}
