using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectedAlphabetUIItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text amount;
    private int _id;

    internal void Init(int id)
    {
        icon.sprite = PlayerDataManager.Instance.DataTexture.GetIconAlphabet(id);
        _id = id;
    }

    internal void SetTxtAmount()
    {
        amount.text = PlayerDataManager.Instance.GetAlphabetAmountInBag(_id).ToString();
    }
}
