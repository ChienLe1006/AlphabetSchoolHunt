using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterUnlockUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    public int Id { get; private set; } 

    internal void Init(int idAlphabet)
    {
        icon.sprite = PlayerDataManager.Instance.DataTexture.GetIconAlphabet(idAlphabet);
        Id = idAlphabet;
    }

    internal void Appear(Action callback = null)
    {
        icon.DOFade(1, 1.5f).SetEase(Ease.Linear).OnComplete(() => callback?.Invoke());
    }
 }
