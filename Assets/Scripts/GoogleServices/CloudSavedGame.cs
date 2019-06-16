using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

public class CloudSavedGame : MonoBehaviour {

    public static ISavedGameMetadata savedGameMetadata;
    public static string SAVE_FILENAME = "magnetic_frog_save";

    public static void OpenSavedGameThenLoad() {
        try {
            if (Social.localUser.authenticated) {
                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.OpenWithAutomaticConflictResolution(SAVE_FILENAME, DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime, LoadOnSavedGameOpened);
            }
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    public static void OpenSavedGameThenSave() {
        try {
            if (Social.localUser.authenticated) {
                ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
                savedGameClient.OpenWithAutomaticConflictResolution(SAVE_FILENAME, DataSource.ReadCacheOrNetwork,
                    ConflictResolutionStrategy.UseLongestPlaytime, SaveOnSavedGameOpened);
            }
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    static void LoadOnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            Debug.LogFormat("LoadOnSavedGameOpened {0}", game);
            savedGameMetadata = game;
            if (game.TotalTimePlayed.CompareTo(TimeSpan.MinValue) >= 1)
                LoadGameData(game);
        }
        else {
            Debug.LogFormat("OnSavedGameOpened error {0}", status);
        }
    }

    static void SaveOnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            Debug.LogFormat("SaveOnSavedGameOpened {0}", game);
            savedGameMetadata = game;
            SaveGame();
        }
        else {
            Debug.LogFormat("OnSavedGameOpened error {0}", status);
        }
    }

    static void SaveGame() {
        Debug.Log("SaveGame()");
        if (Social.localUser.authenticated && savedGameMetadata != null) {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            TimeSpan totalPlaytime = new TimeSpan(0, 0, Mathf.FloorToInt(Time.realtimeSinceStartup));
            totalPlaytime = totalPlaytime.Add(savedGameMetadata.TotalTimePlayed);
            SavedGameMetadataUpdate updatedMetadata = builder
                .WithUpdatedPlayedTime(totalPlaytime)
                .WithUpdatedDescription(string.Format("Saved game at {0} - Playtime: {1} - {2} coin(s)", DateTime.Now, totalPlaytime, ApplicationController.ac.PlayerData.coins))
                .Build();
            byte[] savedData = ObjectToByteArray(ApplicationController.ac.PlayerData);
            Debug.LogFormat("Trying to CommitUpdate() with savedGameMetadata={0} - updatedMetadata={1} - savedData={2}", savedGameMetadata, updatedMetadata, savedData);
            savedGameClient.CommitUpdate(savedGameMetadata, updatedMetadata, savedData, OnSavedGameWritten);
        }
    }

    static void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            Debug.LogFormat("OnSavedGameWritten success {0}", game);
            savedGameMetadata = game;
        }
        else {
            Debug.LogFormat("OnSavedGameWritten failed {0}", status);
        }
    }

    static void LoadGameData(ISavedGameMetadata game) {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }

    static void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data) {
        if (status == SavedGameRequestStatus.Success) {
            Debug.LogFormat("OnSavedGameDataRead() {0}", data);
            PlayerData playerData;
            if (data == null || data.Length == 0) {
                playerData = new PlayerData();
                playerData.lang = Application.systemLanguage;
            }
            else
                playerData = FromByteArray<PlayerData>(data);
            Debug.LogFormat("Parsed save {0}", playerData);
            ApplicationController.ac.LoadCloudSave(playerData);
        }
        else {
            Debug.LogFormat("OnSavedGameDataRead failed {0}", status);
        }
    }

    static byte[] ObjectToByteArray(object obj) {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream()) {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

    static T FromByteArray<T>(byte[] data) {
        if (data == null)
            return default(T);
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream(data)) {
            object obj = bf.Deserialize(ms);
            return (T)obj;
        }
    }
}
