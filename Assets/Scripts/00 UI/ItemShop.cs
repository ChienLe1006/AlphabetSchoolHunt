using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IItemBuyableShop
{
    void OnClickBtnView();
    void OnClickBtnMoney();
    void OnClickBtnVideo();
    void OnClickBtnSpin();
    void OnClickBtnEquip();
    void OnVideoDone(bool success);
}

public class ItemShop : MonoBehaviour, IItemBuyableShop
{
    [SerializeField] protected Image icon, select, lockImg, btnEquipImg;
    [SerializeField] protected Button btnView, btnMoney, btnAds, btnSpin, btnEquip;
    [SerializeField] protected Text txtMoney, txtAds;
    [SerializeField] protected GameObject btn;
    [SerializeField] protected Sprite equip, equiped;
    protected int _id;
    protected PlayerDataManager playerData;
    protected DataShopElement dataShopElement;
    protected DataShopSkinCharacter dataShopSkin;

    private void Start()
    {
        playerData = PlayerDataManager.Instance;
        dataShopElement = playerData.DataShopElement;
        dataShopSkin = playerData.DataShopSkinCharacter;

        btnView.onClick.AddListener(OnClickBtnView);
        btnMoney.onClick.AddListener(OnClickBtnMoney);
        btnAds.onClick.AddListener(OnClickBtnVideo);
        btnSpin.onClick.AddListener(OnClickBtnSpin);
        btnEquip.onClick.AddListener(OnClickBtnEquip);
    }

    internal void EnableSelectImg()
    {
        select.gameObject.SetActive(true);
    }

    internal void DisableSelectImg()
    {
        select.gameObject.SetActive(false);
    }

    public void NoInternet()
    {
        PopupDialogCanvas.Instance.Show("No Internet!");
    }

    public void ShowFail()
    {
        PopupDialogCanvas.Instance.Show("No Video!");
    }

    internal virtual void Init(int id)
    {

    }

    public virtual void OnClickBtnView()
    {
        SoundManager.Instance.PlaySoundButton();
    }

    public virtual void OnClickBtnMoney()
    {
        SoundManager.Instance.PlaySoundButton();
    }

    public virtual void OnClickBtnVideo()
    {
        
    }

    public virtual void OnClickBtnSpin()
    {
        SoundManager.Instance.PlaySoundButton();
    }

    public virtual void OnClickBtnEquip()
    {
        SoundManager.Instance.PlaySoundButton();
    }

    public virtual void OnVideoDone(bool success)
    {
        
    }
}
