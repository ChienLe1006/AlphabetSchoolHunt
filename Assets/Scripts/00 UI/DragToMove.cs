using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToMove : MonoBehaviour, IPointerMoveHandler, IPointerEnterHandler
{
    [SerializeField] private PopupTrashCatcher popup;
    [SerializeField] private Transform trashCan;
    private bool firstSpawn;

    internal void Setup()
    {
        firstSpawn = false;
        trashCan.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!firstSpawn)
        {
            firstSpawn = true;
            trashCan.gameObject.SetActive(true);
            popup.DisableTuts();
            popup.SpawnPaper();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        trashCan.position = new Vector3(eventData.position.x, 644, trashCan.position.z);
    }
}
