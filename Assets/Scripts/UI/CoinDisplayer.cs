using UnityEngine;
using TMPro;

public class CoinDisplayer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI coinText;
    bool isStarted = false;

    private void Start() {
        isStarted = true;
        OnEnable();
        ApplicationController.OnLoad += RefreshUI;
    }

    private void OnEnable() {
        if (!isStarted) return;
        RefreshUI();
    }

    public void RefreshUI() {
        coinText.text = ApplicationController.ac.PlayerData.coins.ToString() + " <sprite name=\"coin\">";
    }

}
