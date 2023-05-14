using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private AsyncOperation asyncOperation;

    private void Start()
    {
        slider.DOValue(1, 2f).SetEase(Ease.InOutQuad).OnComplete(ShowAppOpenAd);
        asyncOperation = SceneManager.LoadSceneAsync(1);
        asyncOperation.allowSceneActivation = false;       
    }

    private void ShowAppOpenAd()
    {
        AppOpenAdManager.Instance.ShowAdIfAvailable(() =>
        {
            asyncOperation.allowSceneActivation = true;
        });
    }
}
