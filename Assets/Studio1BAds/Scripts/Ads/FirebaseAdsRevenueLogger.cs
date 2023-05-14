using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Studio1BAds.Ads
{

    public class FirebaseAdsRevenueLogger
    {

        private const string AD_BANNER_REVENUE = "ad_banner_revenue";
        private const string AD_INTER_REVENUE = "ad_inter_revenue";
        private const string AD_REWARDED_REVENUE = "ad_rewarded_revenue";
        private const string AD_REWARDED_INTER_REVENUE = "ad_rewarded_inter_revenue";
        private const string AD_APP_OPEN_REVENUE = "ad_app_open_revenue";


        private static float BannerRevenue
        {
            get
            {
                return PlayerPrefs.GetFloat(AD_BANNER_REVENUE, 0f);
            }
            set
            {
                PlayerPrefs.SetFloat(AD_BANNER_REVENUE, value);
            }
        }

        private static float InterRevenue
        {
            get
            {
                return PlayerPrefs.GetFloat(AD_INTER_REVENUE, 0f);
            }
            set
            {
                PlayerPrefs.SetFloat(AD_REWARDED_REVENUE, value);
            }
        }

        private static float RewardedRevenue
        {
            get
            {
                return PlayerPrefs.GetFloat(AD_REWARDED_REVENUE, 0f);
            }
            set
            {
                PlayerPrefs.SetFloat(AD_REWARDED_REVENUE, value);
            }
        }

        private static float AppOpenRevenue
        {
            get
            {
                return PlayerPrefs.GetFloat(AD_APP_OPEN_REVENUE, 0f);
            }

            set
            {
                PlayerPrefs.SetFloat(AD_APP_OPEN_REVENUE, value);
            }
        }

        private static float RewardedInterRevenue
        {
            get
            {
                return PlayerPrefs.GetFloat(AD_REWARDED_INTER_REVENUE, 0f);
            }
            set
            {
                PlayerPrefs.SetFloat(AD_REWARDED_INTER_REVENUE, value);
            }
        }

        private static void LogUserProperty(string name, float value)
        {
            Firebase.Analytics.FirebaseAnalytics.SetUserProperty(name, value.ToString());
        }

        public static void AddBannerRevenue(float revenue)
        {
            AdsMediationController.Instance.Enqueue(() => {
                BannerRevenue += revenue;
                LogUserProperty(AD_BANNER_REVENUE, BannerRevenue);
            });
        }

        public static void AddInterRevenue(float revenue)
        {
            AdsMediationController.Instance.Enqueue(() => {
                InterRevenue += revenue;
                LogUserProperty(AD_INTER_REVENUE, InterRevenue);
            });
        }

        public static void AddRewardedRevenue(float revenue)
        {
            AdsMediationController.Instance.Enqueue(() => {
                RewardedRevenue += revenue;
                LogUserProperty(AD_REWARDED_REVENUE, RewardedRevenue);
            });
        }

        public static void AddAppOpenRevenue(float revenue)
        {
            AdsMediationController.Instance.Enqueue(() => {
                AppOpenRevenue += revenue;
                LogUserProperty(AD_APP_OPEN_REVENUE, AppOpenRevenue);
            });
        }

        public static void AddRewardedInterRevenue(float revenue)
        {
            AdsMediationController.Instance.Enqueue(() => {
                RewardedInterRevenue += revenue;
                LogUserProperty(AD_REWARDED_INTER_REVENUE, RewardedInterRevenue);
            });
        }
    }

}