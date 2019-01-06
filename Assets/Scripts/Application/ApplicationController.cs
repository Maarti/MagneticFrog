using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class ApplicationController : MonoBehaviour {
    public static ApplicationController ac;
    [SerializeField] public PlayerData PlayerData; // { get; private set; }
    public float defaultMagnetControllerHeight = -1f;
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
    }

    void MergeSaveIntoInitialData() {
        LoadCharacters();
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
                CharacterSavedData savedCharacter = new CharacterSavedData(character.agility, character.stamina, character.breath);
                newData.Add(character.id, savedCharacter);
            }
        }
        PlayerData.characters = newData;
    }


    // PLAYER DATA SETTERS
    public void RecordNewScore(int newScore) {
        if (newScore > PlayerData.bestScore) {
            PlayerData.bestScore = newScore;
        }
    }

    public void SetMagnetControllerLayoutPosition(float yPosition) {
        PlayerData.magnetControllerHeight = yPosition;
    }

    public void UpateCoins(int value) {
        PlayerData.coins = Mathf.Clamp(PlayerData.coins + value, 0, 999999);
    }

    public void FinishTutorial() {
        PlayerData.isTutorialDone = true;
    }

    public void ResetTutorial() {
        PlayerData.isTutorialDone = false;
    }

}
