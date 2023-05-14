#if MEDIATION_APP_OPEN_AD
//using com.adjust.sdk;
using GoogleMobileAds.Api;
using System;
using UnityEngine;
public class AppOpenAdManager {
    private static AppOpenAdManager instance;

    private AppOpenAd ad;

    private bool isShowingAd = false;
    private Action onAdFinishAction;
    private DateTime loadTime;
    private bool lastLoadFailed;
    private ScreenOrientation currentScreenOrientation = ScreenOrientation.Portrait;
    private int currentIdIndex;
    private int continuousNoFill;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private string AdUnitId
    {
        get
        {
            string[] waterFall = WaterFall;
#if UNITY_ANDROID
            string standaloneId = Studio1BAds.Config.Studio1BAppSettingsLoader.AppSettings.AndroidAdmobAppOpenId;
#else
            string standaloneId = Studio1BAds.Config.Studio1BAppSettingsLoader.AppSettings.IOSAdmobAppOpenId;
#endif
            if (waterFall != null && waterFall.Length > 0)
            {
                if (currentIdIndex >= waterFall.Length)
                {
                    currentIdIndex = 0;
                }
                return waterFall[currentIdIndex];
            }
            else
            {
                return standaloneId;
            }
        }
    }

    private string[] WaterFall
    {
        get
        {
#if UNITY_ANDROID
            return Studio1BAds.Config.Studio1BAppSettingsLoader.AppSettings.AndroidAdmobAppOpenWaterFall;
#else
            return Studio1BAds.Config.Studio1BAppSettingsLoader.AppSettings.IOSAdmobAppOpenWaterFall;
#endif

        }
    }

    public void ShowAdIfAvailable(Action finishAction = null)
    {
        if (!IsAdAvailable || isShowingAd || AdsMediationController.IsFullscreenAdsShowing || Math.Abs(DateTimeOffset.Now.ToUnixTimeMilliseconds() - AdsMediationController.LastFullscreenAdsShown) < 2000)
        {
            LoadAdIfExpiredOrFailed();
            finishAction?.Invoke();
            return;
        }
        this.onAdFinishAction = finishAction;
        ad.OnAdFullScreenContentClosed += HandleAdDidDismissFullScreenContent;
        ad.OnAdFullScreenContentFailed += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdFullScreenContentOpened += HandleAdDidPresentFullScreenContent;
        ad.OnAdImpressionRecorded += HandleAdDidRecordImpression;
        //ad.OnPaidEvent += HandlePaidEvent;
        ad.Show();
        Debug.Log("Show AOA");
    }
    private void HandleAdDidDismissFullScreenContent()
    {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        LoadAd();
        AdsMediationController.Instance.Enqueue(InvokeOnAdFinished);
    }

    private void HandleAdFailedToPresentFullScreenContent(AdError error)
    {
        Debug.LogFormat("Failed to present the ad (reason: {0})", error.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        AdsMediationController.Instance.Enqueue(InvokeOnAdFinished);
        LoadAd();
    }

    private void InvokeOnAdFinished()
    {
        onAdFinishAction?.Invoke();
    }

    private void HandleAdDidPresentFullScreenContent()
    {
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression()
    {

    }

    //private void HandlePaidEvent(object sender, AdValueEventArgs args)
    //{
    //    AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAdMob);
    //    float revenue = args.AdValue.Value / 1000000.0f;
    //    adjustAdRevenue.setRevenue(revenue, args.AdValue.CurrencyCode);
    //    Adjust.trackAdRevenue(adjustAdRevenue);
    //    FirebaseAdsRevenueLogger.AddAppOpenRevenue(revenue);
    //}

    public bool IsAdAvailable
    {
        get
        {
            return ad != null && !IsAdExpired;
        }
    }

    private bool IsAdExpired
    {
        get
        {
            return (DateTime.Now - loadTime).TotalHours > 4;
        }
    }

    private void LoadAdIfExpiredOrFailed()
    {
        if ((ad != null && IsAdExpired) || (ad == null && lastLoadFailed))
        {
            LoadAd();
        }
    }

    public void LoadAd(ScreenOrientation orientation)
    {
        currentScreenOrientation = orientation;
        LoadAd();
    }

    private void LoadAd()
    {
        Debug.Log("Load AOA");
        AdRequest request = new AdRequest.Builder().Build();
        lastLoadFailed = false;
        // Load an app open ad for portrait orientation
        AppOpenAd.Load(AdUnitId, currentScreenOrientation, request, (appOpenAd, error) => {
            if (error != null)
            {
                // Handle the error.         
                Debug.LogFormat("Failed to load the ad. (reason: {0})", error.GetMessage());
                lastLoadFailed = true;
                if (error.GetCode() != 2)
                {
                    continuousNoFill++;
                    currentIdIndex++;
                    string[] waterFall = WaterFall;
                    if (waterFall != null && continuousNoFill < waterFall.Length)
                    {
                        LoadAd();
                    }
                }
                return;
            }
            continuousNoFill = 0;
            // App open ad is loaded.
            ad = appOpenAd;
            loadTime = DateTime.Now;
        });
    }
}
#endif