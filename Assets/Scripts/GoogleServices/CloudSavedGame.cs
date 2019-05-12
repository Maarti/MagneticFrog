using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using UnityEngine;

public class CloudSavedGame : MonoBehaviour {

    public static CloudSavedGame instance;
    public ISavedGameMetadata savedGameMetadata;
    Texture2D savedImage;

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

    public void ShowSelectUI() {
     //   savedImage = GetScreenshot();
        if (Social.localUser.authenticated) {
            uint maxNumToDisplay = 5;
            bool allowCreateNew = true;
            bool allowDelete = true;

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ShowSelectSavedGameUI("Select saved game",
                maxNumToDisplay,
                allowCreateNew,
                allowDelete,
                OnSavedGameSelected);
        }else
            Debug.LogFormat("ShowSelectUI error not authenticated");
    }


    public void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game) {
        if (status == SelectUIStatus.SavedGameSelected) {
            // handle selected game save
            Debug.LogFormat("Save selected {0}", game);
            savedGameMetadata = game;
            OpenSavedGame("slot_1");
        }
        else {
            // handle cancel or error            
            Debug.LogFormat("Save error {0}", status);
        }
    }

    void OpenSavedGame(string filename) {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpened);
    }

    public void OnSavedGameOpened(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            // handle reading or writing of saved game.
            Debug.LogFormat("OnSavedGameOpened {0}", game);
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
                .WithUpdatedDescription("Saved game at " + DateTime.Now);
            if (savedImage != null) {
                // This assumes that savedImage is an instance of Texture2D and that you have already called a function equivalent to getScreenshot() to set savedImage
                byte[] pngData = savedImage.EncodeToPNG();
                builder = builder.WithUpdatedPngCoverImage(pngData);
            }
            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            savedGameClient.CommitUpdate(savedGameMetadata, updatedMetadata, savedData, OnSavedGameWritten);
        }
    }

    public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game) {
        if (status == SavedGameRequestStatus.Success) {
            Debug.LogFormat("OnSavedGameWritten suvvess {0}", game);
        }
        else {
            Debug.LogFormat("OnSavedGameWritten failed {0}", status);   // handle error
        }
    }

    public Texture2D GetScreenshot() {
        // Create a 2D texture that is 1024x700 pixels from which the PNG will be extracted
        Texture2D screenShot = new Texture2D(1024, 700);

        // Takes the screenshot from top left hand corner of screen and maps to top left hand corner of screenShot texture
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, (Screen.width / 1024) * 700), 0, 0);
        return screenShot;
    }
}
