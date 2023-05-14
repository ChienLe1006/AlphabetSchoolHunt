using System.Collections;
//using com.adjust.sdk;
using Studio1BAds.Config;
#if MEDIATION_IRONSOURCE
namespace Studio1BAds.Ads
{
    public class IronSourceAdsMediation : BaseAdsMediation
    {
        private bool isBannerLoaded;
        private BannerPosition currentBannerPosition;
        public override bool IsInterstitialAvailable
        {
            get
            {
#if UNITY_EDITOR
                return UnityEngine.Random.Range(0, 2) == 1;
#else
				return IronSource.Agent.isInterstitialReady();
#endif

            }
        }

        public override bool IsRewardedAdsAvailable
        {
            get
            {
#if UNITY_EDITOR
                return UnityEngine.Random.Range(0, 2) == 1;
#else
				return IronSource.Agent.isRewardedVideoAvailable();
#endif
            }
        }

        public override void HideBanner()
        {
            IronSource.Agent.hideBanner();
        }

        private string IronSourceDevKey
        {
            get
            {
#if UNITY_ANDROID
                return Studio1BAppSettingsLoader.AppSettings.AndroidIronSourceAppKey;
#else
                return Studio1BAppSettingsLoader.AppSettings.IOSIronSourceAppKey;
#endif
            }
        }

        public override bool IsRewardedInterstitialAvailable
        {
            get
            {
                return false;
            }
        }

        public override void Init()
        {
            IronSource.Agent.shouldTrackNetworkState(true);
            IronSource.Agent.init(IronSourceDevKey);

            IronSourceInterstitialEvents.onAdClosedEvent += IronSourceEvents_onInterstitialAdClosedEvent;
            IronSourceInterstitialEvents.onAdLoadFailedEvent += IronSourceEvents_onInterstitialAdLoadFailedEvent;
            IronSourceInterstitialEvents.onAdShowFailedEvent += IronSourceEvents_onInterstitialAdShowFailedEvent;
            IronSourceInterstitialEvents.onAdReadyEvent += IronSourceEvents_onInterstitialAdReadyEvent;

            IronSourceRewardedVideoEvents.onAdClosedEvent += IronSourceRewardedVideoEvents_onAdClosedEvent;
            IronSourceRewardedVideoEvents.onAdShowFailedEvent += IronSourceRewardedVideoEvents_onAdShowFailedEvent;
            IronSourceRewardedVideoEvents.onAdAvailableEvent += IronSourceRewardedVideoEvents_onAdAvailableEvent;
            IronSourceRewardedVideoEvents.onAdUnavailableEvent += IronSourceRewardedVideoEvents_onAdUnavailableEvent;
            IronSourceRewardedVideoEvents.onAdRewardedEvent += IronSourceRewardedVideoEvents_onAdRewardedEvent;

            IronSourceEvents.onBannerAdLoadedEvent += IronSourceEvents_onBannerAdLoadedEvent;

            IronSourceEvents.onImpressionDataReadyEvent += IronSourceEvents_onImpressionDataReadyEvent;
        }

        private void IronSourceRewardedVideoEvents_onAdUnavailableEvent()
        {
            //InvokeOnInterstitialLoadFailed();
        }

        private void IronSourceEvents_onImpressionDataReadyEvent(IronSourceImpressionData impressionData)
        {
            //AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceIronSource);
            //double revenue = impressionData.revenue.HasValue ? impressionData.revenue.Value : 0;
            //adjustAdRevenue.setRevenue(revenue, "USD");
            // optional fields
            //adjustAdRevenue.setAdRevenueNetwork(impressionData.adNetwork);
            //adjustAdRevenue.setAdRevenueUnit(impressionData.adUnit);
            //adjustAdRevenue.setAdRevenuePlacement(impressionData.placement);
            // track Adjust ad revenue
            //Adjust.trackAdRevenue(adjustAdRevenue);
            //string lowerAdUnit = impressionData.adUnit.ToLower();
            //switch (lowerAdUnit)
            //{
            //    case "banner":
            //        FirebaseAdsRevenueLogger.AddBannerRevenue((float)revenue);
            //        break;
            //    case "interstitial":
            //        FirebaseAdsRevenueLogger.AddInterRevenue((float)revenue);
            //        break;
            //    case "rewarded_video":
            //        FirebaseAdsRevenueLogger.AddRewardedRevenue((float)revenue);
            //        break;

            //}
        }

