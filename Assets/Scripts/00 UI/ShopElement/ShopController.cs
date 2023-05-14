using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : UICanvas
{
    [SerializeField] private Button btnSkinTab, btnSkillTab;
    [SerializeField] private Sprite choose, normal;
    [SerializeField] private GameObject skinTab, elementTab;
    [SerializeField] private GameObject objNotEnoughtGold;
    [SerializeField] private SkinCharaterController skinCharaterController;
    public SkinCharaterController SkinCharaterController => skinCharaterController;

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);
        if (isShow)
        {
            Init();
        }
    }

    private void Init()
    {
        skinCharaterController.Init();
        ActiveSkinTab();
    }

    public override void OnBackPressed()
    {
        base.OnBackPressed();
        SoundManager.Instance.PlaySoundButton();
    }

    public void ActiveNotiNotEnoughtGold(Transform trans)
    {
        SetParentNotiNotEnoughtGold(trans);
        objNotEnoughtGold.SetActive(true);
    }

    public void DisableNotiNotEnoughtGold()
    {
        objNotEnoughtGold.SetActive(false);
    }

    private void SetParentNotiNotEnoughtGold(Transform trans)
    {
        objNotEnoughtGold.transform.SetParent(trans);
        objNotEnoughtGold.transform.localPosition = Vector3.zero;
    }

    public void ActiveSkinTab()
    {
        skinTab.SetActive(true);
        elementTab.SetActive(false);
        btnSkinTab.GetComponent<Image>().sprite = choose;
        btnSkillTab.GetComponent<Image>().sprite = normal;
    }

    public void ActiveSkillTab()
    {
        elementTab.SetActive(true);
        skinTab.SetActive(false);      
        btnSkillTab.GetComponent<Image>().sprite = choose;
        btnSkinTab.GetComponent<Image>().sprite = normal;
    }
}
