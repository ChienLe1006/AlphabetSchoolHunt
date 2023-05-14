using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "DataShopSkin", menuName = "ScriptableObjects/Data Shop Skin", order = 1)]
public class DataShopSkin : SerializedScriptableObject
{
    [TableList(ShowIndexLabels = true, DrawScrollView = true, MaxScrollViewHeight = 400, MinScrollViewHeight = 200)]
    public List<DataShop> listDataShopHats;
    [TableList(ShowIndexLabels = true, DrawScrollView = true, MaxScrollViewHeight = 400, MinScrollViewHeight = 200)]
    public List<DataShop> listDataShopSkins;
    [TableList(ShowIndexLabels = true, DrawScrollView = true, MaxScrollViewHeight = 400, MinScrollViewHeight = 200)]
    public List<DataShop> listDataShopPets;

}

public class DataShop
{
    public int idSkin;
    public TypeUnlockItemShop typeUnlock;
    public int numberVideoUnlock;
    public int numberCoinUnlock;
    public int levelUnlock;

}


