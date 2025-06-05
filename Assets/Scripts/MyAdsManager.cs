//using System;
//using UnityEngine;
//using GoogleMobileAds;
//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;
//using UnityEngine.Advertisements;

//public enum AdType
//{
//    TestAds, LiveAds
//}
//public enum AdPriority
//{
//    Admob,
//    Unity
//}

//public class MyAdsManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
//{


//    public AdType adType;
//    public AdPriority adPriority;
//    public AdPosition bannerPosition;

//    public string appID;
//    [Header("Admob Banner")]
//    public string twoGbAdmobBannerId;
//    public string aboveTwoGbadmobBannerId;
//    [Header("Admob interstitial")]
//    public string twoGbAdmobInterstitalId;
//    public string aboveTwoGbadmobInterstitalId;
//    [Header("Admob Rewarded")]
//    public string admobRewardedId;
//    [Header("Admob App Open id")]
//    public string admobAppOpenId;

//    [Header("Admob Ids Active")]
//    public string admobBannerId;
//    public string admobRectBannerId;
//    public string admobInterstitalId;

//    public string unityId;
//    public string unityInterstitalId = "Interstitial_Android";
//    public string unityRewardedId = "Rewarded_Android";

//    //private static bool? _isInitialized;
//    private BannerView _bannerView;
//    private BannerView _rectBannerView;
//    private InterstitialAd _interstitialAd;
//    private RewardedAd _rewardedAd;

//    public static Action onRewardedVideoCompleteEvent;

//    public delegate void actionInterstitialAdClose();
//    public event actionInterstitialAdClose onInterstitialAdCloseEvent;

//    public static MyAdsManager instance;

//    public static MyAdsManager Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                instance = FindObjectOfType<MyAdsManager>();
//                if (instance && instance.gameObject)
//                {
//                    DontDestroyOnLoad(instance.gameObject);
//                }
//            }
//            return instance;
//        }
//    }


//    private void Awake()
//    {
//        if (instance == null)
//        {
//            //If I am the first instance, make me the Singleton
//            instance = this;
//            DontDestroyOnLoad(transform.gameObject);
//        }
//        else
//        {
//            //If a Singleton already exists and you find
//            //another reference in scene, destroy it!
//            if (this != instance)
//            {
//                Destroy(this.gameObject);
//            }
//        }
//        //MobileAds.RaiseAdEventsOnUnityMainThread = true;
//        InitializeGoogleMobileAds();
//        // Use the AppStateEventNotifier to listen to application open/close events.
//        // This is used to launch the loaded ad when we open the app.
//    }
//    private void Start()
//    {

//        Invoke(nameof(LoadAllAds), 2);
//    }

//    #region initialization events
//    /// <summary>
//    /// Initializes the Google Mobile Ads Unity plugin.
//    /// </summary>

//    private void InitializeGoogleMobileAds()
//    {
//        MobileAds.Initialize(initStatus => { Debug.Log("Initialized"); });
//        if (adType == AdType.TestAds)
//        {
//            Advertisement.Initialize(unityId, true, this);
//        }
//        else
//        {
//            Advertisement.Initialize(unityId, false, this);
//        }
//    }
//    public void LoadAllAds()
//    {
//        SetAdIds();
//        LoadAdmobInterstitialAd();
//        LoadAdmobRewardedAd();
//        LoadUnityInterstitial();
//        LoadUnityRewarded();
//        //LoadBanner();
//        //LoadRectBanner(AdPosition.Bottom);
//        //LoadAppOpenAd();
//        //AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
//        //ShowAppOpenAd();
//    }


