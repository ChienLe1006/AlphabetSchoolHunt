using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopElementController : Singleton<ShopElementController>
{
    [SerializeField] private Transform content;
    [SerializeField] private ItemElement itemElement;
    [SerializeField] private List<ItemElement> itemElements;
    [SerializeField] private ShopController shopController;
    public ShopController ShopController => shopController;

    private void Start()
    {
        for (int i = 0; i < Enum.GetNames(typeof(Skill)).Length; i++)
        {
            var skillItemUI = Instantiate(itemElement, content);
            skillItemUI.Init(i);
            itemElements.Add(skillItemUI);
        }
    }

    internal void Select(int id)
    {
        for (int i = 0; i < itemElements.Count; i++)
        {
            itemElements[i].DisableSelectImg();
        }
        itemElements[id].EnableSelectImg();
        shopController.SkinCharaterController.ChangeElementLogo(id);
    }

    internal void UpdateBtnDisplay()
    {
        for (int i = 0; i < itemElements.Count; i++)
        {
            itemElements[i].Init(i);
        }
    }

    private void OnEnable()
    {
        UpdateBtnDisplay();
    }
}
