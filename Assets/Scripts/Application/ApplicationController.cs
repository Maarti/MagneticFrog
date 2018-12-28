using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public class ApplicationController : MonoBehaviour {
    public static ApplicationController ac;
    [SerializeField] public PlayerData PlayerData { get; private set; }
    public float defaultMagnetControllerHeight = -1f;

    void Awake() {
        if (ac == null) {
            DontDestroyOnLoad(gameObject);
            ac = this;
        }
        else if (ac != this) {
            Destroy(gameObject);
            return;
        }
      //  InitLevelSettings();
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
        // MergeSaveIntoInitialData();
    }
    /*
    void InitLevelSettings() {
        levelSettings = LevelSettings.InitLevelSettings();
    }*/

    // PLAYER DATA SETTERS

    public void RecordNewScore(int newScore) {
        if (newScore > PlayerData.bestScore) {
            PlayerData.bestScore = newScore;
        }
    }

    public void SetMagnetControllerLayoutPosition(float yPosition) {
        PlayerData.magnetControllerHeight = yPosition;
    }

}
