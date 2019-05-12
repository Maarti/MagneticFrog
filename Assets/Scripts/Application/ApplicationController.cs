using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class ApplicationController : MonoBehaviour {
    public static ApplicationController ac;
    [SerializeField] public PlayerData PlayerData; // { get; private set; }
    public float? defaultMagnetControllerHeight = null;
    public CharacterSettings[] characters;

    void Awake() {
        if (ac == null) {
            DontDestroyOnLoad(gameObject);
            ac = this;
        }
        else if (ac != this) {
            Destroy(gameObject);
            return;
        }
        Load();
    }

    public void Save() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/toad.dat");
        bf.Serialize(file, PlayerData);
        file.Close();
        Debug.Log(string.Format("Game saved with score {0}", PlayerData.bestScore));
        CloudSavedGame.instance.SaveGame(System.Text.Encoding.ASCII.GetBytes("test"), TimeSpan.MinValue);
    }

    public void Load() {
        if (File.Exists(Application.persistentDataPath + "/toad.dat")) {
            Debug.Log("Save loaded:" + Application.persistentDataPath);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/toad.dat", FileMode.Open);
            PlayerData = (PlayerData)bf.Deserialize(file);
            file.Close();
        }
        else {
            PlayerData = new PlayerData();
            PlayerData.lang = Application.systemLanguage;
            Save();
        }
        MergeSaveIntoInitialData();
        CharacterSelector.RefreshCurrentCharacterDisplay();
    }

    void MergeSaveIntoInitialData() {
        LoadCharacters();
        LoadCurrentCharacter();
    }

    void LoadCharacters() {
        if (PlayerData.characters == null) return;
        foreach (CharacterSettings character in characters) {
            if (PlayerData.characters.ContainsKey(character.id)) {
                CharacterSavedData savedCharacter = PlayerData.characters[character.id];
                character.isUnlocked = true;
                character.agility = (savedCharacter.agility > character.agility) ? savedCharacter.agility : character.agility;
                character.stamina = (savedCharacter.stamina > character.stamina) ? savedCharacter.stamina : character.stamina;
                character.breath = (savedCharacter.breath > character.breath) ? savedCharacter.breath : character.breath;
            }
        }
    }

    public void SaveCharacters() {
        Dictionary<CharacterId, CharacterSavedData> newData = new Dictionary<CharacterId, CharacterSavedData>();
        foreach (CharacterSettings character in characters) {
            if (character.isUnlocked) {
                CharacterSavedData savedCharacter = new CharacterSavedData() {
                    agility = character.agility,
                    stamina = character.stamina,
                    breath = character.breath
                };
                newData.Add(character.id, savedCharacter);
            }
        }
        PlayerData.characters = newData;
    }

    void LoadCurrentCharacter() {
        CharacterSelector.currentCharacter = Array.FindIndex(characters, c => c.id == PlayerData.currentCharacater);
        if (CharacterSelector.currentCharacter < 0) {
            Debug.LogWarning("Current character " + PlayerData.currentCharacater.ToString() + " not found!");
            CharacterSelector.currentCharacter = 0;
        }
        CharacterSelector.currentlyDisplayedCharacter = CharacterSelector.currentCharacter;
    }

    public void SaveCurrentCharacter() {        
        PlayerData.currentCharacater = characters[CharacterSelector.currentCharacter].id;
    }


    // PLAYER DATA SETTERS
    public void RecordNewScore(int newScore) {
        if (newScore > PlayerData.bestScore) {
            PlayerData.bestScore = newScore;
        }
        Leaderboard.ReportScore(newScore);
    }

    public void SetMagnetControllerLayoutPositionAndAlpha(float yPosition, float alpha) {
        PlayerData.magnetControllerHeight = yPosition;
        PlayerData.magnetControllerAlpha = alpha;
    }

    public void UpdateCoins(int value) {
        PlayerData.coins = Mathf.Clamp(PlayerData.coins + value, 0, 999999);
    }

    public void FinishTutorial() {
        PlayerData.isTutorialDone = true;
    }

    public void ResetTutorial() {
        PlayerData.isTutorialDone = false;
    }

    public void UnlockCharacter(int characterId) {
        CharacterSettings character = characters[characterId];
        if (!character.isUnlocked && PlayerData.coins >= character.cost) {
            character.isUnlocked = true;
            UpdateCoins(character.cost * -1);
            SaveCharacters();
        }
        else {
            Debug.LogWarning(String.Format("Can't unlock character {0}. isUnlocked={1} cost={2} coins={3}", characterId, character.isUnlocked, character.cost, PlayerData.coins));
        }
    }

}
