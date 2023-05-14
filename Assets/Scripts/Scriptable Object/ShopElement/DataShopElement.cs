using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "DataShopElement", menuName = "ScriptableObjects/Data Shop Element")]
public class DataShopElement : SerializedScriptableObject
{
    public Dictionary<Skill, ShopElementData> shopElementDatas;
}

public struct ShopElementData
{
    public TypeUnlockItemShop typeUnlock;
    public int numberVideoUnlock;
    public int numberCoinUnlock;
    public bool isLocked;
}