//    private void SetAdIds()
//    {
//        if (adType == AdType.TestAds)
//        {
//            appID = "ca-app-pub-3940256099942544~3347511713";
//            admobInterstitalId = "ca-app-pub-3940256099942544/1033173712";
//            admobBannerId = "ca-app-pub-3940256099942544/6300978111";
//            admobRectBannerId = "ca-app-pub-3940256099942544/6300978111";
//            admobRewardedId = "ca-app-pub-3940256099942544/5224354917";
//            admobAppOpenId = "ca-app-pub-3940256099942544/3419835294";
//            Debug.unityLogger.logEnabled = true;
//        }
//        else if (adType == AdType.LiveAds)
//        {
//            if (SystemInfo.systemMemorySize <3072)
//            {
//                admobInterstitalId = twoGbAdmobBannerId;
//                admobBannerId = twoGbAdmobBannerId;
//            }
//            else
//            {
//                admobInterstitalId = aboveTwoGbadmobInterstitalId;
//                admobBannerId = aboveTwoGbadmobBannerId;
//            }
//            Debug.unityLogger.logEnabled = false;
//        }
//    }
//    #endregion initialization events
//    #region ad calling methods
//    public void ChangePriority()
//    {
//        if (adPriority == AdPriority.Admob)
//        {
//            adPriority = AdPriority.Unity;
//        }
//        else
//        {
//            adPriority = AdPriority.Admob;
//        }
//    }
//    //public void ShowRewarded()
//    //{
//    //    ShowRewardedVideo(adPriority);
//    //}
//    //public void ShowInterstitialAds()
//    //{
//    //    ShowInterstitialAds(adPriority);
//    //}
//    public void ShowRewardedVideo(Action rewardedFuction)
//    {
//        Debug.Log(adPriority);
//        onRewardedVideoCompleteEvent = null;
//        onRewardedVideoCompleteEvent = rewardedFuction;
//        if (adPriority == AdPriority.Unity)
//        {
//            if (IsUnityRewardedAvailable())
//            {
//                ShowUnityRewardedVideo();
//            }
//            else
//            {
//                ShowAdmobRewardedAd();
//                LoadUnityRewarded();
//            }
//        }
//        else if (adPriority == AdPriority.Admob)
//        {
//            if (_rewardedAd != null && _rewardedAd.CanShowAd())
//            {
//                ShowAdmobRewardedAd();
//            }
//            else
//            {
//                ShowUnityRewardedVideo();
//                LoadAdmobRewardedAd();
//                Debug.LogError("Rewarded ad is not ready yet.");
//            }
//        }
//    }
//    public void ShowInterstitialAds()
//    {
//        if (adPriority == AdPriority.Admob)
//        {
//            Debug.Log(1);
//            if (_interstitialAd != null && _interstitialAd.CanShowAd())
//            {
//                Debug.Log(1.1);
//                ShowAdmobInterstitialAd();
//            }
//            else
//            {
//                Debug.Log(1.2);
//                ShowUnityInterstitial();
//                LoadAdmobInterstitialAd();
//            }
//        }
//        else if (adPriority == AdPriority.Unity)
//        {
//            Debug.Log(2);
//            if (IsUnityInterstitialAvailable())
//            {
//                Debug.Log(2.1);
//                ShowUnityInterstitial();
//            }
//            else
//            {
//                Debug.Log(2.2);
//                ShowAdmobInterstitialAd();
//                LoadUnityInterstitial();
//            }
//        }
//    }
//    #endregion ad calling methods
//    #region admob banner ad
//    /// <summary>
//    /// Creates a 320x50 banner at top of the screen.
//    /// </summary>
//    public void CreateBannerView()
//    {
//        Debug.Log("Creating banner view.");

//        // If we already have a banner, destroy the old one.
//        if (_bannerView != null)
//        {
//            DestroyBanner();
//        }

//        // Create a 320x50 banner at top of the screen.
//        _bannerView = new BannerView(admobBannerId, AdSize.Banner, bannerPosition);

//        // Listen to events the banner may raise.
//        ListenToAdEvents();

//        Debug.Log("Banner view created.");
//    }

//    /// <summary>
//    /// Creates the banner view and loads a banner ad.
//    /// </summary>
//    public void LoadBanner()
//    {
//        // Create an instance of a banner view first.
//        if (_bannerView == null)
//        {
//            CreateBannerView();
//        }

//        // Create our request used to load the ad.
//        var adRequest = new AdRequest();

//        // Send the request to load the ad.
//        Debug.Log("Loading banner ad.");
//        _bannerView.LoadAd(adRequest);
//    }

//    /// <summary>
//    /// Shows the ad.
//    /// </summary>
//    public void ShowBanner()
//    {
//        if (_bannerView != null)
//        {
//            Debug.Log("Showing banner view.");
//            _bannerView.Show();
//        }
//        else
//        {
//            LoadBanner();//else load ad
//        }
//    }

