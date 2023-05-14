using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardItem : MonoBehaviour
{
    [SerializeField] private int dayIndex;
    [SerializeField] private Image iconReward;
    [SerializeField] private Text dayTxt;
    [SerializeField] private Text quantity;
    [SerializeField] private GameObject tickCheck;

    private void Start()
    {
        dayTxt.text = $"Day {dayIndex + 1}";       
        Init(PlayerDataManager.Instance.DataDailyReward.dailyDatas[dayIndex]);
    }

    private void OnEnable()
    {
        if (dayIndex <= PlayerDataManager.Instance.LastDailyRewardDayIndex)
        {
            tickCheck.SetActive(true);
        }
    }

    private void Init(RewardDaily dataDailyReward)
    {
        if (dataDailyReward.Countable)
        {
            quantity.gameObject.SetActive(true);
        }
        else quantity.gameObject.SetActive(false);

        var playerData = PlayerDataManager.Instance;
 
        switch (dataDailyReward.Type)
        {
            case TypeGift.GOLD:
                {
                    iconReward.sprite = dataDailyReward.Sprite;
                    quantity.text = $"{dataDailyReward.Amount}";
                }
                break;

            case TypeGift.ELEMENT:
                {
                    if (playerData.GetUnlockElement(dataDailyReward.IdType))
                    {
                        iconReward.sprite = dataDailyReward.Sprite;                      
                    }
                    else
                    {
                        iconReward.sprite = playerData.DataTexture.GetIconElement(dataDailyReward.IdType);
                        iconReward.SetNativeSize();
                    }
                }
                break;
            default:
                break;
        }        
    }
}
