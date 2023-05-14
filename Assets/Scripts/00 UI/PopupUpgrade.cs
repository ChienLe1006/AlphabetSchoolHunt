using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupUpgrade : UICanvas
{
    [SerializeField] private Transform content;
    private PlayerDataManager playerData;

    [Header("CAPACITY")]
    [SerializeField] private Button capacityBtn;
    [SerializeField] private Text capacityPriceTxt, bagLevelTxt;
    private int capacityUpgradeMoney;

    private void Start()
    {        
        capacityBtn.onClick.AddListener(UpgradeCapacity);
    }

    public void Init()
    {
        playerData = PlayerDataManager.Instance;
        content.DOScale(1, 0.4f).SetEase(Ease.OutBack);
        capacityUpgradeMoney = playerData.DataMoneyCapacityUpgrade.moneyToUpgradeCapacity[playerData.BagLevel];

        UpdateDisplay();
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

    private void UpdateDisplay()
    {
        if (playerData.BagLevel == 6)
        {
            capacityBtn.interactable = false;
            capacityPriceTxt.text = "MAX";
            bagLevelTxt.text = "MAX";
            if (playerData.actionUITop != null)
            {
                playerData.actionUITop(TypeItemAnim.Bag);
            }
        }
        else
        {
            if (playerData.GetGold() < capacityUpgradeMoney) capacityBtn.interactable = false;
            else capacityBtn.interactable = true;

            bagLevelTxt.text = $"LV.{playerData.BagLevel}";
            capacityPriceTxt.text = $"{capacityUpgradeMoney}";

            if (playerData.actionUITop != null)
            {
                playerData.actionUITop(TypeItemAnim.Bag);
            }
        }       
    }

    private void UpgradeCapacity()
    {
        GameManager.Instance.Profile.AddGold(-capacityUpgradeMoney);
        playerData.BagCapacity++;
        playerData.BagLevel++;
        capacityUpgradeMoney = playerData.DataMoneyCapacityUpgrade.moneyToUpgradeCapacity[playerData.BagLevel];

        UpdateDisplay();
    }
}
