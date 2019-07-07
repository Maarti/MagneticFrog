using UnityEngine;
using TMPro;

public class UnlockCharacterPanel : MonoBehaviour {
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] GameObject notEnouhGoldText;
    [SerializeField] GameObject purchaseText;
    [SerializeField] GameObject rewardAdButton;
    [SerializeField] GameObject purchaseButton;
    [SerializeField] CharacterSelector characterSelector;
    [SerializeField] CoinDisplayer coinDisplayer;
    bool isStarted = false;

    private void Start() {
        isStarted = true;
        OnEnable();
    }

    private void OnEnable() {
        if (!isStarted) return;
        RefreshUI();
    }

    private void RefreshUI() {
        CharacterSettings displayedChar = ApplicationController.ac.characters[CharacterSelector.currentlyDisplayedCharacter];
        costText.text = "Cost: " + displayedChar.cost.ToString() + " <sprite name=\"coin\">";
        if (ApplicationController.ac.PlayerData.coins >= displayedChar.cost) {
            DisplayPurchasePanel();
        }
        else {
            DisplayNotEnoughGoldPanel();
        }
    }

    private void DisplayNotEnoughGoldPanel() {
        purchaseButton.SetActive(false);
        purchaseText.SetActive(false);
        rewardAdButton.SetActive(true);
        notEnouhGoldText.SetActive(true);
    }

    private void DisplayPurchasePanel() {
        purchaseButton.SetActive(true);
        purchaseText.SetActive(true);
        rewardAdButton.SetActive(false);
        notEnouhGoldText.SetActive(false);
    }

    public void PurchaseCharacter() {
        ApplicationController.ac.UnlockCharacter(CharacterSelector.currentlyDisplayedCharacter);
        ApplicationController.ac.Save();
        this.gameObject.SetActive(false);
        characterSelector.RefreshUI();
        coinDisplayer.RefreshUI();
    }


}
