using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShowRewardedController : MonoBehaviour
{
    public static bool isWatchingRewarded;
    public string placement;

    [SerializeField]
    private UnityEvent onRewardedAdsNotAvailable;
    [SerializeField]
    private UnityEvent onRewardedAdsStart;
    [SerializeField]
    private UnityEvent onRewardedAdsFailedToShow;
    [SerializeField]
    private OnRewardedClosedEvent onClosed;

    private float lastTimescale;

    public void Show()
    {
        //GameAnalytics.LogButtonClick(scenarios, placement);
        bool canShowAds = Application.internetReachability != NetworkReachability.NotReachable &&
                          AdsMediationController.Instance.IsRewardedAvailable;
        if (canShowAds)
        {
            Debug.Log("show reward ads");
            //AudioHelper.Instance.MuteSound();
            //AudioHelper.Instance.MutePlaylist();
            isWatchingRewarded = true;
            lastTimescale = Time.timeScale;
            Time.timeScale = 0;
            //GameData.showAds = true;
            SDKPlayPrefs.SetDateTime(StringConstants.PREF_INTERSTITIAL_LAST_SHOWN, DateTime.Now);
            // GameManager.Instance.PlayerDataManager.onCloseRewardedAds = () => OnRewardedAdsClosed(true);
            // AdsFakeManager.Instance.ShowReward();
            AdsMediationController.Instance.ShowRewardedAds(placement, OnRewardedAdsFailedToShow, OnRewardedAdsClosed);
            //GameAnalytics.LogWatchRewardAds(scenarios, true);
            onRewardedAdsStart?.Invoke();
        }
        else
        {
            //GameAnalytics.LogWatchRewardAds(scenarios, false);
            onRewardedAdsNotAvailable?.Invoke();
        }
    }

    private void OnRewardedAdsClosed(bool success)
    {
        //GameAnalytics.LogWatchRewardAdsDone(scenarios, true, success.ToString());
        //AudioHelper.Instance.UnmuteSound();
        //AudioHelper.Instance.UnmutePlaylist();
        Time.timeScale = lastTimescale;
        isWatchingRewarded = false;
        onClosed?.Invoke(success);
        //Invoke("MarkShowAdsFalseDelay", 0.05f);
    }


    private void OnRewardedAdsFailedToShow()
    {
        //GameAnalytics.LogWatchRewardAdsDone(scenarios, true, "can't_show");
        //AudioHelper.Instance.UnmuteSound();
        //AudioHelper.Instance.UnmutePlaylist();
        Time.timeScale = lastTimescale;
        isWatchingRewarded = false;
        onRewardedAdsFailedToShow?.Invoke();
        //Invoke("MarkShowAdsFalseDelay", 0.05f);
    }

    //private void MarkShowAdsFalseDelay()
    //{
    //    //GameData.showAds = false;
    //}
}

[Serializable]
public class OnRewardedClosedEvent : UnityEvent<bool>
{

}
