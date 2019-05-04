﻿using GooglePlayGames;
using UnityEngine;

public class Leaderboard : MonoBehaviour {

    public void ShowLeaderboardUI() {
        if (Social.localUser.authenticated)
            Social.ShowLeaderboardUI();
        else
            PlayGamesActivator.instance.AuthenticateUser();            
    }

    public void ShowBestScoresLeaderboardUI() {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_best_scores);
    }

    public static void ReportScore(int score) {
        if (Social.localUser.authenticated)
            Social.ReportScore(score, GPGSIds.leaderboard_best_scores, (bool success) => Debug.LogFormat("ReportScore {0} to leaderboard: {1}", score, success));
        else
            Debug.Log("Local user not authenticated => not reporting score");
    }
}
