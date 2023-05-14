using UnityEngine;
using UnityEngine.UI;

public class PopupRewardEndGame : UICanvas
{
    [SerializeField] private Image imgIcon;
    [SerializeField] private Button btnVideo;
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnClaim;
    [SerializeField] private Text txtCoinDailyReward;
    [SerializeField] private Text txtCoinReplace;
    private RewardEndGame _reward;
    [SerializeField] private GameObject objParentRewards, decor;
    private bool isDailyReward, isRewardLetter;

    private void Start()
    {
        btnClose.onClick.AddListener(OnClickBtnClose);
        btnVideo.onClick.AddListener(OnClickBtnVideo);
        btnClaim.onClick.AddListener(OnClickBtnClose);
    }

    public void Init(RewardEndGame reward, TypeDialogReward type)
    {

        _reward = reward;
        objParentRewards.SetActive(false);
        txtCoinReplace.gameObject.SetActive(false);
        var playerData = GameManager.Instance.PlayerDataManager;
        if (reward.Type == TypeEquipment.ELEMENT)
        {
            if (playerData.GetUnlockElement(reward.Id))
            {
                imgIcon.sprite = playerData.DataTexture.IconCoin;
                txtCoinReplace.text = string.Format("+{0}", reward.NumberCoinReplace);
                txtCoinReplace.gameObject.SetActive(true);
            }
            else
            {
                imgIcon.sprite = playerData.DataTexture.GetIconElement(reward.Id);
                imgIcon.SetNativeSize();
            }
        }    
        else if (reward.Type == TypeEquipment.SKIN)
        {
            if (playerData.GetUnlockSkin(TypeEquipment.SKIN, reward.Id))
            {
                imgIcon.sprite = playerData.DataTexture.IconCoin;
                txtCoinReplace.text = string.Format("+{0}", reward.NumberCoinReplace);
                txtCoinReplace.gameObject.SetActive(true);
            }
            else
            {
                imgIcon.sprite = playerData.DataTexture.GetIconSkin(reward.Id);
            }
        }

        switch (type)
        {
            case TypeDialogReward.LUCKY_WHEEL:
                {
                    btnClaim.gameObject.SetActive(true);
                    btnVideo.gameObject.SetActive(false);
                    btnClose.gameObject.SetActive(false);
                    txtCoinReplace.gameObject.SetActive(false);
                    txtCoinDailyReward.gameObject.SetActive(false);
                }
                break;
            case TypeDialogReward.END_GAME:
                {
                    btnClaim.gameObject.SetActive(false);
                    btnVideo.gameObject.SetActive(true);
                    btnClose.gameObject.SetActive(true);
                    txtCoinDailyReward.gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    public void DailyRewardInit(RewardDaily reward, bool x2)
    {
        isDailyReward = true;
        objParentRewards.SetActive(false);
        txtCoinReplace.gameObject.SetActive(false);
        if (reward.Countable)
        {
            txtCoinDailyReward.gameObject.SetActive(true);         
        }
        else
        {
            txtCoinDailyReward.gameObject.SetActive(false);
        }
        var playerData = PlayerDataManager.Instance;
        switch (reward.Type)
        {
            case TypeGift.GOLD:
                {
                    imgIcon.sprite = reward.Sprite;
                    txtCoinDailyReward.text = x2 ? $"+{reward.Amount * 2}" : $"+{reward.Amount}";
                }
                break;

            case TypeGift.ELEMENT:
                {
                    if (playerData.GetUnlockElement(reward.IdType))
                    {
                        GameManager.Instance.Profile.AddGold(reward.NumberCoinReplace);
                        imgIcon.sprite = reward.Sprite;
                        txtCoinReplace.gameObject.SetActive(true);
                        txtCoinReplace.text = $"+{reward.NumberCoinReplace}";
                    }
                    else
                    {
                        imgIcon.sprite = playerData.DataTexture.GetIconElement(reward.IdType);
                        imgIcon.SetNativeSize();
                    }
                }
                break;
        }       

        btnClaim.gameObject.SetActive(true);
        btnVideo.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
    }

    internal void RewardLetterInit(RewardUnlockLetter reward)
    {
        isRewardLetter = true;
        objParentRewards.SetActive(false);
        txtCoinReplace.gameObject.SetActive(false);
        txtCoinDailyReward.gameObject.SetActive(false);

        var playerData = PlayerDataManager.Instance;
        if (playerData.GetUnlockElement(reward.Id))
        {
            GameManager.Instance.Profile.AddGold(reward.numberMoneyReplace);
            imgIcon.sprite = playerData.DataRewardUnlockLetter.replaceSprite;
            txtCoinReplace.gameObject.SetActive(true);
            txtCoinReplace.text = $"+{reward.numberMoneyReplace}";
        }
        else
        {
            playerData.SetUnlockElement(reward.Id);
            playerData.SetIdEquipElement(reward.Id);
            
            GameManager.Instance.CurrentLevelManager.Player.SetSkill(reward.Id);
            GameManager.Instance.CurrentLevelManager.Player.SkinCharater.ChangeElementLogo(reward.Id);

            imgIcon.sprite = playerData.DataTexture.GetIconElement(reward.Id);
            imgIcon.SetNativeSize();
        }
        btnClaim.gameObject.SetActive(true);
        btnVideo.gameObject.SetActive(false);
        btnClose.gameObject.SetActive(false);
    }

    private void OnClickBtnVideo()
    {
        GetComponent<ShowRewardedController>().Show();
        SoundManager.Instance.PlaySoundButton();
    }

    public override void OnBackPressed()
    {
        base.OnBackPressed();
        decor.SetActive(false);
    }

    public void GetReward(bool success)
    {
        if (success)
        {
            OnRewardedVideo(1);
        }
    }

    private void OnClickBtnClose()
    {
        bool isNextReward = true; //RocketRemoteConfig.GetBoolConfig("next_reward_end_game_user_lose_it", false);
        if (isNextReward && !isDailyReward && !isRewardLetter)
        {
            SetupNextReward();
        }
        OnBackPressed();
        SoundManager.Instance.PlaySoundButton();

        if (isRewardLetter)
        {  
            isRewardLetter = false;
            GameManager.Instance.CurrentLevelManager.ShowNextLevelCheck();           
        }
    }

    private void OnRewardedVideo(int x)
    {
        if (x <= 0 && !isShow)
            return;

        if (_reward.Type == TypeEquipment.ELEMENT)
        {
            if (PlayerDataManager.Instance.GetUnlockElement(_reward.Id))
            {
                GameManager.Instance.Profile.AddGold(_reward.NumberCoinReplace, Helper.video_reward_end_game);
            }
            else
            {
                PlayerDataManager.Instance.SetUnlockElement(_reward.Id);
                PlayerDataManager.Instance.SetIdEquipElement(_reward.Id);
            }
        }
        else if (_reward.Type == TypeEquipment.SKIN)
        {
            if (PlayerDataManager.Instance.GetUnlockSkin(TypeEquipment.SKIN, _reward.Id))
            {
                GameManager.Instance.Profile.AddGold(_reward.NumberCoinReplace, Helper.video_reward_end_game);
            }
            else
            {
                PlayerDataManager.Instance.SetUnlockSkin(TypeEquipment.SKIN, _reward.Id);
                PlayerDataManager.Instance.SetIdEquipSkin(TypeEquipment.SKIN, _reward.Id);
            }
        }

        SetupNextReward();
        OnBackPressed();
    }

    public void ActiveReward()
    {
        objParentRewards.SetActive(true);
        decor.SetActive(true);
        SoundManager.Instance.PlaySoundReward();
    }

    private void SetupNextReward()
    {
        var indexReward = GameManager.Instance.PlayerDataManager.GetCurrentIndexRewardEndGame();
        GameManager.Instance.PlayerDataManager.SetProcessReceiveRewardEndGame(0);
        indexReward++;
        GameManager.Instance.PlayerDataManager.SetCurrentIndexRewardEndGame(indexReward);
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
