using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "DataTexture", menuName = "ScriptableObjects/Data Texture")]
public class DataTexture : SerializedScriptableObject
{
   
    [SerializeField] private List<Sprite> ListIconHats;
    [SerializeField] private List<Sprite> ListIconSkins;
    [SerializeField] private List<Sprite> ListIconPets;
    [SerializeField] private List<Sprite> ListIconElements;
    [SerializeField] private List<Sprite> ListIconAlphabets;
    public Sprite IconCoin;
    public Sprite IconCoinLuckyWheel;

    [SerializeField] private List<Sprite> ListIconSprKey;

    public Sprite GetIconHat(int id)
    {
        if (id < ListIconHats.Count)
            return ListIconHats[id];

        return null;
    }

    public Sprite GetIconSkin(int id)
    {
        if (id < ListIconSkins.Count)
            return ListIconSkins[id];

        return null;
    }

    public Sprite GetIconPet(int id)
    {
        if (id < ListIconPets.Count)
            return ListIconPets[id];

        return null;
    }

    public Sprite GetIconElement(int id)
    {
        if (id < ListIconElements.Count)
            return ListIconElements[id];

        return null;
    }

    public Sprite GetIconAlphabet(int id)
    {
        if (id < ListIconAlphabets.Count)
            return ListIconAlphabets[id];

        return null;
    }

    public Sprite GetIconKey(bool isActive)
    {
        int index = isActive ? 0 : 1;
        return ListIconSprKey[index];
    }
}
