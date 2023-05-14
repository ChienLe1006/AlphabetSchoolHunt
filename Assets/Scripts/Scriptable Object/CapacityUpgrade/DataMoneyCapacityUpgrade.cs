using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataMoneyCapacityUpgrade", menuName = "ScriptableObjects/Data Money To Upgrade Capacity")]
public class DataMoneyCapacityUpgrade : SerializedScriptableObject
{
    public Dictionary<int, int> moneyToUpgradeCapacity;
}
