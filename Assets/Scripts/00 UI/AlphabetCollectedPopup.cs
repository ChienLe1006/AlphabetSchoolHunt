using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AlphabetCollectedPopup : MonoBehaviour
{
    [SerializeField] private Image icon;

    internal void Init(int id)
    {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
        icon.sprite = PlayerDataManager.Instance.DataTexture.GetIconAlphabet(id);
        float x = Random.Range(0, 2);
        float y = Random.Range(3, 5);
        Vector3 targetPos = transform.position + new Vector3(x, y, 0);
        transform.DOMove(targetPos, 0.5f).OnComplete(() => gameObject.Despawn());
    }
}
