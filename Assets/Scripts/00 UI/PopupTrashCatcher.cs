using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PopupTrashCatcher : UICanvas
{
    [SerializeField] private Transform content;
    [SerializeField] private GameObject paper;
    [SerializeField] private Sprite[] paperSprites;

    [SerializeField] private GameObject tuts, money, trashCanDecor;
    private int paperCount;
    private GameObject trashObj;

    internal void Init(GameObject obj)
    {
        content.DOScale(1, 0.4f).SetEase(Ease.OutBack);    
        trashObj = obj;

        Setup();
        GetComponentInChildren<DragToMove>().Setup();
    }

    private void Setup()
    {
        tuts.SetActive(true);
        trashCanDecor.SetActive(true);
        paperCount = 0;
        paper.SetActive(false);
        paper.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    internal void DisableTuts()
    {
        tuts.SetActive(false);
        trashCanDecor.SetActive(false);
    }

    private void OnDisable()
    {
        content.localScale = Vector3.zero;
        content.DOKill();
    }

    private void Update()
    {
        if (paper.transform.position.y < 500)
        {
            if (paperCount == 10)
            {
                GameManager.Instance.CurrentLevelManager.DeleteObject(trashObj);
                ResetPaper();
                OnBackPressed();
            }           
            else SpawnPaper();
        }
    }

    internal void SpawnPaper()
    {
        if (paperCount <= 10)
        {
            paperCount++;
            paper.SetActive(true);
            paper.transform.localPosition = new Vector3(Random.Range(-300, 300), 300, 0);
            paper.GetComponent<Image>().sprite = paperSprites[Random.Range(0, 2)];
        }    
    }

    internal void CatchPaper(Transform pos)
    {
        if (paperCount < 10)
        {
            GameManager.Instance.Profile.AddGold(10);
            money.SetActive(true);
            money.transform.position = pos.position + Vector3.up * 50;
            money.transform.DOLocalMoveY(money.transform.localPosition.y + 50, 0.5f).OnComplete(() => money.SetActive(false));
            SpawnPaper();
        }
        else
        {
            GameManager.Instance.Profile.AddGold(10);
            GameManager.Instance.CurrentLevelManager.DeleteObject(trashObj);
            ResetPaper();
            OnBackPressed();           
        }
    }

    private void ResetPaper()
    {
        paper.transform.position = new Vector3(0f, 900, 0f);
        paper.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        paper.SetActive(false);
    }
}
