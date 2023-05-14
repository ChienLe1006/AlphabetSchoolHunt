using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiMainLobby : UICanvas
{
    [Title("Button")]
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnLuckySpin;
    [SerializeField] private Button btnDaily;
    [SerializeField] private Button btnShop;
    [SerializeField] private Button btnOnline;
    [SerializeField] private Button btnSetting;

    [Title("Others")]
    [SerializeField] private List<Image> listImgKeys;
    [SerializeField] private Text txtCurentLevel;
    [SerializeField] private Text onlineTimeTxt;
    [SerializeField] private Text waitTxt;
    [SerializeField] private UIOnlineGift UIOnlineGift;
    [SerializeField] private GameObject onlineGiftNoti;
    [SerializeField] private Transform leftSide, rightSide;

    private float onlineTimeReset;
    private float _time;
    private bool isCountdown;
    private bool canGetReward;
    private int[] onlineMarks = new int[6] { 60, 120, 120, 120, 180, 0 };  

    // Start is called before the first frame update
    void Start()
    {
        btnPlay.onClick.AddListener(OnClickBtnPlay);
        btnLuckySpin.onClick.AddListener(OnClickBtnSpin);
        btnDaily.onClick.AddListener(OnClickBtnDailyReward);
        btnShop.onClick.AddListener(OnClickShop);
        btnOnline.onClick.AddListener(OnClickGetReward);
        btnSetting.onClick.AddListener(OnClickBtnSetting);

        if (PlayerDataManager.Instance.FirstOnlineGiftInDay())
        {
            GameManager.Instance.PlayerDataManager.SetIsGetOnlineGiftFirstTime(true);
            GameManager.Instance.PlayerDataManager.SetTimeOnlineGift(DateTime.Now.ToString());
            PlayerDataManager.Instance.SetFirstOnlineGiftInDay(false);
        }
        Init();      
        SetupOnlineGiftTime();
    }

    private void Update()
    {
        if (!isShow && !isCountdown || (PlayerDataManager.Instance.OnlineTimeMark > 5))
        {
            btnOnline.interactable = false;
            onlineGiftNoti.SetActive(false);
            return;
        }

        _time += Time.unscaledDeltaTime;
        if (_time >= 1)
        {

            onlineTimeReset -= 1;
            if (onlineTimeReset <= 0)
            {
                onlineTimeReset = 0;
                isCountdown = false;
                canGetReward = true;
                onlineTimeTxt.gameObject.SetActive(false);
                onlineGiftNoti.SetActive(true);
                UIOnlineGift.ShowGiftPanel();
            }
            else
            {
                canGetReward = false;
                onlineGiftNoti.SetActive(false);
            }                     
            _time = 0;
        }
        var timeSpan = TimeSpan.FromSeconds(onlineTimeReset);
        onlineTimeTxt.text = Helper.FormatTime(timeSpan.Minutes, timeSpan.Seconds, true);
        waitTxt.text = onlineTimeTxt.text;
    }

    private void Init()
    {                     
        txtCurentLevel.text = string.Format("LEVEL {0}", GameManager.Instance.PlayerDataManager.GetDataLevel().DisplayLevel);
    }

    private void SetupOnlineGiftTime()
    {
        string time = GameManager.Instance.PlayerDataManager.GetTimeOnlineGift();
        bool newDay = Helper.CheckNewDay(time, false);
        if (newDay)
        {
            PlayerDataManager.Instance.OnlineTimeMark = 1;
            onlineTimeReset = onlineMarks[0];
        }
        SetUpWaitTime();
    }

    private void OnApplicationQuit()
    {
        if (isShow && isCountdown || (PlayerDataManager.Instance.OnlineTimeMark <= 5))
        {
            PlayerDataManager.Instance.OnlineGiftTime = onlineTimeReset;
        }             
    }

    private void SetUpWaitTime()
    {
        if (GameManager.Instance.PlayerDataManager.GetIsGetOnlineGiftFirstTime() && PlayerDataManager.Instance.OnlineTimeMark < 6)
        {
            //string time = GameManager.Instance.PlayerDataManager.GetTimeOnlineGift();
            //DateTime timeGetReward = DateTime.Parse(time);

            //long tickTimeNow = DateTime.Now.Ticks;
            //long tickTimeOld = timeGetReward.Ticks;

            //long elapsedTicks = tickTimeNow - tickTimeOld;
            //TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            //float totalSeconds = (float)elapsedSpan.TotalSeconds;

            onlineTimeReset = PlayerDataManager.Instance.OnlineGiftTime; //onlineMarks[PlayerDataManager.Instance.OnlineTimeMark - 1] - totalSeconds;
  
            if (onlineTimeReset <= 0)
            {
                onlineTimeReset = 0;
                onlineTimeTxt.gameObject.SetActive(false);
            }
            else
            {
                isCountdown = true;
                var timeSpan = TimeSpan.FromSeconds(onlineTimeReset);
                onlineTimeTxt.text = Helper.FormatTime(timeSpan.Minutes, timeSpan.Seconds, true);
                onlineTimeTxt.gameObject.SetActive(true);
            }
        }
    }

    private void OnClickGetReward()
    {
        UIOnlineGift.Show(true);
        if (canGetReward)
        {            
            UIOnlineGift.Init(false);
        }
        else
        {           
            UIOnlineGift.Init(true);
        }
    }

    public void OnlineGiftClick()
    {
        PlayerDataManager.Instance.SetTimeOnlineGift(DateTime.Now.ToString());
        PlayerDataManager.Instance.OnlineTimeMark++;
        isCountdown = true;
        onlineTimeReset = onlineMarks[PlayerDataManager.Instance.OnlineTimeMark - 1];
        onlineTimeTxt.gameObject.SetActive(true);

        if (PlayerDataManager.Instance.OnlineTimeMark == 6)
        {
            onlineTimeTxt.gameObject.SetActive(false);
            btnOnline.interactable = false;
        }
    }

    public void SetLayoutKey()
    {
        for (int i = 0; i < listImgKeys.Count; i++)
        {
            listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(false);
        }

        int countKey = GameManager.Instance.Profile.GetKey() > 3 ? 3 : GameManager.Instance.Profile.GetKey();
        for (int i = 0; i < countKey; i++)
        {
            listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(true);
        }
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (IsShow)
        {
            Init();
        }
    }


    private void OnClickBtnPlay()
    {
        GameManager.Instance.StartCurrentLevel();
        ShowAniHide();
        SoundManager.Instance.PlaySoundButton();
        if (PlayerDataManager.Instance.FirstTimePlay())
        {
            PlayerDataManager.Instance.SetFirstTimePlay(false);
        }
        GameManager.Instance.CurrentLevelManager.DecorNPC.SetActive(false);
    }

    private void OnClickBtnImposter()
    {
        GameManager.Instance.StartCurrentLevel();
        ShowAniHide();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnSpin()
    {
        GameManager.Instance.UiController.OpenLuckyWheel();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickShop()
    {
        GameManager.Instance.UiController.OpenShop();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnDailyReward()
    {
        GameManager.Instance.UiController.OpenDailyReward();

        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnNoAds()
    {
        //GameManager.Instance.IapController.PurchaseProduct((int)IdPack.NO_ADS_BASIC);
        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnSetting()
    {
        SoundManager.Instance.PlaySoundButton();
    }

    public void ShowAniHide()
    {
        leftSide.DOLocalMoveX(-540, 0.5f).SetEase(Ease.InBack);
        rightSide.DOLocalMoveX(540, 0.5f).SetEase(Ease.InBack);
        btnPlay.gameObject.SetActive(false);
        txtCurentLevel.transform.parent.gameObject.SetActive(false);
    }

    public void ActiveMainLobby()
    {
        Show(true);
        leftSide.DOLocalMoveX(-320, 0.5f).SetEase(Ease.OutBack);
        rightSide.DOLocalMoveX(320, 0.5f).SetEase(Ease.OutBack);
        btnPlay.gameObject.SetActive(true);
        txtCurentLevel.transform.parent.gameObject.SetActive(true);
    }
}
