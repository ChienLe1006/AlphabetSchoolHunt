using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UiTop : MonoBehaviour
{
    [SerializeField] private Text txtGold, txtBag;
    [SerializeField] private Transform transCoin, transBag;
    [SerializeField] private List<Image> listImgKeys;
    [SerializeField] private RectTransform objKey;

    private void Start()
    {
        GameManager.Instance.PlayerDataManager.actionUITop += UpdateUIHaveAnim;
        SetTextBag();
        UpdateUiGold(0);
        //UpdateLayoutKey();
    }

    public void BackToHome()
    {
        GameManager.Instance.GameStateController.ChangeState(GameState.LOBBY);
        GameManager.Instance.LoadLevelScene(GameManager.Instance.CurrentLevel + 1);
    }

    private void OnDestroy()
    {
        GameManager.Instance.PlayerDataManager.actionUITop -= UpdateUIHaveAnim;
    }

    private void UpdateUiGold(int _type)
    {
        switch (_type)
        {
            case 0:
                txtGold.text = GameManager.Instance.Profile.GetGold().ToString();
                break;

        }

    }
    private void UpdateUIHaveAnim(TypeItemAnim _type)
    {
        switch (_type)
        {
            case TypeItemAnim.Coin:
                {
                    SetTextCoin(GameManager.Instance.Profile.GetGold());
                    PlayAnimationScale(transCoin);
                    break;
                }
            case TypeItemAnim.Key:
                {
                    //UpdateLayoutKey(true);
                    break;
                }
            case TypeItemAnim.Bag:
                {
                    SetTextBag();
                    PlayAnimationScale(transBag);
                    break;
                }
        }
    }

    //private void UpdateLayoutKey(bool isAni = false)
    //{
    //    for (int i = 0; i < listImgKeys.Count; i++)
    //    {
    //        listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(false);
    //    }

    //    int countKey = GameManager.Instance.Profile.GetKey() > 3 ? 3 : GameManager.Instance.Profile.GetKey();
    //    for (int i = 0; i < countKey; i++)
    //    {
    //        listImgKeys[i].sprite = GameManager.Instance.PlayerDataManager.DataTexture.GetIconKey(true);
    //    }

    //    if (isAni)
    //        objKey.DOAnchorPos3DY(-50, 1f).OnComplete(() =>
    //        {
    //            objKey.DOAnchorPos3DY(135, 1f).SetDelay(1f);
    //        });
    //}

    private Tweener tweenCoin;
    private int tmpCoin;
    private void SetTextCoin(int _coin)
    {
        tweenCoin = tweenCoin ?? DOTween.To(() => tmpCoin, x =>
        {
            tmpCoin = x;
            txtGold.text = tmpCoin.ToString();
        }, _coin, 0.3f).SetAutoKill(false).OnComplete(() =>
        {
            tmpCoin = GameManager.Instance.Profile.GetGold();
            txtGold.text = tmpCoin.ToString();
        });
        tweenCoin.ChangeStartValue(tmpCoin);
        tweenCoin.ChangeEndValue(_coin);
        tweenCoin.Play();
    }

    private void PlayAnimationScale(Transform transformScale)
    {
        transformScale.DOScale(1.4f, 0.1f).OnComplete(() => { transformScale.DOScale(1, 0.05f); });
    }

    private void SetTextBag()
    {
        var playerData = PlayerDataManager.Instance;
        if (playerData.CurrentAlphabetAmountInBag == playerData.BagCapacity)
        {
            txtBag.text = "Maxed!";
        }
        else txtBag.text = $"{playerData.CurrentAlphabetAmountInBag}/{playerData.BagCapacity}";
    }
}

