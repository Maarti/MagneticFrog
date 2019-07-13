using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionDisplayer : MonoBehaviour {

    private void OnEnable() {
        RefreshUI();
    }

    private void RefreshUI() {
        string premiumString = (ApplicationController.ac.PlayerData.isPremium) ? " - Premium" : "";
        GetComponent<TextMeshProUGUI>().text = "Version " + Application.version.ToString() + premiumString;
    }
}
