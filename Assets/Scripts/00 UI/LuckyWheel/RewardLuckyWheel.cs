using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardLuckyWheel : MonoBehaviour
{
    [SerializeField] private Image iconReward;
    [SerializeField] private Text txtNumberCoin;
    private Transform transIcon;
    private Transform tranNumberCoin;

    private void Start()
    {
        tranNumberCoin = txtNumberCoin.GetComponent<Transform>();
        transIcon = iconReward.GetComponent<Transform>();
    }

    public void Init(DataRewardLuckyWheel dataLuckyWheel)
    {
        var playerData = GameManager.Instance.PlayerDataManager;
        txtNumberCoin.gameObject.SetActive(true);
        switch (dataLuckyWheel.Type)
        {
            case TypeGift.GOLD:
                {
                    iconReward.sprite = playerData.DataTexture.IconCoinLuckyWheel;
                    txtNumberCoin.text = dataLuckyWheel.Amount.ToString();
                }
                break;

            case TypeGift.ELEMENT:
                {
                    if (playerData.GetUnlockElement(dataLuckyWheel.IdType))
                    {
                        iconReward.sprite = playerData.DataTexture.IconCoin;
                        txtNumberCoin.text = dataLuckyWheel.NumberCoinReplace.ToString();
                    }
                    else
                    {
                        iconReward.sprite = playerData.DataTexture.GetIconElement(dataLuckyWheel.IdType);
                        txtNumberCoin.gameObject.SetActive(false);
                    }
                }
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        //v3Rotate.z = -(_transWheel.eulerAngles.z + angle);
        tranNumberCoin.rotation = Quaternion.identity;
        transIcon.rotation = Quaternion.identity;

    }
}
