using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataRewardUnlockLetter", menuName = "ScriptableObjects/Data Reward Unlock Letter")]
public class DataRewardUnlockLetter : SerializedScriptableObject
{
    public Dictionary<int, RewardUnlockLetter> dataRewardUnlockLetter;
    public Sprite replaceSprite;
}

public struct RewardUnlockLetter
{
    public Skill skill;
    public int Id => (int)skill;
    public int numberMoneyReplace;
}
