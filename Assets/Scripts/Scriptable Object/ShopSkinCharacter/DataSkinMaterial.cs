using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataSkinMaterial", menuName = "ScriptableObjects/Data Skin Material")]
public class DataSkinMaterial : SerializedScriptableObject
{
    public Dictionary<SkinCharacter, Material> skinMaterials;
}
