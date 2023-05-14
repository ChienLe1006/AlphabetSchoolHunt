using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupCollectedAlphabet : Singleton<PopupCollectedAlphabet>
{
    [SerializeField] private CollectedAlphabetUIItem collectedAlphabetUI;

    private void OnEnable()
    {
        SetUpDisplay();
    }

    private void SetUpDisplay()
    {
        LevelManager level = GameManager.Instance.CurrentLevelManager;
        for (int i = 0; i < level.Alphabets.Length; i++)
        {
            AddCollectedAlphabetUIItem((int)level.Alphabets[i]);
        }
    }

    internal void UpdateDisplay()
    {
        CollectedAlphabetUIItem[] collectedAlphabetUIItems = GetComponentsInChildren<CollectedAlphabetUIItem>();
        if (collectedAlphabetUIItems.Length > 0 )
        {
            for (int i = 0; i < collectedAlphabetUIItems.Length; i++)
            {
                collectedAlphabetUIItems[i].SetTxtAmount();
            }
        }
    }

    private void AddCollectedAlphabetUIItem(int id)
    {
        CollectedAlphabetUIItem newCollectedAlphabet = Instantiate(collectedAlphabetUI, transform);
        newCollectedAlphabet.Init(id);
        newCollectedAlphabet.SetTxtAmount();
    }

    internal void ClearChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
