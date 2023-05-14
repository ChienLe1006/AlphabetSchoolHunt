using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingAlphabet : MonoBehaviour
{
    [SerializeField] private Alphabet alphabetName;
    public Alphabet AlphabetName => alphabetName;

    internal void ReturnToNpc(Vector3 npcTrans, Character player, Action callback)
    {
        float x = UnityEngine.Random.Range(0, 2);
        float y = UnityEngine.Random.Range(3, 5);
        Vector3 targetPos = transform.position + new Vector3(x, y, 0);
        transform.LookAt(GameManager.Instance.MainCamera.transform.position);
        transform.DOMove(targetPos, 0.2f).OnComplete(() =>
        {
            transform.DOScale(0, 0.5f).SetDelay(0.2f);
            transform.DOJump(npcTrans, 5, 1, 0.5f).SetDelay(0.25f).OnComplete(() =>
            {
                callback?.Invoke();
                gameObject.Despawn();
                player.AlphabetHolder.Rerange();
            });
        });
    }
}
