using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdsController : MonoBehaviour {

    public static AdsController instance;
    [NonSerialized] public RewardAdType currentRewardAdType;
    [SerializeField] Button[] rewardAdButtons;
    [SerializeField] CoinDisplayer[] coinDisplayers;
    [SerializeField] TextMeshProUGUI earnedCoinsText;
    [SerializeField] Animator earnedCoinsAnim;
    InterstitialAd interstitial;
    RewardBasedVideoAd rewardBasedVideo;
    bool isRequestingRewardAd = false;

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

    public void Start() {
#if UNITY_ANDROID
        string appId = Config.ADMOB_ANDROID_APP_ID;
#elif UNITY_IPHONE
        string appId = "ios_not_supported";
#else
        string appId = "unexpected_platform";
#endif
        MobileAds.Initialize(appId);

        // Get singleton reward based video ad reference.
        rewardBasedVideo = RewardBasedVideoAd.Instance;
        rewardBasedVideo.OnAdLoaded += OnRewardBasedVideoLoaded;
        rewardBasedVideo.OnAdFailedToLoad += OnRewardBasedVideoFailedToLoad;
        rewardBasedVideo.OnAdOpening += OnRewardBasedVideoOpened;
        rewardBasedVideo.OnAdStarted += OnRewardBasedVideoStarted;
        rewardBasedVideo.OnAdRewarded += OnRewardBasedVideoRewarded;
        rewardBasedVideo.OnAdClosed += OnRewardBasedVideoClosed;
        rewardBasedVideo.OnAdLeavingApplication += OnRewardBasedVideoLeftApplication;
        RequestRewardAd();
    }

    //--------------
    // INTERSTITIAL
    //--------------
    public void LoadInterstitial() {
        Debug.Log("LoadInterstitial())");
        if (!ApplicationController.ac.PlayerData.isPremium && Application.internetReachability != NetworkReachability.NotReachable) {
#if UNITY_ANDROID
            // string adUnitId = Config.ADMOB_TEST_INTERSTITIAL_ID;
            string adUnitId = Config.ADMOB_ANDROID_ENDLEVEL_INTERSTITIAL_ID;
#elif UNITY_IPHONE
            string adUnitId = "ios_not_supported";
#else
            string adUnitId = "unexpected_platform";
#endif
            interstitial = new InterstitialAd(adUnitId);
            AdRequest request = new AdRequest.Builder()
                .AddTestDevice(Config.MY_DEVICE_ID_1)
                .Build();
            interstitial.LoadAd(request);
            interstitial.OnAdClosed += OnInterstitialFinished;
            interstitial.OnAdLeavingApplication += OnInterstitialFinished;
        }
    }

    public void ShowInterstitial() {
        if (!ApplicationController.ac.PlayerData.isPremium && interstitial != null && interstitial.IsLoaded()) {
            Debug.Log("Showing Interstitial");
            interstitial.Show();
        }
        else
            Debug.Log("Don't show Interstitial");
    }

    void OnInterstitialFinished(object sender, EventArgs args) {
        Debug.Log("HandleOnAdFinished())");
        interstitial.Destroy();
        interstitial = null;
    }

    //--------------
    // REWARDED AD
    //--------------
    public void RequestRewardAd() {
        Debug.Log("RequestRewardAd");
        if (isRequestingRewardAd || (rewardBasedVideo != null && rewardBasedVideo.IsLoaded())) {
            Debug.Log("Aborted because ad already loaded or already requesting ad.");
            return;
        }
        if (Application.internetReachability != NetworkReachability.NotReachable) {
#if UNITY_ANDROID
            string adUnitId = Config.ADMOB_ANDROID_REWARD_ID;
#elif UNITY_IPHONE
            string adUnitId = "ios_not_supported";
#else
            string adUnitId = "unexpected_platform";
#endif
            AdRequest request = new AdRequest.Builder()
                .AddTestDevice(Config.MY_DEVICE_ID_1)
                .Build();
            rewardBasedVideo.LoadAd(request, adUnitId);
            isRequestingRewardAd = true;
        }
        else {
            Debug.Log("Couldn't Request Reward Ad beacause internet not reachable (retry in 10sec)");
            Invoke("RequestRewardAd", 10);
        }
    }

    public void ShowCoinsRewardAd() {
        if (rewardBasedVideo != null && rewardBasedVideo.IsLoaded()) {
            currentRewardAdType = RewardAdType.Coins;
            rewardBasedVideo.Show();
        }
    }

    public void ShowBonusesRewardAd() {
        if (rewardBasedVideo != null && rewardBasedVideo.IsLoaded()) {
            currentRewardAdType = RewardAdType.Bonuses;
            rewardBasedVideo.Show();
        }
    }

    void OnRewardBasedVideoLoaded(object sender, EventArgs args) {
        Debug.Log("OnRewardBasedVideoLoaded");
        isRequestingRewardAd = false;
        RefreshRewardAdButtons();
    }

    public void OnRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        Debug.Log("OnRewardBasedVideoFailedToLoad " + args.Message + " (retry in 10sec)");
        isRequestingRewardAd = false;
        RefreshRewardAdButtons();
        Invoke("RequestRewardAd", 10);
    }

    public void OnRewardBasedVideoOpened(object sender, EventArgs args) {
        Debug.Log("OnRewardBasedVideoOpened");
        RefreshRewardAdButtons();
    }

    public void OnRewardBasedVideoStarted(object sender, EventArgs args) {
        Debug.Log("OnRewardBasedVideoStarted");
    }

    public void OnRewardBasedVideoClosed(object sender, EventArgs args) {
        Debug.Log("OnRewardBasedVideoClosed");
        RefreshRewardAdButtons();
        RequestRewardAd();
    }

    public void OnRewardBasedVideoRewarded(object sender, Reward args) {
        Debug.Log("OnRewardBasedVideoRewarded");
        if (currentRewardAdType == RewardAdType.Coins)
            CoinsReward((int)args.Amount);
        else if (currentRewardAdType == RewardAdType.Bonuses)
            BonusesReward();
        ApplicationController.ac.Save();
    }

    public void OnRewardBasedVideoLeftApplication(object sender, EventArgs args) {
        Debug.Log("OnRewardBasedVideoLeftApplication");
    }

    void CoinsReward(int amount) {
        Debug.Log(amount.ToString() + " coins rewarded");
        ApplicationController.ac.UpdateCoins(amount);
        earnedCoinsText.text = "+ " + amount.ToString() + " <sprite name=\"coin\">";
        earnedCoinsAnim.SetTrigger("tick");
        Invoke("RefreshCoinDisplayers", 1.8f);
    }

    void BonusesReward() {
        Debug.Log("Bonuses rewarded");
        // [clear the bonuses cooldown here]
        // RefreshBonusesIcons();
    }

    void RefreshRewardAdButtons() {
        Debug.Log("RefreshRewardAdButtons");
        if (rewardAdButtons != null) {
            bool enable = (rewardBasedVideo != null && rewardBasedVideo.IsLoaded());
            foreach (Button button in rewardAdButtons) {
                button.interactable = enable;
            }
        }
    }

    void RefreshCoinDisplayers() {
        Debug.Log("RefreshCoinDisplayers");
        if (coinDisplayers != null) {
            foreach (CoinDisplayer coinDisplayer in coinDisplayers) {
                coinDisplayer.RefreshUI();
            }
        }
    }

    public void OnDestroy() {
        if (rewardBasedVideo != null) {
            rewardBasedVideo.OnAdLoaded -= OnRewardBasedVideoLoaded;
            rewardBasedVideo.OnAdFailedToLoad -= OnRewardBasedVideoFailedToLoad;
            rewardBasedVideo.OnAdOpening -= OnRewardBasedVideoOpened;
            rewardBasedVideo.OnAdStarted -= OnRewardBasedVideoStarted;
            rewardBasedVideo.OnAdRewarded -= OnRewardBasedVideoRewarded;
            rewardBasedVideo.OnAdClosed -= OnRewardBasedVideoClosed;
            rewardBasedVideo.OnAdLeavingApplication -= OnRewardBasedVideoLeftApplication;
        }
        if (interstitial != null) {
            interstitial.OnAdClosed -= OnInterstitialFinished;
            interstitial.OnAdLeavingApplication -= OnInterstitialFinished;
            interstitial.Destroy();
        }
    }

    public enum RewardAdType { Coins, Bonuses }
}
