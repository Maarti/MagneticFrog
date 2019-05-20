using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CloudSavedGame : MonoBehaviour {

    public static CloudSavedGame instance;
    public static ISavedGameMetadata savedGameMetadata;
    public static string SAVE_FILENAME = "magnetic_frog_save";
    public ISavedGameClient savedGameClient;
    public int totalSavedSlots = 0;

    public void Awake() {
        if (instance == null) {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
            return;
        }
    }

    /*  public void Init() {
          Debug.LogFormat("CloudSavedGame.Init() authent={0}", Social.localUser.authenticated);
          if (Social.localUser.authenticated) {
             savedGameClient = PlayGamesPlatform.Instance.SavedGame;
              FetchAllSavedGames();
          }
      }*/

    /*  public void ShowSelectUI() {
          if (Social.localUser.authenticated) {
              uint maxNumToDisplay = 3;
              bool allowCreateNew = totalSavedSlots <= 2;
              bool allowDelete = true;


              savedGameClient.ShowSelectSavedGameUI("Select saved game",
                  maxNumToDisplay,
                  allowCreateNew,
                  allowDelete,
                  OnSavedGameSelected);
          }
          else
              PlayGamesActivator.instance.AuthenticateUser();
      }

      public void FetchAllSavedGames() {
          savedGameClient.FetchAllSavedGames(DataSource.ReadCacheOrNetwork, (SavedGameRequestStatus status, List<ISavedGameMetadata> games) => {
              if (status == SavedGameRequestStatus.Success) {
                  totalSavedSlots = games.Count;
                  Debug.LogFormat("FetchAllSavedGames() totalSavedSlots={0}", totalSavedSlots);
              }
              else
                  Debug.LogFormat("OnFetchAll error {0}", status);
          });
      }

      public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game) {
          if (status == SelectUIStatus.SavedGameSelected) {
              // handle selected game save
              Debug.LogFormat("Save selected {0}", game);
              savedGameMetadata = game;
              bool isNewSlot = (game.Filename == "");
              string filename = isNewSlot ? GetNewSlotName() : game.Filename;
              OpenSavedGame(filename, (game.Filename == ""));
          }
          else {
              // handle cancel or error            
              Debug.LogFormat("Save error {0}", status);
          }
      }

      string GetNewSlotName() {
          return "slot_" + totalSavedSlots;
      }*/

    public static void OpenSavedGame() {
        if (Social.localUser.authenticated) {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.OpenWithAutomaticConflictResolution(SAVE_FILENAME, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
        }
    }

    static void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            // handle reading or writing of saved game.
            Debug.LogFormat("OnSavedGameOpened {0}", game);
            savedGameMetadata = game;
            if (game.TotalTimePlayed.CompareTo(TimeSpan.MinValue) >= 1)
                LoadGameData(game);
            /*   if (savedGameMetadata.Filename == "")
                   SaveGame(System.Text.Encoding.ASCII.GetBytes("test"), TimeSpan.MinValue);*/
        }
        else {
            // handle error
            Debug.LogFormat("OnSavedGameOpened error {0}", status);
        }
    }

    public static void SaveGame() {
        Debug.Log("SaveGame()");
        if (Social.localUser.authenticated && savedGameMetadata != null) {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            TimeSpan totalPlaytime = new TimeSpan(0, 0, Mathf.FloorToInt(Time.realtimeSinceStartup));
            totalPlaytime = totalPlaytime.Add(savedGameMetadata.TotalTimePlayed);
            builder = builder
                .WithUpdatedPlayedTime(totalPlaytime)
                .WithUpdatedDescription(string.Format("Saved game at {0} - Playtime: {1} - {2} coin(s)", DateTime.Now, totalPlaytime, ApplicationController.ac.PlayerData.coins));
            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            byte[] savedData = ObjectToByteArray(ApplicationController.ac.PlayerData);
            savedGameClient.CommitUpdate(savedGameMetadata, updatedMetadata, savedData, OnSavedGameWritten);
        }
    }

    static void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            Debug.LogFormat("OnSavedGameWritten success {0}", game);
            savedGameMetadata = game;
            OpenSavedGame();
            //       FetchAllSavedGames();
        }
        else {
            Debug.LogFormat("OnSavedGameWritten failed {0}", status);   // handle error
        }
    }

    static void LoadGameData(ISavedGameMetadata game) {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }

    static void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data) {
        if (status == SavedGameRequestStatus.Success) {
            Debug.LogFormat("OnSavedGameDataRead() {0}", data);
            PlayerData playerData = FromByteArray<PlayerData>(data);
            Debug.LogFormat("Parsed save {0}", playerData);
            ApplicationController.ac.LoadCloudSave(playerData);
        }
        else {
            // handle error
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
