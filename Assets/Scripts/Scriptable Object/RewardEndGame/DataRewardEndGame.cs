using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataRewardEndGame", menuName = "ScriptableObjects/Data Reward EndGame")]
public class DataRewardEndGame : SerializedScriptableObject
{
    public List<RewardEndGame> Datas;
}

public struct RewardEndGame
{
    public TypeEquipment Type;
    public int Id;
    public int NumberWin;
    public int NumberCoinReplace;
}
