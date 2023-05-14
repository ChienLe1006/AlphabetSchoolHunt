using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "DataTextureSkin", menuName = "ScriptableObjects/Data Texture Skin")]
public class DataTextureSkin : SerializedScriptableObject
{
    public Dictionary<Skin, Texture> dictTextureSkin;
    public Material[] bodyOutlineMaterials;
    public Material[] hatOutlineMaterials;
}
