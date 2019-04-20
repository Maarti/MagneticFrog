using System;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdsController : MonoBehaviour {

    public static AdsController instance;
    InterstitialAd interstitial;

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
    }
    
    public void LoadInterstitial() {
        Debug.Log("LoadInterstitial())");
        if (!ApplicationController.ac.PlayerData.isPremium && Application.internetReachability != NetworkReachability.NotReachable) {
#if UNITY_ANDROID
            // string adUnitId = Config.ADMOB_TEST_INTERSTITIAL_ID;
            string adUnitId = Config.ADMOB_ENDLEVEL_INTERSTITIAL_ID;
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
            interstitial.OnAdClosed += HandleOnAdFinished;
            interstitial.OnAdLeavingApplication += HandleOnAdFinished;
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

    void HandleOnAdFinished(object sender, EventArgs args) {
        Debug.Log("HandleOnAdFinished())");
        this.interstitial.Destroy();
        this.interstitial = null;
    }


    }