//    /// <summary>
//    /// Hides the ad.
//    /// </summary>
//    public void HideBanner()
//    {
//        if (_bannerView != null)
//        {
//            Debug.Log("Hiding banner view.");
//            _bannerView.Hide();
//        }
//    }

//    /// <summary>
//    /// Destroys the ad.
//    /// When you are finished with a BannerView, make sure to call
//    /// the Destroy() method before dropping your reference to it.
//    /// </summary>
//    public void DestroyBanner()
//    {
//        if (_bannerView != null)
//        {
//            Debug.Log("Destroying banner view.");
//            _bannerView.Destroy();
//            _bannerView = null;
//        }
//    }

//    /// <summary>
//    /// Listen to events the banner may raise.
//    /// </summary>
//    private void ListenToAdEvents()
//    {
//        // Raised when an ad is loaded into the banner view.
//        _bannerView.OnBannerAdLoaded += () =>
//        {
//            Debug.Log("Banner view loaded an ad with response : "
//                + _bannerView.GetResponseInfo());
//        };
//        // Raised when an ad fails to load into the banner view.
//        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
//        {
//            Debug.LogError("Banner view failed to load an ad with error : " + error);
//        };
//        // Raised when the ad is estimated to have earned money.
//        _bannerView.OnAdPaid += (AdValue adValue) =>
//        {
//            Debug.Log(String.Format("Banner view paid {0} {1}.",
//                adValue.Value,
//                adValue.CurrencyCode));
//        };
//        // Raised when an impression is recorded for an ad.
//        _bannerView.OnAdImpressionRecorded += () =>
//        {
//            Debug.Log("Banner view recorded an impression.");
//        };
//        // Raised when a click is recorded for an ad.
//        _bannerView.OnAdClicked += () =>
//        {
//            Debug.Log("Banner view was clicked.");
//        };
//        // Raised when an ad opened full screen content.
//        _bannerView.OnAdFullScreenContentOpened += () =>
//        {
//            Debug.Log("Banner view full screen content opened.");
//        };
//        // Raised when the ad closed full screen content.
//        _bannerView.OnAdFullScreenContentClosed += () =>
//        {
//            Debug.Log("Banner view full screen content closed.");
//        };
//    }

//    #endregion end admob banner
//    #region admob Rect banner ad
//    /// <summary>
//    /// Creates a 320x50 banner at top of the screen.
//    /// </summary>
//    public void CreateRectBannerView(AdPosition _adPosition)
//    {
//        Debug.Log("Creating rect banner view.");

//        // If we already have a banner, destroy the old one.
//        if (_rectBannerView != null)
//        {
//            DestroyRectBanner();
//        }

//        // Create a 320x50 banner at top of the screen.
//        _rectBannerView = new BannerView(admobRectBannerId, AdSize.MediumRectangle, _adPosition);

//        // Listen to events the banner may raise.
//        ListenToAdEventsRectBanner();

//        Debug.Log("rect Banner view created.");
//    }

//    /// <summary>
//    /// Creates the banner view and loads a banner ad.
//    /// </summary>
//    public void LoadRectBanner(AdPosition _adPosition)
//    {
//        // Create an instance of a banner view first.
//        if (_rectBannerView == null)
//        {
//            CreateRectBannerView(_adPosition);
//        }

//        // Create our request used to load the ad.
//        var adRequest = new AdRequest();

//        // Send the request to load the ad.
//        Debug.Log("Loading rect banner ad.");
//        _rectBannerView.LoadAd(adRequest);
//    }

//    /// <summary>
//    /// Shows the ad.
//    /// </summary>
//    public void ShowRectBanner(AdPosition _adPosition)
//    {
//        if (_rectBannerView != null)
//        {
//            Debug.Log("Showing rect banner view.");
//            _rectBannerView.Show();
//        }
//        else
//        {
//            LoadRectBanner(_adPosition);//else load ad
//        }
//    }

//    /// <summary>
//    /// Hides the ad.
//    /// </summary>
//    public void HideRectBanner()
//    {
//        if (_rectBannerView != null)
//        {
//            Debug.Log("Hiding rect banner view.");
//            _rectBannerView.Hide();
//        }
//    }

