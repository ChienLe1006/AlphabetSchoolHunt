using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataMoneyUnlockAlphabet", menuName = "ScriptableObjects/Data Money To Unlock Alphabet")]
public class DataMoneyUnlockAlphabet : SerializedScriptableObject
{
    public Dictionary<Alphabet, int> moneyToUnlockAlphabet;
}
