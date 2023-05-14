using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ItemSkinCharacter : ItemShop
{
    internal override void Init(int id)
    {
        PlayerDataManager playerData = PlayerDataManager.Instance;
        icon.sprite = playerData.DataTexture.GetIconSkin(id);
        _id = id;

        var skinData = playerData.DataShopSkinCharacter.shopSkinDatas[(SkinCharacter)id];
        if (!skinData.isLocked)
        {
            if (playerData.GetUnlockSkin(TypeEquipment.SKIN, id))
            {
                btnEquip.gameObject.SetActive(true);
                btnSpin.gameObject.SetActive(false);
                btnMoney.gameObject.SetActive(false);
                btnAds.gameObject.SetActive(false);
                if (playerData.GetIdEquipSkin(TypeEquipment.SKIN) == id)
                {
                    btnEquipImg.sprite = equiped;
                    btnEquip.gameObject.GetComponentInChildren<Text>().text = "EQUIPED";
                }
                else
                {
                    btnEquipImg.sprite = equip;
                    btnEquip.gameObject.GetComponentInChildren<Text>().text = "EQUIP";
                }
            }
            else
            {
                if (skinData.unlockType == TypeUnlockItemShop.SPIN)
                {
                    btnSpin.gameObject.SetActive(true);
                    btnMoney.gameObject.SetActive(false);
                    btnAds.gameObject.SetActive(false);
                    btnEquip.gameObject.SetActive(false);
                }
                else if (skinData.unlockType == TypeUnlockItemShop.COIN)
                {
                    btnMoney.gameObject.SetActive(true);
                    btnSpin.gameObject.SetActive(false);
                    btnAds.gameObject.SetActive(false);
                    btnEquip.gameObject.SetActive(false);

                    txtMoney.text = $"{skinData.numberCoinUnlock}";
                }
                else if (skinData.unlockType == TypeUnlockItemShop.VIDEO)
                {
                    btnAds.gameObject.SetActive(true);
                    btnMoney.gameObject.SetActive(false);
                    btnSpin.gameObject.SetActive(false);
                    btnEquip.gameObject.SetActive(false);

                    int numberWatchVideo = playerData.GetNumberWatchVideoSkin(TypeEquipment.SKIN, id);
                    txtAds.text = string.Format("{0}/{1}", numberWatchVideo, skinData.numberVideoUnlock);
                }
            }
        }
        else
        {
            btn.SetActive(false);
            lockImg.gameObject.SetActive(true);
        }
    }

    public override void OnClickBtnView()
    {
        base.OnClickBtnView();
        ShopSkinController.Instance.Select(_id);
    }

    public override void OnClickBtnVideo()
    {
        OnClickBtnView();
        GetComponent<ShowRewardedController>().Show();
    }

    public override void OnVideoDone(bool success)
    {
        if (success)
        {
            int numberWatchVideo = playerData.GetNumberWatchVideoSkin(TypeEquipment.SKIN, _id);
            numberWatchVideo++;
            if (numberWatchVideo >= dataShopSkin.shopSkinDatas[(SkinCharacter)_id].numberVideoUnlock)
            {
                playerData.SetUnlockSkin(TypeEquipment.SKIN, _id);
            }

            playerData.SetNumberWatchVideoSkin(TypeEquipment.SKIN, _id, numberWatchVideo);
            ShopSkinController.Instance.UpdateBtnDisplay();
        }
    }

    public override void OnClickBtnMoney()
    {
        OnClickBtnView();
        if (GameManager.Instance.Profile.GetGold() < dataShopSkin.shopSkinDatas[(SkinCharacter)_id].numberCoinUnlock)
        {
            ShopSkinController.Instance.ShopController.ActiveNotiNotEnoughtGold(this.transform);
            return;
        }

        playerData.SetUnlockSkin(TypeEquipment.SKIN, _id);
        GameManager.Instance.Profile.AddGold(-dataShopSkin.shopSkinDatas[(SkinCharacter)_id].numberCoinUnlock);
        ShopSkinController.Instance.UpdateBtnDisplay();
    }

    public override void OnClickBtnSpin()
    {
        base.OnClickBtnSpin();
        ShopSkinController.Instance.ShopController.OnBackPressed();
        GameManager.Instance.UiController.OpenLuckyWheel();
    }

    public override void OnClickBtnEquip()
    {
        base.OnClickBtnEquip();
        OnClickBtnView();
        playerData.SetIdEquipSkin(TypeEquipment.SKIN, _id);
        ShopSkinController.Instance.UpdateBtnDisplay();
    }
}
