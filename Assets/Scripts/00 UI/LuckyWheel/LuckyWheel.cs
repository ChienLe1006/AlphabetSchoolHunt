using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LuckyWheel : UICanvas
{
    public Transform wheel;
    [SerializeField] private Button btnFreeSpin;
    [SerializeField] private Button btnVideoSpin;

    private int slotWheel;
    private Vector3 angleTarget;

    private const int NUMBER_ROTATE_AROUND = 5;
    private const int NUMBER_PIECE_OF_WHEEL = 8;
    private const float TIME_ROTATE = 3f;


    [SerializeField] private List<RewardLuckyWheel> listItemRewards;
    [SerializeField] private List<ItemFreeReward> listItemFreeSpin;
    [SerializeField] private Text txtNumberDaily;

    [SerializeField] private RectTransform rectPointNumberWatchVideoSpin;
    [SerializeField] private Text txtNumberWatchVideo;
    [SerializeField] private Image imgBarProcessWatchVideo;
    [SerializeField] private GameObject objClockFreeSpin;
    [SerializeField] private Text txtClockFreeSpin;
    [SerializeField] private GameObject mask;

    private float WidthBar = 700;
    private float totalTimeReset;
    private bool isCoutdown;
    private float _time;

    private void Start()
    {
        btnFreeSpin.onClick.AddListener(OnClickBtnFreeSpin);
        btnVideoSpin.onClick.AddListener(OnClickBtnVideoSpin);

    }

    private void Update()
    {
        if (!isShow && !isCoutdown)
        {
            return;
        }

        _time += Time.unscaledDeltaTime;
        if (_time >= 1)
        {            
            totalTimeReset -= 1;

            if (totalTimeReset <= 0)
            {
                totalTimeReset = 0;
                isCoutdown = false;
                btnFreeSpin.interactable = true;
                objClockFreeSpin.SetActive(false);
            }           
            _time = 0;
        }
        var timeSpan = TimeSpan.FromSeconds(totalTimeReset);
        txtClockFreeSpin.text = Helper.FormatTimeIgnorSecond(timeSpan.Hours, timeSpan.Minutes, true);
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (isShow)
            Init();
    }

    private void Init()
    {
        mask.SetActive(false);
        isCoutdown = false;
        for (int i = 0; i < GameManager.Instance.PlayerDataManager.DataLuckyWheel.ListDataRewrds.Count; i++)
        {
            listItemRewards[i].Init(GameManager.Instance.PlayerDataManager.DataLuckyWheel.ListDataRewrds[i]);
        }

        btnFreeSpin.interactable = GameManager.Instance.PlayerDataManager.GetFreeSpin();

        SetupFreeRewards();
        SetupTimeWaitFreeSpin();

        var timeLogin = GameManager.Instance.PlayerDataManager.GetTimeLoginSpinVideo();
        var isNewDay = Helper.CheckNewDay(timeLogin, false);
        if (isNewDay)
        {
            GameManager.Instance.PlayerDataManager.SetNumberWatchDailyVideo(GameManager.Instance.PlayerDataManager.DataLuckyWheel.NumberSpinDaily);
            GameManager.Instance.PlayerDataManager.SetTimeLoginSpinVideo(DateTime.Now.ToString());
        }

        SetLayoutBtnVideoSpin();
    }

    private void SetupTimeWaitFreeSpin()
    {
        if (!GameManager.Instance.PlayerDataManager.GetFreeSpin())
        {
            string time = GameManager.Instance.PlayerDataManager.GetTimeLoginSpinFreeWheel();
            DateTime timeLogin = DateTime.Parse(time);

            long tickTimeNow = DateTime.Now.Ticks;
            long tickTimeOld = timeLogin.Ticks;

            long elapsedTicks = tickTimeNow - tickTimeOld;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);

            float totalSeconds = (float)elapsedSpan.TotalSeconds;

            totalTimeReset = 7 * 60 * 60 - totalSeconds;
            if (totalTimeReset <= 0)
            {
                totalTimeReset = 0;
                objClockFreeSpin.SetActive(false);
                btnVideoSpin.interactable = true;
            }
            else
            {
                isCoutdown = true;
                var timeSpan = TimeSpan.FromSeconds(totalTimeReset);
                txtClockFreeSpin.text = Helper.FormatTimeIgnorSecond(timeSpan.Hours, timeSpan.Minutes, true);

                objClockFreeSpin.SetActive(true);
            }

        }
    }

    private void SetLayoutBtnVideoSpin()
    {
        int numberWatchDailyVideo = GameManager.Instance.PlayerDataManager.GetNumberWatchDailyVideo();
        txtNumberDaily.text = numberWatchDailyVideo.ToString();

        btnVideoSpin.interactable = numberWatchDailyVideo > 0 ? true : false;
    }

    private void SetupFreeRewards()
    {
        for (int i = 0; i < listItemFreeSpin.Count; i++)
        {
            var reward = GameManager.Instance.PlayerDataManager.DataLuckyWheel.ListDataReceiveFrees[i];
            listItemFreeSpin[i].Init(reward);
        }

        int numberWatchVideo = GameManager.Instance.PlayerDataManager.GetNumberWatchVideoSpin();
        txtNumberWatchVideo.text = numberWatchVideo.ToString();


        var ratio = (float)numberWatchVideo / 20f;
        ratio = ratio > 1 ? 1 : ratio;
        rectPointNumberWatchVideoSpin.anchoredPosition = new Vector2(ratio * WidthBar, 55);
        imgBarProcessWatchVideo.fillAmount = ratio;


    }

    private void StartRotateWheel(bool isFree = true)
    {
        mask.SetActive(true);
        SoundManager.Instance.PlaySoundSpin();

        btnFreeSpin.interactable = false;
        btnVideoSpin.interactable = false;

        slotWheel = GameManager.Instance.PlayerDataManager.DataLuckyWheel.GetIdLuckyWheel();

        angleTarget = CalculateAngle(NUMBER_PIECE_OF_WHEEL - slotWheel);

        wheel.DORotate(angleTarget, TIME_ROTATE)
               .SetEase(Ease.InOutCubic)
               .OnComplete(OnCompleteRotate);

        if (!isFree)
        {
            int numberWatchDailyVideo = GameManager.Instance.PlayerDataManager.GetNumberWatchDailyVideo();
            numberWatchDailyVideo--;
            if (numberWatchDailyVideo <= 0)
                numberWatchDailyVideo = 0;

            GameManager.Instance.PlayerDataManager.SetNumberWatchDailyVideo(numberWatchDailyVideo);
        }

        int numberWatchVideo = GameManager.Instance.PlayerDataManager.GetNumberWatchVideoSpin();
        numberWatchVideo++;
        GameManager.Instance.PlayerDataManager.SetNumberWatchVideoSpin(numberWatchVideo);
    }

    private Vector3 CalculateAngle(int indexPrize)
    {
        return new Vector3(0, 0, -360 * NUMBER_ROTATE_AROUND - (indexPrize) * (360 / NUMBER_PIECE_OF_WHEEL));
    }

    private void OnCompleteRotate()
    {
        // complete wheel
        
        mask.SetActive(false);
        var playerData = PlayerDataManager.Instance;
        var dataReward = playerData.DataLuckyWheel.ListDataRewrds[slotWheel];
 
        switch (dataReward.Type)
        {
            case TypeGift.GOLD:
                {
                    GameManager.Instance.Profile.AddGold(dataReward.Amount, "lucky_wheel");

                    SoundManager.Instance.PlaySoundReward();
                }
                break;

            case TypeGift.ELEMENT:
                {
                    if (playerData.GetUnlockElement(dataReward.IdType))
                    {
                        GameManager.Instance.Profile.AddGold(dataReward.NumberCoinReplace, "lucky_wheel");
                        SoundManager.Instance.PlaySoundReward();
                    }
                    else
                    {
                        RewardEndGame reward = new RewardEndGame();
                        reward.Type = TypeEquipment.ELEMENT;
                        reward.Id = dataReward.IdType;
                        //reward.skill = (Skill)dataReward.IdType;
                        GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);
                        playerData.SetUnlockElement(dataReward.IdType);
                        playerData.SetIdEquipElement(dataReward.IdType);
                    }
                }   
                break;
            default:
                break;
        }

        Init();
    }

    private void OnClickBtnFreeSpin()
    {
        SoundManager.Instance.PlaySoundButton();

        GameManager.Instance.PlayerDataManager.SetFreeSpin(false);
        GameManager.Instance.PlayerDataManager.SetTimeLoginSpinFreeWheel(DateTime.Now.ToString());
        StartRotateWheel();

        totalTimeReset = 7 * 60 * 60;
        isCoutdown = true;
        objClockFreeSpin.SetActive(true);
#if UNITY_ANDROID
        NotificationManager.Instance.SendNotiLuckyWheel();
#endif
    }

    private void OnClickBtnVideoSpin()
    {
        SoundManager.Instance.PlaySoundButton();
        GetComponent<ShowRewardedController>().Show();
    }

    public void GetReward(bool success)
    {
        if (success)
        {
            OnRewardVideo(1);
        }
    }

    private void OnRewardVideo(int x)
    {
        if (x <= 0 && !isShow)
            return;

        StartRotateWheel(false);
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
