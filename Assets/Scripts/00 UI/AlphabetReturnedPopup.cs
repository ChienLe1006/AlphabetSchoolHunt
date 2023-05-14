using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphabetReturnedPopup : MonoBehaviour
{
    [SerializeField] private Image icon;

    internal void Init(int id, Vector3 npcTrans, Action callback)
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        icon.sprite = PlayerDataManager.Instance.DataTexture.GetIconAlphabet(id);
        float x = UnityEngine.Random.Range(0, 2);
        float y = UnityEngine.Random.Range(3, 5);
        Vector3 targetPos = transform.position + new Vector3(x, y, 0);
        transform.DOMove(targetPos, 0.2f).OnComplete(() => 
        {
            transform.DOScale(0, 0.5f).SetDelay(0.2f);
            transform.DOJump(npcTrans, 5, 1, 0.5f).SetDelay(0.25f).OnComplete(() =>
            {
                callback?.Invoke();
                gameObject.Despawn();
            });
        }); 
    }
}
