using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsFakeManager : Singleton<AdsFakeManager>
{
    [SerializeField] private FakeAds fakeAds;

    public void ShowReward()
    {
        //UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
        fakeAds.ShowAdsRewarded();        
    }

    public void ShowInter()
    {
        //UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
        fakeAds.ShowAdsInter();
    }

    public void Close()
    {
        //UltimateJoystick.EnableJoystick(Constants.MAIN_JOINSTICK);
        fakeAds.gameObject.SetActive(false);

        GameManager.Instance.PlayerDataManager.onCloseRewardedAds?.Invoke();
        GameManager.Instance.PlayerDataManager.onCloseRewardedAds = null;

        GameManager.Instance.PlayerDataManager.onCloseInterAds?.Invoke();
        GameManager.Instance.PlayerDataManager.onCloseInterAds = null;
    }
}
