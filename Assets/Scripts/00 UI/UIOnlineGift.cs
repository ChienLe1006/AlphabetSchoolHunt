using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;
using UnityEngine.UI;

public class UIOnlineGift : UICanvas
{
    [SerializeField] private GameObject waitPanel;
    [SerializeField] private GameObject giftPanel;
    [SerializeField] private Button close, claim, claimx2;
    [SerializeField] private Image giftImg;
    [SerializeField] private Text quantity;
    [SerializeField] private Text claimTxt;

    [SerializeField] private UiMainLobby UiMainLobby;

    private void Start()
    {
        close.onClick.AddListener(OnBackPressed);
        claim.onClick.AddListener(Claim);
        claimx2.onClick.AddListener(ClaimX2);
    }

    public void Init(bool isWait)
    {
        if (isWait)
        {
            waitPanel.SetActive(true);
            giftPanel.SetActive(false);
        }
        else
        {
            giftPanel.SetActive(true);
            waitPanel.SetActive(false);
            SetUpGift();
        }
    }

    private void SetUpGift()
    {
        var giftData = PlayerDataManager.Instance.DataOnlineGift;
        giftImg.sprite = giftData.rewardOnlineDatas[PlayerDataManager.Instance.OnlineTimeMark - 1].sprite;
        giftImg.SetNativeSize();
        quantity.text = $"{giftData.rewardOnlineDatas[PlayerDataManager.Instance.OnlineTimeMark - 1].Amount}";
        claimTxt.text = $"Collect {giftData.rewardOnlineDatas[PlayerDataManager.Instance.OnlineTimeMark - 1].Amount}";
    }

    private void Claim()
    {
        UiMainLobby.OnlineGiftClick();
        OnBackPressed();
        GameManager.Instance.Profile.AddGold(PlayerDataManager.Instance.DataOnlineGift.rewardOnlineDatas[PlayerDataManager.Instance.OnlineTimeMark - 2].Amount, "daily_reward");
    }

    private void ClaimX2()
    {
        GetComponent<ShowRewardedController>().Show();       
    }

    public void GetReward(bool success)
    {
        if (success)
        {
            if (!isShow) return;
            UiMainLobby.OnlineGiftClick();
            OnBackPressed();
            GameManager.Instance.Profile.AddGold(PlayerDataManager.Instance.DataOnlineGift.rewardOnlineDatas[PlayerDataManager.Instance.OnlineTimeMark - 2].Amount * 2, "daily_reward");
        }
    }

    public void ShowGiftPanel()
    {
        waitPanel.SetActive(false);
        giftPanel.SetActive(true);
        SetUpGift();
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
