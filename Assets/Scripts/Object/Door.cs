using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform doorLeft, doorRight;

    internal void Open()
    {
        doorLeft.DORotate(new Vector3(0, 135 + transform.eulerAngles.y, 0), 1f).SetEase(Ease.OutBounce);
        doorRight.DORotate(new Vector3(0, -135 + transform.eulerAngles.y, 0), 1f).SetEase(Ease.OutBounce);
    }
}
