using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Profile
{

    public int GetNumberPlay()
    {
        return GameManager.Instance.PlayerDataManager.GetNumberPlay();
    }

    public void SetNumberPlay(int num)
    {
        var playerdata = GameManager.Instance.PlayerDataManager;
        playerdata.SetNumberPlay(num);
    }

    public void AddGold(int goldBonus, string _analytic = null)
    {
        var playerdata = GameManager.Instance.PlayerDataManager;
        int _count = GetGold() + goldBonus;
        playerdata.SetGold(_count);

        if (playerdata.actionUITop != null)
        {
            playerdata.actionUITop(TypeItemAnim.Coin);
        }
    }

    public int GetGold()
    {
        return GameManager.Instance.PlayerDataManager.GetGold();
    }

    public void AddKey(int amount, string _analytic)
    {
        var playerdata = GameManager.Instance.PlayerDataManager;

        playerdata.SetKey(GetKey() + amount);

        if (playerdata.actionUITop != null && amount == 1)
        {
            playerdata.actionUITop(TypeItemAnim.Key);
        }
    }

    public int GetKey()
    {
        return GameManager.Instance.PlayerDataManager.GetKey();
    }
}
