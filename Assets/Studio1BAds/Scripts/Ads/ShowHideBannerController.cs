using Studio1BAds.Ads;
using System.Threading.Tasks;
using UnityEngine;

public class ShowHideBannerController : MonoBehaviour
{
    [SerializeField]
    private bool showOnEnabled;
    [SerializeField] private GameObject fakeBanner;

    private void OnEnable()
    {
        //if (GameData.RemoveAds) return;
        if (showOnEnabled)
        {
            // await Task.Yield();
            AdsMediationController.Instance.ShowBanner(BannerPosition.Bottom);
        }
        else
        {
            // await Task.Yield();
            AdsMediationController.Instance.HideBanner();
        }
    }

    private void OnDisable()
    {
        //if (GameData.RemoveAds) return;
        if (showOnEnabled)
        {
            AdsMediationController.Instance.HideBanner();
        }
        else
        {
            AdsMediationController.Instance.ShowBanner(BannerPosition.Bottom);
        }
    }
}
