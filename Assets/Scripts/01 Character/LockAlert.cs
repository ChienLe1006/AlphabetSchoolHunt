using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockAlert : MonoBehaviour
{
    private bool isShowing;

    internal void Init()
    {
        transform.LookAt(GameManager.Instance.MainCamera.transform.position);
        if (isShowing) return;
        isShowing = true;
        gameObject.SetActive(true);
        Invoke("TurnOff", 0.5f);
    }

    private void TurnOff()
    {
        isShowing = false;
        gameObject.SetActive(false);
    }
}
