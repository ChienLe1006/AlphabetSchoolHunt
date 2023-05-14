using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataShopSkinCharacter", menuName = "ScriptableObjects/Data Shop Skin Character")]
public class DataShopSkinCharacter : SerializedScriptableObject
{
    public Dictionary<SkinCharacter, ShopSkinData> shopSkinDatas;
}

public struct ShopSkinData
{
    public TypeUnlockItemShop unlockType;
    public int numberVideoUnlock;
    public int numberCoinUnlock;
    public bool isLocked;
}