//    /// <summary>
//    /// Destroys the ad.
//    /// When you are finished with a BannerView, make sure to call
//    /// the Destroy() method before dropping your reference to it.
//    /// </summary>
//    public void DestroyRectBanner()
//    {
//        if (_rectBannerView != null)
//        {
//            Debug.Log("Destroying rect banner view.");
//            _rectBannerView.Destroy();
//            _rectBannerView = null;
//        }
//    }

//    /// <summary>
//    /// Listen to events the banner may raise.
//    /// </summary>
//    private void ListenToAdEventsRectBanner()
//    {
//        // Raised when an ad is loaded into the banner view.
//        _rectBannerView.OnBannerAdLoaded += () =>
//        {
//            Debug.Log("Banner view loaded an ad with response : "
//                + _rectBannerView.GetResponseInfo());
//        };
//        // Raised when an ad fails to load into the banner view.
//        _rectBannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
//        {
//            Debug.LogError("Banner view failed to load an ad with error : " + error);
//        };
//        // Raised when the ad is estimated to have earned money.
//        _rectBannerView.OnAdPaid += (AdValue adValue) =>
//        {
//            Debug.Log(String.Format("Banner view paid {0} {1}.",
//                adValue.Value,
//                adValue.CurrencyCode));
//        };
//        // Raised when an impression is recorded for an ad.
//        _rectBannerView.OnAdImpressionRecorded += () =>
//        {
//            Debug.Log("Banner view recorded an impression.");
//        };
//        // Raised when a click is recorded for an ad.
//        _rectBannerView.OnAdClicked += () =>
//        {
//            Debug.Log("Banner view was clicked.");
//        };
//        // Raised when an ad opened full screen content.
//        _rectBannerView.OnAdFullScreenContentOpened += () =>
//        {
//            Debug.Log("Banner view full screen content opened.");
//        };
//        // Raised when the ad closed full screen content.
//        _rectBannerView.OnAdFullScreenContentClosed += () =>
//        {
//            Debug.Log("Banner view full screen content closed.");
//        };
//    }

//    #endregion end admob Rect banner
//    #region admob interstitial Ad
//    public void LoadAdmobInterstitialAd()
//    {

//        if (SystemInfo.systemMemorySize >=3072 && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
//            || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
//        {
//            // Clean up the old ad before loading a new one.
//            if (_interstitialAd != null)
//            {
//                DestroyAdmobInterstitialAd();
//            }

//            Debug.Log("Loading interstitial ad.");

//            // Create our request used to load the ad.
//            var adRequest = new AdRequest();

//            // Send the request to load the ad.
//            InterstitialAd.Load(admobInterstitalId, adRequest, (InterstitialAd ad, LoadAdError error) =>
//            {
//                // If the operation failed with a reason.
//                if (error != null)
//                {
//                    Debug.LogError("Interstitial ad failed to load an ad with error : " + error);
//                    return;
//                }
//                // If the operation failed for unknown reasons.
//                // This is an unexpected error, please report this bug if it happens.
//                if (ad == null)
//                {
//                    Debug.LogError("Unexpected error: Interstitial load event fired with null ad and null error.");
//                    return;
//                }

//                // The operation completed successfully.
//                Debug.Log("Interstitial ad loaded with response : " + ad.GetResponseInfo());
//                _interstitialAd = ad;

//                // Register to ad events to extend functionality.
//                RegisterEventHandlers(ad);
//            });
//        }
        
//    }

//    /// <summary>
//    /// Shows the ad.
//    /// </summary>
//    public void ShowAdmobInterstitialAd()
//    {
//      if (SystemInfo.systemMemorySize >= 3072 && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
//            || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
//      {
//        if (_interstitialAd != null && _interstitialAd.CanShowAd())
//        {
//            Debug.Log("Showing interstitial ad.");
//            _interstitialAd.Show();
//        }
//        else
//        {
//            LoadAdmobInterstitialAd();
//            Debug.LogError("Interstitial ad is not ready yet.");
//        }

//      }

//    }

//    /// <summary>
//    /// Destroys the ad.
//    /// </summary>
//    public void DestroyAdmobInterstitialAd()
//    {
//        if (_interstitialAd != null)
//        {
//            Debug.Log("Destroying interstitial ad.");
//            _interstitialAd.Destroy();
//            _interstitialAd = null;
//        }

//    }


