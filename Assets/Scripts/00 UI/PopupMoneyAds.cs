using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupMoneyAds : UICanvas
{
    [SerializeField] private Text txtMoney;
    [SerializeField] private Transform content;
    private int _amount;
    private GameObject valiObj;

    internal void Init(int amount, GameObject obj)
    {
        content.DOScale(1, 0.4f).SetEase(Ease.OutBack);
        txtMoney.text = "+" + amount.ToString();
        _amount = amount;
        valiObj = obj;
    }

    public override void OnBackPressed()
    {
        base.OnBackPressed();
        SoundManager.Instance.PlaySoundButton();
    }

    private void OnDisable()
    {
        content.localScale = Vector3.zero;
        content.DOKill();
    }

    public void ShowAds()
    {
        GetComponent<ShowRewardedController>().Show();
        OnBackPressed();
    }

    public void GetReward(bool success)
    {
        if (success)
        {
            GameManager.Instance.Profile.AddGold(_amount, "money_ads_item");
            GameManager.Instance.CurrentLevelManager.DeleteObject(valiObj);
            SoundManager.Instance.PlaySoundReward();
        }
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
