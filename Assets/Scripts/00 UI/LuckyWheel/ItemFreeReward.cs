using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFreeReward : MonoBehaviour
{
    [SerializeField] private Text txtNumberSpin;
    [SerializeField] private GameObject ObjTick;

    private float WidthBar = 630;

    public void Init(RewardEndGame data)
    {
        txtNumberSpin.text = data.NumberWin.ToString();
        bool unlock = PlayerDataManager.Instance.GetUnlockElement(data.Id);
        if (!unlock)
        {
            int numberWatchVideo = GameManager.Instance.PlayerDataManager.GetNumberWatchVideoSpin();
            if (data.NumberWin <= numberWatchVideo)
            {              
                unlock = true;
                RewardEndGame reward = new RewardEndGame();
                reward.Type = data.Type;
                reward.Id = data.Id;
                //reward.skill = (Skill)data.Id;

                GameManager.Instance.UiController.OpenPopupReward(reward, TypeDialogReward.LUCKY_WHEEL);

                GameManager.Instance.PlayerDataManager.SetUnlockElement(data.Id);
                GameManager.Instance.PlayerDataManager.SetIdEquipElement(data.Id);
            }
        }

        ObjTick.SetActive(unlock);
        float ratio = (float)data.NumberWin / 20f;
        var v3 = new Vector3(ratio * WidthBar, 10, 0);
        this.GetComponent<RectTransform>().anchoredPosition = v3;
    }
}
