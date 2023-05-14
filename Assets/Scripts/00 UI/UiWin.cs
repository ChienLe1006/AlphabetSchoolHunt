using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiWin : UICanvas
{
    [SerializeField] private Button btnNoThank;
    [SerializeField] private Button btnX3Coin;
    [SerializeField] private Image iconReward;
    [SerializeField] private Image imgProcessReward;
    [SerializeField] private Text txtProcessReward;
    [SerializeField] private Text txtCoin;
    [SerializeField] private Text txtCoinReward;
    [SerializeField] private Text txtCoinAds;
    [SerializeField] private GameObject objFx;
    [SerializeField] private RectTransform arrow;
    private int percentReward;
    private int _gold;
    private int factor;

    private void Start()
    {
        btnNoThank.onClick.AddListener(OnClickBtnNoThank);
        btnX3Coin.onClick.AddListener(OnClickBtnX3Coin);
    }

    private void Update()
    {
        float x = arrow.anchoredPosition.x;
        if (x < 350 && x > 135)
        {
            factor = 2;
        }
        else if (x < 135 && x > -135)
        {
            factor = 3;
        }
        else if (x < -135 && x > -350)
        {
            factor = 5;
        }
        txtCoinAds.text = $"{_gold * factor}";
    }

    public void Init(int gold)
    {
        _gold = gold;
        objFx.SetActive(false);
        
        txtCoin.text = string.Format("Collect {0}", gold);
        btnX3Coin.gameObject.SetActive(true);
        SetupRewardEndGame();

        if (GameManager.Instance.CurrentGameMode == GameMode.HIDE)
        {
            SoundManager.Instance.PlaySoundWinCrewamate();
        }
        else
        {
            SoundManager.Instance.PlaySoundWinImposter();
        }

        StartCoroutine(IEWaitShowFx());
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (!isShow)
            GameManager.Instance.ShowInterAdsEndGame("end_game_win");
    }

    private void SetupRewardEndGame()
    {
        txtCoinReward.gameObject.SetActive(false);
        var playerData = GameManager.Instance.PlayerDataManager;
        int indexReward = playerData.GetCurrentIndexRewardEndGame();

        if (indexReward >= playerData.DataRewardEndGame.Datas.Count)
        {
            indexReward = 0;
            playerData.SetCurrentIndexRewardEndGame(indexReward);
        }

        var reward = playerData.DataRewardEndGame.Datas[indexReward];
        if (reward.Type == TypeEquipment.ELEMENT)
        {
            if (playerData.GetUnlockElement(reward.Id))
            {
                iconReward.sprite = playerData.DataTexture.IconCoin;
                txtCoinReward.text = string.Format("+{0}", reward.NumberCoinReplace);
                txtCoinReward.gameObject.SetActive(true);
            }
            else
            {
                iconReward.sprite = playerData.DataTexture.GetIconElement(reward.Id);
                iconReward.SetNativeSize();
            }
        }
        else if (reward.Type == TypeEquipment.SKIN)
        {
            if (playerData.GetUnlockSkin(TypeEquipment.SKIN, reward.Id))
            {
                iconReward.sprite = playerData.DataTexture.IconCoin;
                txtCoinReward.text = string.Format("+{0}", reward.NumberCoinReplace);
                txtCoinReward.gameObject.SetActive(true);
            }
            else
            {
                iconReward.sprite = playerData.DataTexture.GetIconSkin(reward.Id);
            }
        }
        
        int process = playerData.GetProcessReceiveRewardEndGame();
        process++;
        playerData.SetProcessReceiveRewardEndGame(process);
        if (process >= reward.NumberWin)
        {
            playerData.SetProcessReceiveRewardEndGame(0);

            StartCoroutine(IEWaitShowRewardEndGame(reward));

        }
        else
        {
            // check show rate game
            int lvlShowRate = 1; //RocketRemoteConfig.GetIntConfig("config_show_rate_game", 1000);
            if (PlayerPrefs.GetInt("showRate", 0) == 0 && GameManager.Instance.PlayerDataManager.GetDataLevel().DisplayLevel >= lvlShowRate)
            {
                StartCoroutine(IEShowRateGame());
                PlayerPrefs.SetInt("showRate", 1);
            }
        }

        float ratio = (float)process / (float)reward.NumberWin;
        imgProcessReward.fillAmount = 0;
        imgProcessReward.DOFillAmount(ratio, 1f);
        percentReward = (int)(ratio * 100);
        SetPercentReward(percentReward);
    }

    private Tweener tweenCoin;
    private int tmpPercent;
    private void SetPercentReward(int percent)
    {
        tweenCoin = tweenCoin ?? DOTween.To(() => tmpPercent, x =>
        {
            tmpPercent = x;
            txtProcessReward.text = string.Format("{0}%", tmpPercent);
        }, percent, 1f).SetAutoKill(false).OnComplete(() =>
         {
             tmpPercent = percentReward;
             txtProcessReward.text = string.Format("{0}%", tmpPercent);
         });

        tweenCoin.ChangeStartValue(tmpPercent);
        tweenCoin.ChangeEndValue(percent);
        tweenCoin.Play();
    }

    private void OnClickBtnNoThank()
    {
        OnBackPressed();
        GameManager.Instance.Profile.AddGold(Constants.GOLD_WIN, "end_game");
        GameManager.Instance.LoadLevelScene(GameManager.Instance.CurrentLevel + 1);
        SoundManager.Instance.PlaySoundButton();
    }

    private void OnClickBtnX3Coin()
    {
        arrow.GetComponent<DOTweenAnimation>().DOPause();
        GetComponent<ShowRewardedController>().Show();
        SoundManager.Instance.PlaySoundButton();
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

        btnX3Coin.gameObject.SetActive(false);

        GameManager.Instance.Profile.AddGold(_gold * factor, Helper.video_reward_end_game);

        txtCoin.text = string.Format("+{0}", _gold * factor);
        SoundManager.Instance.PlaySoundReward();
    }

    private IEnumerator IEWaitShowRewardEndGame(RewardEndGame reward)
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.END_GAME);
    }

    private IEnumerator IEWaitShowFx()
    {
        yield return new WaitForSeconds(0.5f);
        objFx.SetActive(true);
    }

    private IEnumerator IEShowRateGame()
    {
        yield return new WaitForSeconds(0.5f);
        PopupRateGame.Instance.Show();
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
