using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public UiMainLobby UiMainLobby;
    public UiLose UiLose;
    public UiWin UiWin;
    public ShopController Shop;
    public UiTop UiTop;
    public PopupRewardEndGame PopupRewardEndGame;
    public LuckyWheel LuckeyWheel;
    public SkeletonGraphic Loading;
    public GameObject ObjTutHand;
    public PopupDailyReward dailyReward;
    public GameObject ObjJoyStick;
    public PopupMoneyAds PopupMoneyAds;
    public PopupUpgrade PopupUpgrade;
    public PopupNewLetter PopupNewLetter;
    public GameObject UILevelInfo;
    public PopupWaterCleaning PopupWaterCleaning;
    public PopupTrashCatcher PopupTrashCatcher;

    public void Init()
    {
        UltimateJoystick.DisableJoystick(Constants.MAIN_JOINSTICK);
    }

    public void OpenUiLose()
    {
        UiLose.Show(true);
    }

    public void OpenUiWin(int gold)
    {
        UiWin.Show(true);
        UiWin.Init(gold);
    }

    public void OpenShop()
    {
        Shop.Show(true);
    }

    public void OpenPopupReward(RewardEndGame reward, TypeDialogReward type)
    {
        if (PopupRewardEndGame.IsShow)
            return;

        PopupRewardEndGame.Show(true);
        PopupRewardEndGame.Init(reward, type);
    }

    public void OpenPopupRewardDaily(RewardDaily reward, bool x2)
    {
        if (PopupRewardEndGame.IsShow)
            return;

        PopupRewardEndGame.Show(true);
        PopupRewardEndGame.DailyRewardInit(reward, x2);
    }

    public void OpenPopupRewardLetter(RewardUnlockLetter reward)
    {
        PopupRewardEndGame.Show(true);
        PopupRewardEndGame.RewardLetterInit(reward);
    }

    public void OpenLuckyWheel()
    {
        LuckeyWheel.Show(true);
    }

    public void OpenDailyReward()
    {
        dailyReward.Show(true);
    }

    public void OpenLoading(bool isLoading)
    {
        Loading.gameObject.SetActive(isLoading);
    }

    public void ActiveTutHand(bool isActive)
    {
        ObjTutHand.SetActive(isActive);
    }

    public void OpenPopupMoneyAds(int amount, GameObject obj)
    {
        PopupMoneyAds.Show(true);
        PopupMoneyAds.Init(amount, obj);
    }

    public void OpenPopupUpgrade()
    {
        PopupUpgrade.Show(true);
        PopupUpgrade.Init();
    }

    public void OpenPopupNewLetter(int id)
    {
        PopupNewLetter.Show(true);
        PopupNewLetter.Init(id);
    }

    public void OpenPopupWaterCleaning(int amount, GameObject obj)
    {
        PopupWaterCleaning.Show(true);
        PopupWaterCleaning.Init(amount, obj);
    }

    public void OpenPopupTrashCatcher(GameObject obj)
    {
        PopupTrashCatcher.Show(true);
        PopupTrashCatcher.Init(obj);
    }
}

