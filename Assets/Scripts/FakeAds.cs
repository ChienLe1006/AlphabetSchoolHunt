using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FakeAds : MonoBehaviour
{
    [SerializeField] private Text adsType;
    [SerializeField] private Image countdown;
    [SerializeField] private GameObject closeBtn;
    [SerializeField] private Button openAdsBtn;
    [SerializeField] private GameObject banner;

    private void Start()
    {
        closeBtn.GetComponent<Button>().onClick.AddListener(AdsFakeManager.Instance.Close);
        openAdsBtn.onClick.AddListener(OpenAdsUrl);
    }

    private void OnEnable()
    {
        countdown.fillAmount = 1;
        closeBtn.SetActive(false);
        banner.SetActive(false);
        countdown.DOFillAmount(0, 5f).SetEase(Ease.Linear).SetUpdate(true).OnComplete(() => closeBtn.SetActive(true));
    }

    private void OnDisable()
    {
        banner.SetActive(true);
    }

    public void ShowAdsRewarded()
    {
        gameObject.SetActive(true);
        adsType.text = "FAKE REWARDED ADS";
    }

    public void ShowAdsInter()
    {
        gameObject.SetActive(true);
        adsType.text = "FAKE INTER ADS";
    }

    private void OpenAdsUrl()
    {
        Application.OpenURL("https://1bstudios.com/");
    }
}
