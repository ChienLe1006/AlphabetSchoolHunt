using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemElement : ItemShop
{
    internal override void Init(int id)
    {
        PlayerDataManager playerData = PlayerDataManager.Instance;
        icon.sprite = playerData.DataTexture.GetIconElement(id);
        icon.SetNativeSize();
        _id = id;
        
        var skillData = playerData.DataShopElement.shopElementDatas[(Skill)id];
        if (!skillData.isLocked)
        {
            if (playerData.GetUnlockElement(id))
            {
                btnEquip.gameObject.SetActive(true);
                btnSpin.gameObject.SetActive(false);
                btnMoney.gameObject.SetActive(false);
                btnAds.gameObject.SetActive(false);
                if (playerData.GetIdEquipElement() == id)
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
                if (skillData.typeUnlock == TypeUnlockItemShop.SPIN)
                {
                    btnSpin.gameObject.SetActive(true);
                    btnMoney.gameObject.SetActive(false);
                    btnAds.gameObject.SetActive(false);
                    btnEquip.gameObject.SetActive(false);
                }
                else if (skillData.typeUnlock == TypeUnlockItemShop.COIN)
                {
                    btnMoney.gameObject.SetActive(true);
                    btnSpin.gameObject.SetActive(false);
                    btnAds.gameObject.SetActive(false);
                    btnEquip.gameObject.SetActive(false);

                    txtMoney.text = $"{skillData.numberCoinUnlock}";
                }
                else if (skillData.typeUnlock == TypeUnlockItemShop.VIDEO)
                {
                    btnAds.gameObject.SetActive(true);
                    btnMoney.gameObject.SetActive(false);
                    btnSpin.gameObject.SetActive(false);
                    btnEquip.gameObject.SetActive(false);

                    int numberWatchVideo = playerData.GetNumberWatchVideoSkin(TypeEquipment.ELEMENT, id);
                    txtAds.text = string.Format("{0}/{1}", numberWatchVideo, skillData.numberVideoUnlock);
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
        ShopElementController.Instance.Select(_id);
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
            int numberWatchVideo = playerData.GetNumberWatchVideoSkin(TypeEquipment.ELEMENT, _id);
            numberWatchVideo++;
            if (numberWatchVideo >= dataShopElement.shopElementDatas[(Skill)_id].numberVideoUnlock)
            {
                playerData.SetUnlockElement(_id);
            }

            playerData.SetNumberWatchVideoSkin(TypeEquipment.ELEMENT, _id, numberWatchVideo);
            ShopElementController.Instance.UpdateBtnDisplay();
        }
    }

    public override void OnClickBtnMoney()
    {
        OnClickBtnView();
        if (GameManager.Instance.Profile.GetGold() < dataShopElement.shopElementDatas[(Skill)_id].numberCoinUnlock)
        {
            ShopElementController.Instance.ShopController.ActiveNotiNotEnoughtGold(this.transform);
            return;
        }

        playerData.SetUnlockElement(_id);
        GameManager.Instance.Profile.AddGold(-dataShopElement.shopElementDatas[(Skill)_id].numberCoinUnlock);
        ShopElementController.Instance.UpdateBtnDisplay();
    }

    public override void OnClickBtnSpin()
    {
        base.OnClickBtnSpin();
        ShopElementController.Instance.ShopController.OnBackPressed();
        GameManager.Instance.UiController.OpenLuckyWheel();
    }

    public override void OnClickBtnEquip()
    {
        base.OnClickBtnEquip();
        OnClickBtnView();
        playerData.SetIdEquipElement(_id);
        ShopElementController.Instance.UpdateBtnDisplay();
    }   
}
