using UnityEngine;
using UnityEngine.UI;

public enum ButtonType { ResetTuto, IAP_Premium, IAP_Frogbot }

public class ButtonActivator : MonoBehaviour {

    [SerializeField] ButtonType type;
    Button button;
    bool isStarted = false;

    private void Start() {
        button = GetComponent<Button>();
        isStarted = true;
    }

    private void OnEnable() {
        if (!isStarted) Start();
        RefreshUI();
    }

    private void RefreshUI() {
        bool enable = true;
        switch (type) {
            case ButtonType.ResetTuto:
                enable = ApplicationController.ac.PlayerData.isTutorialDone;
                break;
        }
        button.interactable = enable;
    }


}