//    private void RegisterEventHandlers(InterstitialAd ad)
//    {
//        // Raised when the ad is estimated to have earned money.
//        ad.OnAdPaid += (AdValue adValue) =>
//        {
//            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
//                adValue.Value,
//                adValue.CurrencyCode));
//            LoadAdmobInterstitialAd();

//        };
//        // Raised when an impression is recorded for an ad.
//        ad.OnAdImpressionRecorded += () =>
//        {
//            Debug.Log("Interstitial ad recorded an impression.");
//        };
//        // Raised when a click is recorded for an ad.
//        ad.OnAdClicked += () =>
//        {
//            Debug.Log("Interstitial ad was clicked.");
//        };
//        // Raised when an ad opened full screen content.
//        ad.OnAdFullScreenContentOpened += () =>
//        {
//            Debug.Log("Interstitial ad full screen content opened.");
//        };
//        // Raised when the ad closed full screen content.
//        ad.OnAdFullScreenContentClosed += () =>
//        {
//            Debug.Log("Interstitial ad full screen content closed.");
//            LoadAdmobInterstitialAd();
//        };
//        // Raised when the ad failed to open full screen content.
//        ad.OnAdFullScreenContentFailed += (AdError error) =>
//        {
//            Debug.LogError("Interstitial ad failed to open full screen content with error : "
//                + error);
//            LoadAdmobInterstitialAd();
//        };

//    }
//    #endregion end admob interstitial ad
//    #region admob rewarded Ad
//    public void LoadAdmobRewardedAd()
//    {
//        if (SystemInfo.systemMemorySize >= 3072 && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
//              || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
//        {
//            // Clean up the old ad before loading a new one.
//            if (_rewardedAd != null)
//            {
//                DestroyAdmobRewardedAd();
//            }

//            Debug.Log("Loading rewarded ad.");

//            // Create our request used to load the ad.
//            var adRequest = new AdRequest();

//            // Send the request to load the ad.
//            RewardedAd.Load(admobRewardedId, adRequest, (RewardedAd ad, LoadAdError error) =>
//            {
//            // If the operation failed with a reason.
//            if (error != null)
//                {
//                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
//                    return;
//                }
//            // If the operation failed for unknown reasons.
//            // This is an unexpected error, please report this bug if it happens.
//            if (ad == null)
//                {
//                    Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
//                    return;
//                }

//            // The operation completed successfully.
//            Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
//                _rewardedAd = ad;

//            // Register to ad events to extend functionality.
//            RegisterEventHandlers(ad);


//            });
//        }
//    }

//    /// <summary>
//    /// Shows the ad.
//    /// </summary>
//    public void ShowAdmobRewardedAd()
//    {
//        if (SystemInfo.systemMemorySize >= 3072 && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
//                || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
//        {
//            if (_rewardedAd != null && _rewardedAd.CanShowAd())
//            {
//                Debug.Log("Showing rewarded ad.");
//                _rewardedAd.Show((Reward reward) =>
//                {
//                    Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
//                                            reward.Amount,
//                                            reward.Type));
//                });
//            }
//            else
//            {
//                LoadAdmobRewardedAd();
//                Debug.LogError("Rewarded ad is not ready yet.");
//            }

//        }
//    }

//    /// <summary>
//    /// Destroys the ad.
//    /// </summary>
//    public void DestroyAdmobRewardedAd()
//    {
//        if (_rewardedAd != null)
//        {
//            Debug.Log("Destroying rewarded ad.");
//            _rewardedAd.Destroy();
//            _rewardedAd = null;
//        }


