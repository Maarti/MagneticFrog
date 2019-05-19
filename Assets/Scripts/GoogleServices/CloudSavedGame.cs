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
    public ISavedGameMetadata savedGameMetadata;
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

    public void Init() {
        Debug.LogFormat("CloudSavedGame.Init() authent={0}", Social.localUser.authenticated);
        if (Social.localUser.authenticated) {
            savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            FetchAllSavedGames();
        }
    }

    public void ShowSelectUI() {
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
    }

    void OpenSavedGame(string filename, bool saveImmediately = false) {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            // handle reading or writing of saved game.
            Debug.LogFormat("OnSavedGameOpened {0}", game);
            if (savedGameMetadata.Filename == "")
                SaveGame(System.Text.Encoding.ASCII.GetBytes("test"), TimeSpan.MinValue);
            savedGameMetadata = game;
        }
        else {
            // handle error
            Debug.LogFormat("OnSavedGameOpened error {0}", status);
        }
    }

    public void SaveGame(byte[] savedData, TimeSpan totalPlaytime) {
        if (Social.localUser.authenticated) {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            builder = builder
                .WithUpdatedPlayedTime(totalPlaytime)
                .WithUpdatedDescription(string.Format("Saved game at {0} - {1} coin(s)", DateTime.Now, ApplicationController.ac.PlayerData.coins));

            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            savedGameClient.CommitUpdate(savedGameMetadata, updatedMetadata, savedData, OnSavedGameWritten);
        }
    }

    public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            Debug.LogFormat("OnSavedGameWritten success {0}", game);
            FetchAllSavedGames();
        }
        else {
            Debug.LogFormat("OnSavedGameWritten failed {0}", status);   // handle error
        }
    }

    public static byte[] ObjectToByteArray(object obj) {
        if (obj == null)
            return null;
        BinaryFormatter bf = new BinaryFormatter();
        using (MemoryStream ms = new MemoryStream()) {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }

}
