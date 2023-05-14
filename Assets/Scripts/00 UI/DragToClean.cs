using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToClean : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private Transform waterImg;
    [SerializeField] private Transform moneyTxt;
    [SerializeField] private GameObject broom;
    private bool canClean = true;

    internal void Setup()
    {
        canClean = true;
        waterImg.localScale = Vector3.one;
        broom.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponentInParent<PopupWaterCleaning>().TurnOffTuts();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        broom.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        var popup = GetComponentInParent<PopupWaterCleaning>();
        if (!popup.AutoClean)
        {
            if (canClean)
            {
                broom.SetActive(true);
                broom.transform.position = eventData.position;
                waterImg.localScale -= Vector3.one * Time.deltaTime * 0.2f;

                if (waterImg.localScale.x < 0f)
                {
                    canClean = false;
                    moneyTxt.gameObject.SetActive(true);
                    moneyTxt.DOLocalMoveY(moneyTxt.localPosition.y + 200, 1).OnComplete(() =>
                    {
                        popup.DoneClean();
                    });
                }
            }
        }
    }
}
