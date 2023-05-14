using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataDailyReward", menuName = "ScriptableObjects/Data Daily Reward")]
public class DataDailyReward : SerializedScriptableObject
{
    public List<RewardDaily> dailyDatas;
}

public struct RewardDaily
{
    public TypeGift Type;
    public int Amount;
    public int IdType;
    public int NumberCoinReplace;
    public Sprite Sprite;
    public bool Countable;
}