        private void IronSourceEvents_onBannerAdLoadedEvent()
        {
        }

        private void IronSourceRewardedVideoEvents_onAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo adInfo)
        {
            InvokeOnRewardedSuccess();
        }

        private void IronSourceRewardedVideoEvents_onAdAvailableEvent(IronSourceAdInfo adInfo)
        {
            InvokeOnRewardedLoaded(adInfo.adNetwork);
        }

        private void IronSourceRewardedVideoEvents_onAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            InvokeOnRewardedDisplayFailed();
        }

        private void IronSourceRewardedVideoEvents_onAdClosedEvent(IronSourceAdInfo adInfo)
        {
            InvokeOnRewardedClosed();
        }

        private void IronSourceEvents_onInterstitialAdReadyEvent(IronSourceAdInfo adInfo)
        {
            InvokeOnInterstitialLoaded(adInfo.adNetwork);
        }

        private void IronSourceEvents_onInterstitialAdShowFailedEvent(IronSourceError error, IronSourceAdInfo adInfo)
        {
            InvokeOnInterstitialDisplayFailed();
        }

        private void IronSourceEvents_onInterstitialAdLoadFailedEvent(IronSourceError error)
        {
            InvokeOnInterstitialLoadFailed();
        }

        private void IronSourceEvents_onInterstitialAdClosedEvent(IronSourceAdInfo adInfo)
        {
            InvokeOnInterstitialClosed();
        }

        public override void InitInterstitial()
        {
            IronSource.Agent.init(IronSourceDevKey, IronSourceAdUnits.INTERSTITIAL);
        }

        public override void InitRewardedAds()
        {
            IronSource.Agent.init(IronSourceDevKey, IronSourceAdUnits.REWARDED_VIDEO);
        }

        public override void LoadInterstitial()
        {
            IronSource.Agent.loadInterstitial();
        }

        public override void LoadRewardedAds()
        {
        }

        public override void ShowBanner(BannerPosition position)
        {
            if (isBannerLoaded)
            {
                if (position != currentBannerPosition)
                {
                    IronSource.Agent.destroyBanner();
                    IronSourceBannerPosition ironsourcePosition = position == BannerPosition.Top ? IronSourceBannerPosition.TOP : IronSourceBannerPosition.BOTTOM;
                    AdsMediationController.Instance.Enqueue(LoadBannerDelay(ironsourcePosition));
                }
            }
            else
            {
                IronSource.Agent.init(IronSourceDevKey, IronSourceAdUnits.BANNER);
                IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, position == BannerPosition.Top ? IronSourceBannerPosition.TOP : IronSourceBannerPosition.BOTTOM);
                isBannerLoaded = true;
            }
            currentBannerPosition = position;
            IronSource.Agent.displayBanner();
        }

        private IEnumerator LoadBannerDelay(IronSourceBannerPosition position)
        {
            yield return null;
            IronSource.Agent.loadBanner(IronSourceBannerSize.BANNER, position);
        }

        public override void ShowInterstitial(string placement)
        {
#if UNITY_EDITOR
            InvokeOnInterstitialClosed();
#endif
            IronSource.Agent.showInterstitial(placement);
        }

        public override void ShowRewardedAds(string placement)
        {
#if UNITY_EDITOR
            InvokeOnRewardedSuccess();
            InvokeOnRewardedClosed();
#endif
            IronSource.Agent.showRewardedVideo(placement);
        }

        public override void InitRewardedInterstitialAds()
        {

        }

        public override void LoadRewardedInterstitial()
        {

        }

        public override void ShowRewardedInterstitial(string placement)
        {
            ShowRewardedAds(placement);
        }
    }
}
#endif