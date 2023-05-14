using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Studio1BAds.Config
{
    public class Studio1BAppSettings : ScriptableObject
    {
        public static readonly string STUDIO1B_CONFIGURATION_ASSET_PATH = Path.Combine(Studio1BConstants.STUDIO1B_RESOURCES_PATH, Studio1BConstants.STUDIO1B_APP_SETTINGS_NAME + ".asset");

        [Header("Admob")]
        public string AndroidAdmobAppOpenId;
        public string[] AndroidAdmobAppOpenWaterFall;
        public string AndroidAdmobBannerId;
        public string AndroidAdmobInterstitialId;
        public string AndroidAdmobRewardedId;
        public string AndroidAdmobRewardedInterstitialId;
        public string IOSAdmobAppOpenId;
        public string[] IOSAdmobAppOpenWaterFall;
        public string IOSAdmobBannerId;
        public string IOSAdmobInterstitialId;
        public string IOSAdmobRewardedId;
        public string IOSAdmobRewardedInterstitialId;

        [Header("Max Mediation")]
        public string MaxSdkKey;
        public string AndroidMaxBannerAdUnit;
        public string AndroidMaxInterstitialAdUnit;
        public string AndroidMaxRewardedAdUnit;
        public string IOSMaxBannerAdUnit;
        public string IOSMaxInterstitialAdUnit;
        public string IOSMaxRewardedAdUnit;

        [Header("IronSource Mediation")]
        public string AndroidIronSourceAppKey;
        public string IOSIronSourceAppKey;
        public string AndroidIronSourceDevKey;
        public string IOSIronSourceDevKey;

        [Header("Huawei Ads")]
        public string AndroidHuaweiBannerAdUnit;
        public string AndroidHuaweiInterstitialAdUnit;
        public string AndroidHuaweiRewardedAdUnit;
        public string AndroidHuaweiRewardedInterstitialAdUnit;
    }
}
