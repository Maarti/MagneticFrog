using GooglePlayGames;
using System;
using UnityEngine;

public class Achievement : MonoBehaviour {

    public static void Unlock(string achievementId) {
        try {
            if (Social.localUser.authenticated)
                Social.ReportProgress(achievementId, 100.0f, null);
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    public static void Reveal(string achievementId) {
        try {
            if (Social.localUser.authenticated)
                Social.ReportProgress(achievementId, 0.0f, null);
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    public static void Increment(string achievementId, int step) {
        try {
            if (Social.localUser.authenticated)
                PlayGamesPlatform.Instance.IncrementAchievement(achievementId, step, null);
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

    public static void ShowUI() {
        try {
            if (Social.localUser.authenticated)
                Social.ShowAchievementsUI();
            else
                PlayGamesActivator.instance.AuthenticateUser();
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }
    public void ShowAchievementsUI() {
        ShowUI();
    }

    /** Unlock achievements according to the score recorded */
    public static void UnlockScore(int score) {
        try {
            if (Social.localUser.authenticated) {
                if (score >= 5000)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_5000);
                if (score >= 4000)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_4000);
                if (score >= 3000)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_3000);
                if (score >= 2000)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_2000);
                if (score >= 1500)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_1500);
                if (score >= 1000)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_1000);
                if (score >= 750)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_750);
                if (score >= 500)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_500);
                if (score >= 200)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_200);
                if (score >= 100)
                    Unlock(GPGSIds.achievement_to_infinity_and_beyond_100);
            }
        }
        catch (Exception e) {
            Debug.LogError(e);
        }
    }

}
