using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowInterstitialController : MonoBehaviour
{
    [SerializeField] private UnityEvent onFinished;
    public string placement;
    private float lastTimescale;

    public virtual void Show()
    {
        if (ShouldShowInterstitial)
        {
            bool interstitialReady = AdsMediationController.Instance.IsInterstitialAvailable;
            if (interstitialReady)
            {
                Debug.Log("show inter ads");
                //AudioHelper.Instance.MuteSound();
                //AudioHelper.Instance.MutePlaylist();
                lastTimescale = Time.timeScale;
                Time.timeScale = 0;
                //GameData.showAds = true;
                //GameManager.Instance.PlayerDataManager.onCloseInterAds = () => InterClosed(true);
                // AdsFakeManager.Instance.ShowInter();
                AdsMediationController.Instance.ShowInterstitial(placement, InterClosed);
                //GameAnalytics.LogInterstitialAdsImpression(scenariosInter, true, true);
            }
            else
            {
                //GameAnalytics.LogInterstitialAdsImpression(scenariosInter, false, true);
                CannotShow();
            }
        }
        else
        {
            //GameAnalytics.LogInterstitialAdsImpression(scenariosInter, AdsMediationController.Instance.IsInterstitialAvailable, false);
            CannotShow();
        }
    }

    private bool ShouldShowInterstitial
    {
        get
        {
            
            DateTime lastShown = SDKPlayPrefs.GetDateTime(StringConstants.PREF_INTERSTITIAL_LAST_SHOWN, DateTime.Now.Subtract(TimeSpan.FromDays(1)));
            long intervalSeconds = 20; //RemoteConfigManager.GetLong(StringConstants.RC_INTERSTITIAL_INTERVAL_SECONDS);
            //long startGameSeconds = RemoteConfigManager.GetLong(StringConstants.RC_INTERSTITIAL_FROM_STARTUP_SECONDS);
            if ((DateTime.Now - lastShown).TotalSeconds > intervalSeconds && !GameManager.Instance.PlayerDataManager.IsNoAds())
            {
                return true;
            }
            return false;
        }
    }

    private void CannotShow()
    {
        //AudioHelper.Instance.UnmuteSound();
        //AudioHelper.Instance.UnmutePlaylist();
        //GameAnalytics.LogInterstitialAdsImpressionDone(scenariosInter, false, false);
        onFinished?.Invoke();
    }

    private void InterClosed(bool success)
    {
        //AudioHelper.Instance.UnmuteSound();
        //AudioHelper.Instance.UnmutePlaylist();
        //GameAnalytics.LogInterstitialAdsImpressionDone(scenariosInter, true, success);
        SDKPlayPrefs.SetDateTime(StringConstants.PREF_INTERSTITIAL_LAST_SHOWN, DateTime.Now);
        Time.timeScale = lastTimescale;
        onFinished?.Invoke();
        //Invoke("MarkShowAdsFalseDelay", 0.05f);
    }

    //private void MarkShowAdsFalseDelay()
    //{
    //    //GameData.showAds = false;
    //}
}
