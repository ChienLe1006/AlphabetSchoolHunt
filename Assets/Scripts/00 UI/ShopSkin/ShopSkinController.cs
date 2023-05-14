using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSkinController : Singleton<ShopSkinController>
{
    [SerializeField] private Transform content;
    [SerializeField] private ItemSkinCharacter itemSkin;
    [SerializeField] private List<ItemSkinCharacter> itemSkins;
    [SerializeField] private ShopController shopController;
    public ShopController ShopController => shopController;

    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(SkinCharacter)).Length; i++)
        {
            var skinItemUI = Instantiate(itemSkin, content);
            skinItemUI.Init(i);
            itemSkins.Add(skinItemUI);
        }
    }

    internal void Select(int id)
    {
        for (int i = 0; i < itemSkins.Count; i++)
        {
            itemSkins[i].DisableSelectImg();
        }
        itemSkins[id].EnableSelectImg();
        shopController.SkinCharaterController.ChangeSkinCharacter(id);
    }

    internal void UpdateBtnDisplay()
    {
        for (int i = 0; i < itemSkins.Count; i++)
        {
            itemSkins[i].Init(i);
        }
    }

    private void OnEnable()
    {
        UpdateBtnDisplay();
    }
}
