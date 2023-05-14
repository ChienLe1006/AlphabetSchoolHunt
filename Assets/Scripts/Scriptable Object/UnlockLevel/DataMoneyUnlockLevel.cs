using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataMoneyUnlockLevel", menuName = "ScriptableObjects/Data Money To Unlock Level")]
public class DataMoneyUnlockLevel : SerializedScriptableObject
{
    public Dictionary<int, int> moneyToUnlockLevel;
}
