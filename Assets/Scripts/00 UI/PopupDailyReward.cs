using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDailyReward : UICanvas
{
    [SerializeField] private Button btnClaim, btnX2;
    [SerializeField] private Button close;
    private int dayIndex;

    private void Start()
    {
        close.onClick.AddListener(OnBackPressed);
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (isShow)
            Init();
    }

    private void Init()
    {
        dayIndex = PlayerDataManager.Instance.CurrentDailyRewardDayIndex;
        if (dayIndex != PlayerDataManager.Instance.LastDailyRewardDayIndex)
        {
            btnClaim.gameObject.SetActive(true);
            btnX2.gameObject.SetActive(true);
            close.gameObject.SetActive(false);
        }
        else
        {
            btnClaim.gameObject.SetActive(false);
            btnX2.gameObject.SetActive(false);
            close.gameObject.SetActive(true);
        }
    }

    public void Claim(bool x2)
    {
        var rewardData = PlayerDataManager.Instance.DataDailyReward.dailyDatas[dayIndex];
        switch (rewardData.Type)
        {
            case TypeGift.GOLD:
                {
                    GameManager.Instance.Profile.AddGold(x2 ? rewardData.Amount * 2 : rewardData.Amount, "daily_reward");                  
                }
                break;
            default:
                break;
        }
        OnBackPressed();
        GameManager.Instance.UiController.OpenPopupRewardDaily(rewardData, x2);

        PlayerDataManager.Instance.LastDailyRewardDayIndex = dayIndex;
        PlayerDataManager.Instance.LastDailyRewardClaim = DateTime.Now;
    }

    public void GetReward(bool success)
    {
        if (success)
        {
            Claim(true);
        }
    }

    public override void OnBackPressed()
    {
        base.OnBackPressed();
        SoundManager.Instance.PlaySoundButton();
    }

    public void NoInternet()
    {
        PopupDialogCanvas.Instance.Show("No Internet!");
    }

    public void ShowFail()
    {
        PopupDialogCanvas.Instance.Show("No Video!");
    }
}
