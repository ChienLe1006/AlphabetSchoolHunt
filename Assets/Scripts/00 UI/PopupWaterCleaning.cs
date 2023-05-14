using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWaterCleaning : UICanvas
{
    [SerializeField] private Text txtMoney;
    [SerializeField] private Transform content;   
    private int _amount;

    [SerializeField] private GameObject broomAuto, broomTuts, txtTuts;
    [SerializeField] private Transform waterImg, money;
    [SerializeField] private Button autoCleanBtn;
    private Vector3 txtMoneyTrans;
    public bool AutoClean { get; private set; }
    private GameObject puddleObj;

    protected override void Awake()
    {
        txtMoneyTrans = money.localPosition;
    }

    internal void Init(int amount, GameObject obj)
    {
        content.DOScale(1, 0.4f).SetEase(Ease.OutBack);
        txtMoney.text = "+" + amount.ToString();
        _amount = amount;
        puddleObj = obj;

        Setup();
        GetComponentInChildren<DragToClean>().Setup();        
    }

    private void Setup()
    {
        autoCleanBtn.interactable = true;
        txtTuts.gameObject.SetActive(true);
        broomTuts.SetActive(true);
        broomAuto.SetActive(false);
        AutoClean = false;
        money.gameObject.SetActive(false);
        money.localPosition = txtMoneyTrans;
    }

    internal void TurnOffTuts()
    {
        broomTuts.SetActive(false);
        txtTuts.SetActive(false);
    }

    private void OnDisable()
    {
        content.localScale = Vector3.zero;
        content.DOKill();
    }

    public override void OnBackPressed()
    {
        base.OnBackPressed();
        waterImg.DOKill();
    }

    public void ShowAds()
    {
        GetComponent<ShowRewardedController>().Show();
    }

    public void GetReward(bool success)
    {
        if (success)
        {
            TurnOffTuts();
            AutoClean = true;
            autoCleanBtn.interactable = false;
            broomAuto.SetActive(true);
            waterImg.DOScale(0, 4).OnComplete(() =>
            {
                money.gameObject.SetActive(true);
                money.DOLocalMoveY(money.localPosition.y + 200, 1).OnComplete(() =>
                {
                    DoneClean();
                });
            });           
        }
    }

    internal void DoneClean()
    {
        GameManager.Instance.Profile.AddGold(_amount);
        SoundManager.Instance.PlaySoundReward();
        GameManager.Instance.CurrentLevelManager.DeleteObject(puddleObj);
        OnBackPressed();
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
