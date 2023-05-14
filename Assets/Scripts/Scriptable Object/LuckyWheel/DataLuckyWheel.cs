using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataLuckyWheel", menuName = "ScriptableObjects/Data Lucky Wheel")]
public class DataLuckyWheel : SerializedScriptableObject
{
    public int NumberSpinDaily;

    [TableList(ShowIndexLabels = true, DrawScrollView = true, MaxScrollViewHeight = 400, MinScrollViewHeight = 200)]
    public List<DataRewardLuckyWheel> ListDataRewrds;
    [TableList(ShowIndexLabels = true, DrawScrollView = true, MaxScrollViewHeight = 400, MinScrollViewHeight = 200)]
    public List<RewardEndGame> ListDataReceiveFrees;

    public int GetIdLuckyWheel()
    {

        return Random.Range(0, ListDataRewrds.Count);

    }
}

public class DataRewardLuckyWheel
{
    public TypeGift Type;
    public int IdType;
    public int Amount;
    public int NumberCoinReplace;
}
