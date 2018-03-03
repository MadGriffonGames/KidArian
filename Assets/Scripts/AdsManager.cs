﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    private static AdsManager instance;
    public static AdsManager Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType<AdsManager>();
            return instance;
        }
    }

    [SerializeField]
    public GameObject networkWarningPrefab;

    GameObject warning;

    bool isLvlEnd;

    public bool isInterstitialClosed = false;
    public bool isRewardVideoWatched = false;
    public bool isRewardedVideoFailed = false;
    public bool isRewardedVideoShown = false;

    InterstitialAd interstitial;
    private RewardBasedVideoAd adMobRewardedVideo;

    public IAdsPlacement currentPlacement;
    bool isRewardGiven;

    string appKey;
    string unityGameId;
    string adMobAppId;

    bool isVideoWatched;
    bool isVideoFailed;
    bool isTimerTick;
    bool isDelayComplete;
    float timer;
    const float DELAY_TIME = 1f;

    private void Awake()
    {
        AppMetrica.Instance.ActivateWithAPIKey("cefd065c-fd53-443d-9f27-9ddf930a936f");

        isVideoWatched = false;
        isVideoFailed = false;
        isTimerTick = false;
        isDelayComplete = false;
    }

    void Start ()
    {

#if UNITY_ANDROID
        appKey = "e98a9abebc918269e0b487f18fd271b1313447f412d4561e";
        unityGameId = "1671703";
        adMobAppId = "ca-app-pub-7702587672519508~1294300679";

#elif UNITY_IOS
		appKey = "c9868b381a1331f2a7d3d5b4dd9bc721f403735f82deebff";
        unityGameId = "1671704";
        adMobAppId = "ca-app-pub-7702587672519508~6806924428";

#endif

        Advertisement.Initialize(unityGameId);
        MobileAds.Initialize(adMobAppId);

        RequestInterstitial();
        RequestRewardedVideo();

        isInterstitialClosed = false;
    }

    private void Update()
    {
        if (isTimerTick)
        {
            timer += Time.deltaTime;
        }

        if (timer >= DELAY_TIME)
        {
            timer = 0;
            isTimerTick = false;
            isDelayComplete = true;
        }

        if (isVideoWatched && isDelayComplete)
        {
            isVideoWatched = false;
            isDelayComplete = false;
            currentPlacement.OnRewardedVideoWatched();

            if (SoundManager.Instance.currentMusic != null)
            {
                SoundManager.Instance.currentMusic.UnPause();
            }
        }
        else if (isVideoFailed && isDelayComplete)
        {
            isVideoFailed = false;
            isDelayComplete = false;
            currentPlacement.OnRewardedVideoFailed();

            if (SoundManager.Instance.currentMusic != null)
            {
                SoundManager.Instance.currentMusic.UnPause();
            }
        }
    }

    void RequestRewardedVideo()
    {
        adMobRewardedVideo = RewardBasedVideoAd.Instance;

#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7702587672519508/5527360718";
#elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-7702587672519508/4520985510";
#else
            string adUnitId = "unexpected_platform";
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Called when the user should be rewarded for watching a video.
        adMobRewardedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        adMobRewardedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when an ad request failed to load.
        adMobRewardedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Load the rewarded video ad with the request.
        this.adMobRewardedVideo.LoadAd(request, adUnitId);
    }

    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-7702587672519508/4784645751";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-7702587672519508/2187779299";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Called when the ad is closed.
        interstitial.OnAdClosed += HandleOnAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    public void ShowAdsAtLevelEnd()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
            RequestInterstitial();
        }
        else
        {
            isInterstitialClosed = true;
        }
    }

   public void ShowRewardedVideo(IAdsPlacement _currentPlacemnt)
    {
        currentPlacement = _currentPlacemnt;
        Debug.LogError("Changed current placement on" + _currentPlacemnt);

        isRewardGiven = false;
        if (SoundManager.Instance.currentMusic != null)
        {
            SoundManager.Instance.currentMusic.Pause();
        }
#if UNITY_EDITOR
        OnVideoWatched();
#endif

        if (CanNotShowRewardedVideo())
        {
            InstantiateWarning();
        }
        else
        {
            if (PlayerPrefs.GetInt("NoAds") == 0)
            {
                int tmp = Random.Range(1, 3);
                if (tmp == 1)
                {
                    if (Advertisement.IsReady())
                    {
                        Debug.Log("UnityAds");
                        UnityAdsShowRewardedVideo();
                    }
                    else
                    {
						#if UNITY_ANDROID
                        if (adMobRewardedVideo.IsLoaded())
                        {
                            Debug.Log("AdMob");
                            AdMobShowRewardedVideo();
                        }
						#else
							UnityAdsShowRewardedVideo();
						#endif
                    }
                }
                else
                {
					#if UNITY_ANDROID
                    if (adMobRewardedVideo.IsLoaded())
                    {
                        Debug.Log("AdMob");
                        AdMobShowRewardedVideo();
                    }
                    else
                    {
                        if (Advertisement.IsReady())
                        {
                            Debug.Log("UnityAds");
                            UnityAdsShowRewardedVideo();
                        }
                    }
					#else
					if (Advertisement.IsReady())
					{
						Debug.Log("UnityAds");
						UnityAdsShowRewardedVideo();
					}
					#endif
                }
            }
            else
            {
                OnVideoWatched();
            }
        }
    }

    void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            Debug.Log("Video completed - Offer a reward to the player");
            OnVideoWatched();
        }
        else if (result == ShowResult.Skipped)
        {
            Debug.LogWarning("Video was skipped - Do NOT reward the player");
            OnVideoFailed();
        }
        else if (result == ShowResult.Failed)
        {
            Debug.LogError("Video failed to show");
            OnVideoFailed();
        }
    }

    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        Debug.LogError("Video closrd to show");

        isInterstitialClosed = true;
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        OnVideoWatched();

        string type = args.Type;
        double amount = args.Amount;
        Debug.LogError("Video completed - Offer a reward to the player");
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        OnVideoFailed();

        Debug.LogError("Video failed to show");
    }

    public void HandleRewardBasedVideoClosed(object sender, System.EventArgs args)
    {
        OnVideoFailed();

        Debug.LogError("Video skiped");
    }

    //other

    void UnityAdsShowRewardedVideo()
    {
        ShowOptions options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
    }

    void AdMobShowRewardedVideo()
    {
        adMobRewardedVideo.Show();
        RequestRewardedVideo();
    }

    bool CanNotShowRewardedVideo()
    {
        return PlayerPrefs.GetInt("NoAds") == 0 && Application.internetReachability == NetworkReachability.NotReachable;
    }

    void InstantiateWarning()
    {
        if (SceneManager.GetActiveScene().name.Contains("Level"))
        {
#if !UNITY_IOS
            if (GameObject.Find("NetworkWarning(Clone)") == null)//if there is no active warnings in scene
            {
                warning = Instantiate(networkWarningPrefab, GameObject.FindObjectOfType<UI>().gameObject.transform);
                warning.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            }
#endif
        }
        else
        {
#if !UNITY_IOS
            if (GameObject.Find("NetworkWarning(Clone)") == null)//if there is no active warnings in scene
            {
                GameObject ui;
                ui = GameObject.FindGameObjectWithTag("UI").gameObject;

                warning = Instantiate(networkWarningPrefab, ui.transform);
                warning.GetComponent<RectTransform>().localScale = new Vector2(2, 2);
            }
#endif
        }
        warning.GetComponent<RectTransform>().localPosition = new Vector2();

        OnVideoFailed(); 
    }

    void OnVideoWatched()
    {
        isVideoWatched = true;
        isTimerTick = true;
    }

    void OnVideoFailed()
    {
        isVideoFailed = true;
        isTimerTick = true;
    }
}
