using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
[Singleton("Popup/WaitingCanvas", true)]
public class WaitingCanvas : Singleton<WaitingCanvas>
{
    public bool isShow { get; set; }
    [SerializeField]
    private Text text;
    private IDisposable timeOutDisposable;
    public override void Awake()
    {
        base.Awake();

        if (!isDestroy)
        {
            Instance.Init();
        }
        if (!isShow)
        {
            Hide();
        }
    }

    public void Show(string txt, Action timeoutAction = null, float timeout = 15)
    {
        isShow = true;
        if (timeOutDisposable != null)
        {
            timeOutDisposable.Dispose();
        }
        text.text = txt;
        gameObject.SetActive(true);
        timeOutDisposable = Observable.Timer(TimeSpan.FromSeconds(timeout)).Subscribe(i =>
        {
            Hide();
            if (timeoutAction != null)
            {
                timeoutAction();
            }
        });
    }

    public void Hide()
    {

        if (gameObject == null)
            return;
        gameObject.SetActive(false);
        isShow = false;
        if (timeOutDisposable != null)
        {
            timeOutDisposable.Dispose();
        }
    }
}