//    }
//    private void RegisterEventHandlers(RewardedAd ad)
//    {
//        // Raised when the ad is estimated to have earned money.
//        ad.OnAdPaid += (AdValue adValue) =>
//        {
//            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
//                adValue.Value,
//                adValue.CurrencyCode));
//            if (onRewardedVideoCompleteEvent != null)
//            {
//                onRewardedVideoCompleteEvent.Invoke();
//                onRewardedVideoCompleteEvent = null;
//            }
//            LoadAdmobRewardedAd();
//        };
//        // Raised when an impression is recorded for an ad.
//        ad.OnAdImpressionRecorded += () =>
//        {
//            Debug.Log("Rewarded ad recorded an impression.");
//        };
//        // Raised when a click is recorded for an ad.
//        ad.OnAdClicked += () =>
//        {
//            Debug.Log("Rewarded ad was clicked.");
//        };
//        // Raised when the ad opened full screen content.
//        ad.OnAdFullScreenContentOpened += () =>
//        {
//            Debug.Log("Rewarded ad full screen content opened.");
//        };
//        // Raised when the ad closed full screen content.
//        ad.OnAdFullScreenContentClosed += () =>
//        {
//            if (onRewardedVideoCompleteEvent != null)
//            {
//                onRewardedVideoCompleteEvent.Invoke();
//                onRewardedVideoCompleteEvent = null;
//            }
//            Debug.Log("Rewarded ad full screen content closed.");
//            LoadAdmobRewardedAd();
//        };
//        // Raised when the ad failed to open full screen content.
//        ad.OnAdFullScreenContentFailed += (AdError error) =>
//        {
//            Debug.LogError("Rewarded ad failed to open full screen content with error : "
//                + error);
//            LoadAdmobRewardedAd();
//        };

//    }
//    #endregion end admob rewarded ad
//    //#region unity interstitial Ad
//    //#endregion end unity interstitial ad
//    #region admob open app Ad
//    // App open ads can be preloaded for up to 4 hours.
//    private readonly TimeSpan TIMEOUT = TimeSpan.FromHours(4);
//    private DateTime _expireTime;
//    private AppOpenAd _appOpenAd;



//    private void OnDestroy()
//    {
//        // Always unlisten to events when complete.
//        //AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
//    }

//    /// <summary>
//    /// Loads the ad.
//    /// </summary>
//    public void LoadAppOpenAd()
//    {
//        // Clean up the old ad before loading a new one.
//        if (_appOpenAd != null)
//        {
//            DestroyAppOpenAd();
//        }

//        Debug.Log("Loading app open ad.");

//        // Create our request used to load the ad.
//        var adRequest = new AdRequest();

//        // Send the request to load the ad.
//        AppOpenAd.Load(admobAppOpenId, adRequest, (AppOpenAd ad, LoadAdError error) =>
//        {
//            // If the operation failed with a reason.
//            if (error != null)
//            {
//                Debug.LogError("App open ad failed to load an ad with error : "
//                                + error);
//                return;
//            }

//            // If the operation failed for unknown reasons.
//            // This is an unexpected error, please report this bug if it happens.
//            if (ad == null)
//            {
//                Debug.LogError("Unexpected error: App open ad load event fired with " +
//                               " null ad and null error.");
//                return;
//            }

//            // The operation completed successfully.
//            Debug.Log("App open ad loaded with response : " + ad.GetResponseInfo());
//            _appOpenAd = ad;

//            // App open ads can be preloaded for up to 4 hours.
//            _expireTime = DateTime.Now + TIMEOUT;

//            // Register to ad events to extend functionality.
//            RegisterEventHandlers(ad);


//        });


//    }

//    /// <summary>
//    /// Shows the ad.
//    /// </summary>
//    public void ShowAppOpenAd()
//    {
//        // App open ads can be preloaded for up to 4 hours.
//        if (_appOpenAd != null && _appOpenAd.CanShowAd())
//        {
//            Debug.Log("Showing app open ad.");
//            _appOpenAd.Show();
//        }
//        else
//        {
//            Debug.LogError("App open ad is not ready yet.");
//        }

//    }

//    /// <summary>
//    /// Destroys the ad.
//    /// </summary>
//    public void DestroyAppOpenAd()
//    {
//        if (_appOpenAd != null)
//        {
//            Debug.Log("Destroying app open ad.");
//            _appOpenAd.Destroy();
//            _appOpenAd = null;
//        }
//    }

//    private void OnAppStateChanged(AppState state)
//    {
//        Debug.Log("App State changed to : " + state);

//        // If the app is Foregrounded and the ad is available, show it.
//        if (state == AppState.Foreground)
//        {
//            ShowAppOpenAd();
//        }
//    }

