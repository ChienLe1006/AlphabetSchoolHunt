using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataNoti", menuName = "ScriptableObjects/Data Data Noti")]
public class DataNotification : SerializedScriptableObject
{
    public Dictionary<TypeNoti, DataNoti> DictDataNoti;
}

public class DataNoti
{
    public List<string> ListTitles;
    public List<string> ListContents;
}

public enum TypeNoti
{
    None = 0,
    Spin = 1,
    Open_App = 2
}