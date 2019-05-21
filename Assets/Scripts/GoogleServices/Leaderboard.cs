using GooglePlayGames;
using System;
using UnityEngine;

public class Leaderboard : MonoBehaviour {

    public void ShowLeaderboardUI() {
        try {
            if (Social.localUser.authenticated)
                Social.ShowLeaderboardUI();
            else
                PlayGamesActivator.instance.AuthenticateUser();
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    public void ShowBestScoresLeaderboardUI() {
        try {
            if (Social.localUser.authenticated)
                PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_best_scores);
            else
                PlayGamesActivator.instance.AuthenticateUser();
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    public static void ReportScore(int score) {
        try {
            if (Social.localUser.authenticated)
                Social.ReportScore(score, GPGSIds.leaderboard_best_scores, (bool success) => Debug.LogFormat("ReportScore {0} to leaderboard: {1}", score, success));
            else
                Debug.Log("Local user not authenticated => not reporting score");
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

}
