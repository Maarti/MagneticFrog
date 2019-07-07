using UnityEngine;
using TMPro;

public enum StatsType { AGILITY, STAMINA, BREATH }

public class UpgradeStatPanel : MonoBehaviour {
    [SerializeField] TextMeshProUGUI purchaseText;
    [SerializeField] TextMeshProUGUI notEnouhGoldText;
    [SerializeField] GameObject rewardAdButton;
    [SerializeField] GameObject purchaseButton;
    [SerializeField] CharacterSelector characterSelector;
    [SerializeField] CoinDisplayer coinDisplayer;
    CharacterSettings currentCharacter;
    StatsType currentStat;
    string statString = "";
    int cost = -1;

    public void OpenAgilityDialog() {
        OpenDialog(StatsType.AGILITY);
    }
    public void OpenStaminaDialog() {
        OpenDialog(StatsType.STAMINA);
    }
    public void OpenBreathDialog() {
        OpenDialog(StatsType.BREATH);
    }

    private void OpenDialog(StatsType statsType) {
        currentStat = statsType;
        currentCharacter = ApplicationController.ac.characters[CharacterSelector.currentlyDisplayedCharacter];
        cost = GetCost(currentCharacter, statsType);
        if (cost < 0) return;
        statString = GetStatString(statsType);
        if (ApplicationController.ac.PlayerData.coins >= cost)
            DisplayPurchasePanel();
        else
            DisplayNotEnoughGoldPanel();
        gameObject.SetActive(true);
    }

    private int GetCost(CharacterSettings character, StatsType statsType) {
        int statsAmount = 0;
        switch (statsType) {
            case StatsType.AGILITY:
                statsAmount = character.agility;
                break;
            case StatsType.STAMINA:
                statsAmount = character.stamina;
                break;
            case StatsType.BREATH:
                statsAmount = character.breath;
                break;
        }
        if (statsAmount < CharacterSettings.MAX_STAT && statsAmount >= 0)
            return CharacterSettings.STATS_COST[statsAmount];
        else
            return -1;
    }

    private string GetStatString(StatsType statsType) {
        switch (statsType) {
            case StatsType.AGILITY:
                return "agility";
            case StatsType.STAMINA:
                return "stamina";
            case StatsType.BREATH:
                return "breath";
            default:
                return "this stat";
        }
    }

    private void DisplayNotEnoughGoldPanel() {
        notEnouhGoldText.text = "You can upgrade <b><smallcaps>" + statString + "</smallcaps></b> for <nobr>" + cost.ToString() + " <sprite name=\"coin\"></nobr>.\n\nYou don't have enough <sprite name=\"coin\"> but you can earn some by watching a short video:";
        purchaseButton.SetActive(false);
        purchaseText.gameObject.SetActive(false);
        rewardAdButton.SetActive(true);
        notEnouhGoldText.gameObject.SetActive(true);
    }

    private void DisplayPurchasePanel() {
        purchaseText.text = "You can upgrade <b><smallcaps>" + statString + "</smallcaps></b> for <nobr>" + cost.ToString() + " <sprite name=\"coin\"></nobr>.\n\nProceed?";
        purchaseButton.SetActive(true);
        purchaseText.gameObject.SetActive(true);
        rewardAdButton.SetActive(false);
        notEnouhGoldText.gameObject.SetActive(false);
    }

    public void UpgradeCurrentStat() {
        switch (currentStat) {
            case StatsType.AGILITY:
                ApplicationController.ac.UpgradeAgility(currentCharacter, cost);
                break;
            case StatsType.STAMINA:
                ApplicationController.ac.UpgradeStamina(currentCharacter, cost);
                break;
            case StatsType.BREATH:
                ApplicationController.ac.UpgradeBreath(currentCharacter, cost);
                break;
        }
        gameObject.SetActive(false);        
        ApplicationController.ac.Save();        
        characterSelector.RefreshUI();
        coinDisplayer.RefreshUI();
    }

}
