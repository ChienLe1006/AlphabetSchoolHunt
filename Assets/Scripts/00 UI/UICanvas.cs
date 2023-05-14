//using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : MonoBehaviour
{
    public bool isDestroyWhenClosed = false;
    public bool isDisableWhenClosed = true;
    public bool isAutoSetSortingLayer = true;


    protected Stack<Action> actionOpen
    {
        get
        {
            if (_actionOpen == null)
                _actionOpen = new Stack<Action>();
            return _actionOpen;
        }
    }
    protected Stack<Action> actionClose
    {
        get
        {
            if (_actionClose == null)
                _actionClose = new Stack<Action>();
            return _actionClose;
        }
    }

    protected bool isShow = false;

    private RectTransform _rect;
    private Stack<Action> _actionOpen;
    private Stack<Action> _actionClose;
    private RectTransform Rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }

            return _rect;
        }
    }
    protected virtual void Awake()
    {

    }
    public bool IsShow
    {
        get
        {
            return isShow;
        }
    }
    public void SetActionClose(Action _action)
    {
        if (_action != null)
            actionClose.Push(_action);
    }
    public void SetActionOpen(Action _action)
    {
        if (_action != null)
            actionOpen.Push(_action);
    }
    public virtual void Show(bool _isShown, bool isHideMain = true)
    {
        if (isShow == _isShown)
        {

            if (isShow)
            {
                if (isAutoSetSortingLayer)
                {
                    Rect.SetAsLastSibling();
                }
            }
            return;
        }
        isShow = _isShown;
        if (isShow)
        {
            if (isAutoSetSortingLayer)
            {
                Rect.SetAsLastSibling();
            }
            //if (isHideMain)
            //    GameManager.Instance.PushStack(this);
            //else
            //    GameManager.Instance.uiStack.Push(this);

            gameObject.SetActive(true);
            if (actionOpen.Count > 0)
            {
                actionOpen.Pop()();
            }

            //SoundManager.Instance.PlaySoundPopup();
        }
        else
        {
            //GameManager.Instance.PopStack();
            if (actionClose.Count > 0)
            {
                actionClose.Pop()();
            }
            if (isDisableWhenClosed)
            {
                gameObject.SetActive(false);
            }
            else if (isDestroyWhenClosed)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }

        }

    }
    public virtual void OnBackPressed()
    {
        Show(false);
        //SoundManager.Instance.PlaySoundButton();
        //if (GameManager.Instance.currentScreen == TypeScreenGame.LOBBY)
        //    LobbyController.Instance.CheckQueue();
    }
}
