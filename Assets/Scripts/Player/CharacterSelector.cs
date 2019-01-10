using System.Collections;
using UnityEngine;

public class CharacterSelector : MonoBehaviour {

    public static int currentCharacter = 0;
    public static int currentlyDisplayedCharacter = 0;
    public static CharacterId DEFAULT_CHARACTER = CharacterId.GREEN;

    public void NextCharacter() {
        int totalCharacters = ApplicationController.ac.characters.Length;
        int oldCharacter = currentlyDisplayedCharacter;
        int newCharacter = (currentlyDisplayedCharacter + 1 > totalCharacters - 1) ? 0 : currentlyDisplayedCharacter + 1;
        StartCoroutine(SwipeCharacterOverTime(oldCharacter, .25f, 0f, -3f, false));
        StartCoroutine(SwipeCharacterOverTime(newCharacter, .25f, 3f, -3f));
        currentlyDisplayedCharacter = newCharacter;
    }

    public void PreviousCharacter() {
        int totalCharacters = ApplicationController.ac.characters.Length;
        int oldCharacter = currentlyDisplayedCharacter;
        int newCharacter = (currentlyDisplayedCharacter - 1 < 0) ? totalCharacters - 1 : currentlyDisplayedCharacter - 1;
        StartCoroutine(SwipeCharacterOverTime(oldCharacter, .25f, 0f, +3f, false));
        StartCoroutine(SwipeCharacterOverTime(newCharacter, .25f, -3f, 3f));
        currentlyDisplayedCharacter = newCharacter;
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
            character.skin.SetActive(false);
        }
    }

    public static void EnableCurrentCharacter() {
        ApplicationController.ac.characters[currentCharacter].skin.SetActive(true);
    }

    public void SelectCharacter() {
        currentCharacter = currentlyDisplayedCharacter;
        ApplicationController.ac.SaveCurrentCharacter();
        ApplicationController.ac.Save();
    }

}
