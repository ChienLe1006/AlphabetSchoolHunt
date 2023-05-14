using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    [SerializeField] private PopupTrashCatcher popup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        popup.CatchPaper(this.transform);
    }
}
