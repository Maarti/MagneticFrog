using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelector : MonoBehaviour {

    public static int currentCharacter = 0;
    public static int currentlyDisplayedCharacter = 0;
    public static CharacterId DEFAULT_CHARACTER = CharacterId.HYLA;
    [SerializeField] Slider agilitySlider;
    [SerializeField] Slider staminaSlider;
    [SerializeField] Slider breathSlider;
    [SerializeField] Button agilityButton;
    [SerializeField] Button staminaButton;
    [SerializeField] Button breathButton;
    [SerializeField] Button selectButton;
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] GameObject selectButtonObj;
    [SerializeField] GameObject purchaseButtonObj;
    [SerializeField] GameObject cloudImg;
    [SerializeField] TextMeshProUGUI priceTxt;

    void OnEnable() {
        RefreshCurrentCharacterDisplay();
        RefreshUI();
    }

    void OnDisable() {
        StopAllCoroutines();
        RefreshCurrentCharacterDisplay();
    }

    public void NextCharacter() {
        int totalCharacters = ApplicationController.ac.characters.Length;
        int oldCharacter = currentlyDisplayedCharacter;
        int newCharacter = (currentlyDisplayedCharacter + 1 > totalCharacters - 1) ? 0 : currentlyDisplayedCharacter + 1;
        StartCoroutine(SwipeCharacterOverTime(oldCharacter, .25f, 0f, -3f, false));
        StartCoroutine(SwipeCharacterOverTime(newCharacter, .25f, 3f, -3f));
        currentlyDisplayedCharacter = newCharacter;
        RefreshUI();
    }

    public void PreviousCharacter() {
        int totalCharacters = ApplicationController.ac.characters.Length;
        int oldCharacter = currentlyDisplayedCharacter;
        int newCharacter = (currentlyDisplayedCharacter - 1 < 0) ? totalCharacters - 1 : currentlyDisplayedCharacter - 1;
        StartCoroutine(SwipeCharacterOverTime(oldCharacter, .25f, 0f, +3f, false));
        StartCoroutine(SwipeCharacterOverTime(newCharacter, .25f, -3f, 3f));
        currentlyDisplayedCharacter = newCharacter;
        RefreshUI();
    }

    IEnumerator SwipeCharacterOverTime(int characterIndex, float duration, float xOffsetStart, float xOffsetEnd, bool activeAtTheEnd = true) {
        GameObject character = ApplicationController.ac.characters[characterIndex].skin;
        float startTime = Time.time;
        float endTime = startTime + duration;
        Vector3 startPos = character.transform.position;
        startPos.x = xOffsetStart;
        Vector3 currentPos = startPos;
        Vector3 endPos = startPos;
        endPos.x += xOffsetEnd;
        character.transform.position = startPos;
        character.SetActive(true);
        while (Time.time < endTime) {
            currentPos.x = Mathf.Lerp(startPos.x, endPos.x, (Time.time - startTime) / duration);
            character.transform.position = currentPos;
            yield return null;
        }
        if (activeAtTheEnd) {
            character.transform.position = endPos;
            character.SetActive(true);
        }
        else {
            character.transform.position = startPos;
            character.SetActive(false);
        }
    }

    public static void DisableAllCharacters() {
        foreach (CharacterSettings character in ApplicationController.ac.characters) {
            if (character.skin != null)
                character.skin.SetActive(false);
        }
    }

    public static void EnableCurrentCharacter() {
        GameObject charSkin = ApplicationController.ac.characters[currentCharacter].skin;
        if (charSkin != null) {
            charSkin.SetActive(true);
            Vector3 charPos = charSkin.transform.position;
            charPos.x = 0f;
            charSkin.transform.position = charPos;
        }
    }

    public static void RefreshCurrentCharacterDisplay() {
        DisableAllCharacters();
        EnableCurrentCharacter();
        currentlyDisplayedCharacter = currentCharacter;
    }

    // Wrapper non-static to be called from button OnClick()
    public void RefreshCharacterDisplay() {
        RefreshCurrentCharacterDisplay();
    }

    public void SelectCharacter() {
        currentCharacter = currentlyDisplayedCharacter;
        ApplicationController.ac.SaveCurrentCharacter();
        ApplicationController.ac.Save();
        RefreshUI();
        Animator anim = ApplicationController.ac.characters[currentCharacter].skin.GetComponent<Animator>();
        if (anim != null) anim.SetTrigger("flip");
    }

    public void RefreshUI() {
        CharacterSettings displayedChar = ApplicationController.ac.characters[currentlyDisplayedCharacter];
        characterName.text = displayedChar.name;
        // Stat value
        agilitySlider.value = displayedChar.agility;
        staminaSlider.value = displayedChar.stamina;
        breathSlider.value = displayedChar.breath;
        // Display upgrade btn
        agilityButton.gameObject.SetActive(displayedChar.agility < 3);
        staminaButton.gameObject.SetActive(displayedChar.stamina < 3);
        breathButton.gameObject.SetActive(displayedChar.breath < 3);
        // Enable upgrade btn
        agilityButton.interactable = displayedChar.isUnlocked;
        staminaButton.interactable = displayedChar.isUnlocked;
        breathButton.interactable = displayedChar.isUnlocked;
        // Select btn
        if (currentCharacter == currentlyDisplayedCharacter) {
            selectButton.interactable = false;
        }
        else {
            selectButton.interactable = true;
        }
        // Purchase btn
        if (displayedChar.isUnlocked) {
            selectButtonObj.SetActive(true);
            purchaseButtonObj.SetActive(false);
            cloudImg.SetActive(false);
        }
        else {
            selectButtonObj.SetActive(false);
            purchaseButtonObj.SetActive(true);
            cloudImg.SetActive(true);
            priceTxt.text = displayedChar.cost + " <sprite name=\"coin\">";
        }
    }
       
}
