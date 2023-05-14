using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "DataShopIap", menuName = "ScriptableObjects/Data Shop Iap")]
public class DataShopIap : SerializedScriptableObject
{
    public Dictionary<IdPack, InfoPackage> dictInfoPackage;
}


public class InfoPackage
{
    public string name;
    public List<DataElementGift> listRewardPack;

}
[Serializable]
public class DataElementGift
{
    public TypeGift type;
    public int amount;

    public DataElementGift(TypeGift _type, int _amount)
    {
        type = _type;
        amount = _amount;
    }

    public DataElementGift()
    {

    }
}