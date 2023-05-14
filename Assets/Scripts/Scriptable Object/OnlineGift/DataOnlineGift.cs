using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "DataOnlineGift", menuName = "ScriptableObjects/Data Online Gift")]
public class DataOnlineGift : SerializedScriptableObject
{
    public List<RewardOnline> rewardOnlineDatas;
}

public class RewardOnline
{
    public TypeGift Type;
    public int Amount;
    public Sprite sprite;
}
