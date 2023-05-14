using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    public Action<TypeItemAnim> actionUITop;
    public DataTexture DataTexture;
    public DataSkinMaterial DataSkinMaterial;
    public DataRewardEndGame DataRewardEndGame;
    public DataLuckyWheel DataLuckyWheel;
    public DataDailyReward DataDailyReward;
    public DataOnlineGift DataOnlineGift;
    public DataShopElement DataShopElement;
    public DataShopSkinCharacter DataShopSkinCharacter;
    public DataMoneyUnlockLevel DataMoneyUnlockLevel;
    public DataMoneyUnlockAlphabet DataMoneyUnlockAlphabet;
    public DataMoneyCapacityUpgrade DataMoneyCapacityUpgrade;
    public DataMoneyReceiveFromAlphabet DataMoneyReceiveFromAlphabet;
    public DataRewardUnlockLetter DataRewardUnlockLetter;
    public Action onCloseRewardedAds;
    public Action onCloseInterAds;
    private DataLevel dataLevel;
    private Alphabet firstUnlockAlphabet;
    private NPCAlphabet firstNPCAlphabet;
    public Alphabet FirstUnlockAlphabet 
    {
        get => firstUnlockAlphabet;
        set => firstUnlockAlphabet = value;
    }

    public NPCAlphabet FirstNPCAlphabet
    {
        get => firstNPCAlphabet;
        set => firstNPCAlphabet = value;
    }

    public void SetDataLevel(DataLevel dataLevel)
    {
        this.dataLevel = dataLevel;
        PlayerPrefs.SetString(Helper.DataLevel, JsonUtility.ToJson(dataLevel));
        SetMaxLevelReached(dataLevel.Level);
    }

    public DataLevel GetDataLevel()
    {
        if (dataLevel != null)
        {
            return dataLevel;
        }

        var dataLevelJson = PlayerPrefs.GetString(Helper.DataLevel, string.Empty);
        if (dataLevelJson == string.Empty)
        {
            dataLevel = new DataLevel(1, 1);
        }
        else
        {
            dataLevel = JsonUtility.FromJson<DataLevel>(dataLevelJson);
        }

        return dataLevel;
    }

    private void SetMaxLevelReached(int currentLevel)
    {
        PlayerPrefs.SetInt(Helper.DataMaxLevelReached, Math.Max(GetMaxLevelReached(), currentLevel));
    }

    public int GetMaxLevelReached()
    {
        return PlayerPrefs.GetInt(Helper.DataMaxLevelReached, 1);
    }

    public bool GetUnlockSkin(TypeEquipment type, int id)
    {
        return PlayerPrefs.GetInt(Helper.DataTypeSkin + type + id, 0) == 0 ? false : true;
    }

    public void SetUnlockSkin(TypeEquipment type, int id)
    {
        PlayerPrefs.SetInt(Helper.DataTypeSkin + type + id, 1);
    }

    public int GetIdEquipSkin(TypeEquipment type)
    {
        return PlayerPrefs.GetInt(Helper.DataEquipSkin + type, 0);
    }

    public void SetIdEquipSkin(TypeEquipment type, int id)
    {
        PlayerPrefs.SetInt(Helper.DataEquipSkin + type, id);
    }

    public bool GetUnlockElement(int id)
    {
        return PlayerPrefs.GetInt(Helper.DataTypeElement + id, 0) == 0 ? false : true;
    }

    public void SetUnlockElement(int id)
    {
        PlayerPrefs.SetInt(Helper.DataTypeElement + id, 1);
        if (!FirstSkillUnlock) FirstSkillUnlock = true;
    }

    public int GetIdEquipElement()
    {
        return PlayerPrefs.GetInt(Helper.CurrentEquipedElement, 0);
    }

    public void SetIdEquipElement(int id)
    {
        PlayerPrefs.SetInt(Helper.CurrentEquipedElement, id);        
    }

    public int GetNumberWatchVideoSkin(TypeEquipment type, int id)
    {
        return PlayerPrefs.GetInt(Helper.DataNumberWatchVideo + type + id, 0);
    }

    public void SetNumberWatchVideoSkin(TypeEquipment type, int id, int number)
    {
        PlayerPrefs.SetInt(Helper.DataNumberWatchVideo + type + id, number);
    }

    public int GetGold()
    {
        return PlayerPrefs.GetInt(Helper.GOLD, 100);
    }

    public void SetGold(int _count)
    {
        PlayerPrefs.SetInt(Helper.GOLD, _count);
    }

    public int GetKey()
    {
        return PlayerPrefs.GetInt(Helper.KEY, 0);
    }

    public void SetKey(int _count)
    {
        PlayerPrefs.SetInt(Helper.KEY, _count);
    }

    public int GetCurrentIndexRewardEndGame()
    {
        return PlayerPrefs.GetInt(Helper.CurrentRewardEndGame, 0);
    }

    public void SetCurrentIndexRewardEndGame(int index)
    {
        PlayerPrefs.SetInt(Helper.CurrentRewardEndGame, index);
    }

    public int GetProcessReceiveRewardEndGame()
    {
        return PlayerPrefs.GetInt(Helper.ProcessReceiveEndGame, 0);
    }

    public void SetProcessReceiveRewardEndGame(int number)
    {
        PlayerPrefs.SetInt(Helper.ProcessReceiveEndGame, number);
    }


    public int GetNumberWatchDailyVideo()
    {
        return PlayerPrefs.GetInt("NumberWatchDailyVideo", DataLuckyWheel.NumberSpinDaily);
    }

    public void SetNumberWatchDailyVideo(int number)
    {
        PlayerPrefs.SetInt("NumberWatchDailyVideo", number);
    }

    public bool GetFreeSpin()
    {
        return PlayerPrefs.GetInt("FreeSpin", 1) > 0 ? true : false;
    }

    public void SetFreeSpin(bool isFree)
    {
        int free = isFree ? 1 : 0;
        PlayerPrefs.SetInt("FreeSpin", free);
    }

    public int GetNumberWatchVideoSpin()
    {
        return PlayerPrefs.GetInt("NumberWatchVideoSpin", 0);

    }

    public void SetNumberWatchVideoSpin(int count)
    {
        PlayerPrefs.SetInt("NumberWatchVideoSpin", count);
    }

    public string GetTimeLoginSpinFreeWheel()
    {
        return PlayerPrefs.GetString("TimeSpinFreeWheel", "");
    }

    public void SetTimeLoginSpinFreeWheel(string time)
    {
        PlayerPrefs.SetString("TimeSpinFreeWheel", time);
    }

    public string GetTimeLoginSpinVideo()
    {
        return PlayerPrefs.GetString("TimeLoginSpinVideo", "");
    }

    public void SetTimeLoginSpinVideo(string time)
    {
        PlayerPrefs.SetString("TimeLoginSpinVideo", time);
    }

    public string GetTimeOnlineGift()
    {
        return PlayerPrefs.GetString("TimeOnlineGift", "");
    }

    public void SetTimeOnlineGift(string time)
    {
        PlayerPrefs.SetString("TimeOnlineGift", time);
    }

    public bool GetIsGetOnlineGiftFirstTime()
    {
        return PlayerPrefs.GetInt("OnlineGift1stTime", 0) > 0 ? true : false;
    }

    public void SetIsGetOnlineGiftFirstTime(bool status)
    {
        int value = status ? 1 : 0;
        PlayerPrefs.SetInt("OnlineGift1stTime", value);
    }

    public void SetSoundSetting(bool isOn)
    {
        PlayerPrefs.SetInt(Helper.SoundSetting, isOn ? 1 : 0);
    }

    public bool GetSoundSetting()
    {
        return PlayerPrefs.GetInt(Helper.SoundSetting, 1) == 1;
    }

    public void SetMusicSetting(bool isOn)
    {
        PlayerPrefs.SetInt(Helper.MusicSetting, isOn ? 1 : 0);
    }

    public bool GetMusicSetting()
    {
        return PlayerPrefs.GetInt(Helper.MusicSetting, 1) == 1;

    }

    public bool IsNoAds()
    {
        return PlayerPrefs.GetInt("NoAds", 0) == 1;
    }

    public void SetNoAds()
    {
        PlayerPrefs.SetInt("NoAds", 1);
    }

    private List<int> listIdSkin = new List<int>();
    public int GetIdSkinOtherPlayer()
    {
        if (listIdSkin.Count == 0)
        {
            for (int i = 1; i < 12; i++)
            {
                listIdSkin.Add(i);
            }
        }

        var index = UnityEngine.Random.Range(0, listIdSkin.Count);
        int id = listIdSkin[index];
        listIdSkin.RemoveAt(index);

        return id;
    }

    public void ClearListIdSkin()
    {
        if (listIdSkin.Count > 0)
            listIdSkin.Clear();
    }

    public void SetNumberPlay(int num)
    {
        PlayerPrefs.SetInt("NumberPlay", num);
    }

    public int GetNumberPlay()
    {
        return PlayerPrefs.GetInt("NumberPlay", 0);
    }

    public void SetQuitTime(string time)
    {
        PlayerPrefs.SetString("QuitTime", time);
    }

    public string GetQuitTime()
    {
        return PlayerPrefs.GetString("QuitTime", "");
    }

    public bool GetQuitGame()
    {
        return PlayerPrefs.GetInt("QuitGame", 0) == 1 ? true : false;
    }

    public void SetQuitGame(bool status)
    {
        int value = status? 1 : 0;
        PlayerPrefs.SetInt("QuitGame", value);
    } 

    public int OnlineTimeMark
    {
        get
        {
            return PlayerPrefs.GetInt("online_mark", 1);
        }
        set
        {
            PlayerPrefs.SetInt("online_mark", value);
        }
    }

    public bool FirstOnlineGiftInDay()
    {
        return PlayerPrefs.GetInt("first_online_gift_in_day", 1) == 1 ? true : false;
    }

    public void SetFirstOnlineGiftInDay(bool status)
    {
        int value = status ? 1 : 0;
        PlayerPrefs.SetInt("first_online_gift_in_day", value);
    }

    private static DateTime GetDateTime(string key, DateTime defaultValue)
    {
        string strValue = PlayerPrefs.GetString(key);
        DateTime result = defaultValue;
        if (!string.IsNullOrEmpty(strValue))
        {
            long dateData = Convert.ToInt64(strValue);
            result = DateTime.FromBinary(dateData);
        }
        return result;
    }

    private static void SetDateTime(string key, DateTime date)
    {
        PlayerPrefs.SetString(key, date.ToBinary().ToString());
    }

    public DateTime LastDailyRewardClaim
    {
        get
        {
            return GetDateTime("last_daily_reward", DateTime.MinValue);
        }
        set
        {
            SetDateTime("last_daily_reward", value);
        }
    }

    public int CurrentDailyRewardDayIndex
    {
        get
        {
            DateTime lastDailyRewardClaim = LastDailyRewardClaim;
            if (lastDailyRewardClaim == DateTime.MinValue)
            {
                return 0;
            }
            else
            {
                int lastDailyRewardDayIndex = LastDailyRewardDayIndex;
                if (lastDailyRewardClaim.Date == DateTime.Now.Date)
                {
                    return lastDailyRewardDayIndex;
                }
                else
                {
                    return lastDailyRewardDayIndex == 6 ? 0 : lastDailyRewardDayIndex + 1;
                }
            }
        }
    }

    public int LastDailyRewardDayIndex
    {
        get
        {
            return PlayerPrefs.GetInt("last_daily_reward_day_index", -1);
        }
        set
        {
            PlayerPrefs.SetInt("last_daily_reward_day_index", value);
        }
    }

    public bool NewUser()
    {
        return PlayerPrefs.GetInt("new_user", 1) == 1? true: false;
    }

    public void SetNewUser(bool status)
    {
        int value = status? 1 : 0;
        PlayerPrefs.SetInt("new_user", value);
    }

    public bool FirstTimePlay()
    {
        return PlayerPrefs.GetInt("first_time_play", 1) == 1 ? true : false;
    }

    public void SetFirstTimePlay(bool status)
    {
        int value = status ? 1 : 0;
        PlayerPrefs.SetInt("first_time_play", value);
    }

    public bool FirstAlphabetKill
    {
        get => GetBool("first_alphabet_kill");
        set => SetBool("first_alphabet_kill", value);
    }

    public int GetAlphabetAmountInBag(int idAlphabet)
    {
        return PlayerPrefs.GetInt("amount" + idAlphabet, 0);
    }

    public void AddAlphabetAmountInBag(int idAlphabet, int amount)
    {
        PlayerPrefs.SetInt("amount" + idAlphabet, GetAlphabetAmountInBag(idAlphabet) + amount);
    }

    public int CurrentAlphabetAmountInBag
    {
        get => PlayerPrefs.GetInt("current_alphabet_in_bag", 0);
        set => PlayerPrefs.SetInt("current_alphabet_in_bag", value);
    }

    public int BagCapacity
    {
        get => PlayerPrefs.GetInt("bag_capacity", 5);
        set => PlayerPrefs.SetInt("bag_capacity", value);
    }

    public int CurrentAmountInNPC
    {
        get => PlayerPrefs.GetInt("current_amount_in_npc", 0);
        set => PlayerPrefs.SetInt("current_amount_in_npc", value);
    }

    private static bool GetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetInt(key) == 1 : defaultValue;
    }

    private static void SetBool(string key, bool value)
    {        
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public int GetAmountMoneyUnlockLevel(int level)
    {
        return PlayerPrefs.GetInt("amount_money_unlock" + level, DataMoneyUnlockLevel.moneyToUnlockLevel[level]);
    }

    public void SetAmountMoneyUnlockLevel(int level, int amount)
    {
        PlayerPrefs.SetInt("amount_money_unlock" + level, GetAmountMoneyUnlockLevel(level) + amount);
    }

    public bool GetUnlockAlphabet(Alphabet alphabet)
    {
        return GetBool("alphabet" + alphabet);
    }

    public void SetUnlockAlphabet(Alphabet alphabet)
    {
        SetBool("alphabet" + alphabet, true);
    }

    public int GetAmountMoneyUnlockAlphabet(Alphabet alphabet)
    {
        return PlayerPrefs.GetInt("amount_money_unlock" + alphabet, DataMoneyUnlockAlphabet.moneyToUnlockAlphabet[alphabet]);
    }

    public void SetAmountMoneyUnlockAlphabet(Alphabet alphabet, int amount)
    {
        PlayerPrefs.SetInt("amount_money_unlock" + alphabet, GetAmountMoneyUnlockAlphabet(alphabet) + amount);
    }

    public bool FirstAlphabetUnlock
    {
        get => GetBool("first_alphabet_unlock");
        set => SetBool("first_alphabet_unlock", value);
    }

    public bool FirstAlphabetReturn
    {
        get => GetBool("first_alphabet_return");
        set => SetBool("first_alphabet_return", value);
    }

    public float OnlineGiftTime
    {
        get => PlayerPrefs.GetFloat("online_gift_time", 60f);
        set => PlayerPrefs.SetFloat("online_gift_time", value);
    }

    public int BagLevel
    {
        get => PlayerPrefs.GetInt("bag_level", 1);
        set => PlayerPrefs.SetInt("bag_level", value);
    }

    public bool FirstSkillUnlock
    {
        get => GetBool("first_skill_unlock");
        set => SetBool("first_skill_unlock", value);
    }

    public bool FirstGoToUpgrade
    {
        get => GetBool("first_go_to_upgrade");
        set => SetBool("first_go_to_upgrade", value);
    }
}