//    private void RegisterEventHandlers(AppOpenAd ad)
//    {
//        // Raised when the ad is estimated to have earned money.
//        ad.OnAdPaid += (AdValue adValue) =>
//        {
//            Debug.Log(String.Format("App open ad paid {0} {1}.",
//                adValue.Value,
//                adValue.CurrencyCode));
//        };
//        // Raised when an impression is recorded for an ad.
//        ad.OnAdImpressionRecorded += () =>
//        {
//            Debug.Log("App open ad recorded an impression.");
//        };
//        // Raised when a click is recorded for an ad.
//        ad.OnAdClicked += () =>
//        {
//            Debug.Log("App open ad was clicked.");
//        };
//        // Raised when an ad opened full screen content.
//        ad.OnAdFullScreenContentOpened += () =>
//        {
//            Debug.Log("App open ad full screen content opened.");

//        };
//        // Raised when the ad closed full screen content.
//        ad.OnAdFullScreenContentClosed += () =>
//        {
//            Debug.Log("App open ad full screen content closed.");

//            // It may be useful to load a new ad when the current one is complete.
//            LoadAppOpenAd();
//        };
//        // Raised when the ad failed to open full screen content.
//        ad.OnAdFullScreenContentFailed += (AdError error) =>
//        {
//            Debug.LogError("App open ad failed to open full screen content with error : "
//                            + error);
//        };
//    }
//    #endregion end open app ad

//    #region unity ads 
//    public bool IsUnityInterstitialAvailable()
//    {
//        return IsUnityInterstitialAdReady(unityInterstitalId);
//    }

//    public bool IsUnityRewardedAvailable()
//    {
//        return IsUnityRewardedAdReady(unityRewardedId);
//    }
//    public void LoadUnityInterstitial()
//    {
//        if (SystemInfo.systemMemorySize >= 3072 && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
//           || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
//        {
//            Advertisement.Load(unityInterstitalId, this);
//        }
//    }
//    public void LoadUnityRewarded()
//    {
//        if (SystemInfo.systemMemorySize >= 3072 && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
//           || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
//        {
//            Advertisement.Load(unityRewardedId, this);
//        }
//    }
//    public void ShowUnityRewardedVideo()
//    {
//        if (IsUnityRewardedAvailable())
//        {
//            Advertisement.Show(unityRewardedId, this);
//        }
//        else
//        {
//            LoadUnityRewarded();
//        }
//    }

//    public void ShowUnityInterstitial()
//    {
//        if (SystemInfo.systemMemorySize >= 3072 && (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
//           || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork))
//        {
//            if (IsUnityInterstitialAvailable())
//            {
//                Advertisement.Show(unityInterstitalId, this);
//            }
//            else
//            {
//                LoadUnityInterstitial();
//            }
//        }
//    }

//    // If the ad successfully loads, add a listener to the button and enable it:
//    public void OnUnityAdsAdLoaded(string adUnitId)
//    {
//        Debug.Log("Ad Loaded: " + unityRewardedId);
//    }

//    public bool IsUnityRewardedAdReady(string id)
//    {

//        return unityRewardedId.Equals(id);
//    }
//    public bool IsUnityInterstitialAdReady(string id)
//    {

//        return unityInterstitalId.Equals(id);
//    }

//    // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
//    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
//    {
//        if (adUnitId.Equals(unityRewardedId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
//        {
//            Debug.Log("Unity Ads Rewarded Ad Completed");
//            // Grant a reward.

//            if (onRewardedVideoCompleteEvent != null)
//            {
//                onRewardedVideoCompleteEvent();
//            }
//            // Load another ad:
//            Advertisement.Load(unityRewardedId, this);
//        }

//        if (adUnitId.Equals(unityInterstitalId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
//        {
//            Debug.Log("Unity Ads Interstitial Ad Completed");
//            // Grant a reward.
//            if (onInterstitialAdCloseEvent != null)
//            {
//                onInterstitialAdCloseEvent();
//            }
//            // Load another ad:
//            Advertisement.Load(unityInterstitalId, this);
//        }
//    }

//    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
//    {
//        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
//        // Use the error details to determine whether to try to load another ad.
//    }

//    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
//    {
//        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
//        // Use the error details to determine whether to try to load another ad.
//        LoadUnityInterstitial();
//        LoadUnityRewarded();
//    }

//    public void OnUnityAdsShowStart(string adUnitId) { }
//    public void OnUnityAdsShowClick(string adUnitId) { }

//    public void OnInitializationComplete()
//    {
//        // throw new NotImplementedException();
//    }

//    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
//    {
//        //  throw new NotImplementedException();
//    }
//    #endregion
//}
