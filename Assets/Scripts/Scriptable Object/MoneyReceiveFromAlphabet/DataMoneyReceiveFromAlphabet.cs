using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataMoneyReceiveFromAlphabet", menuName = "ScriptableObjects/Data Money Receive From Alphabet")]
public class DataMoneyReceiveFromAlphabet : SerializedScriptableObject
{
    public Dictionary<Alphabet, int> dataMoneyReceiveFromAlphabet;
}
