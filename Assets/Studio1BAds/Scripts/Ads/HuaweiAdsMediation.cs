using System;
using System.Collections;
using System.Collections.Generic;
#if MEDIATION_HUAWEI
using HmsPlugin;
using HuaweiMobileServices.Ads;
using UnityEngine;
using WidogameFoundation.Ads;
using WidogameFoundation.Config;

namespace WidogameFoundation.Ads {
	public class HuaweiAdsMediation : BaseAdsMediation {

		private RewardAd rewardAd;
		private RewardAd rewardInterstitialAd;
		private bool isShowingReward;
		private BannerPosition currentBannerPosition;

		public override bool IsInterstitialAvailable {
			get {
				return HMSAdsKitManager.Instance.IsInterstitialAdLoaded;
			}
		}

		public override bool IsRewardedAdsAvailable {
			get {
				return HMSAdsKitManager.Instance.IsRewardedAdLoaded;
			}
		}

		public override bool IsRewardedInterstitialAvailable {
			get {
				return IsRewardedAdsAvailable;
			}
		}

		public override void HideBanner() {

			HMSAdsKitManager.Instance.HideBannerAd();
		}

		public override void Init() {

		}

		public override void InitInterstitial() {
			HMSAdsKitManager.Instance.OnInterstitialAdFailed += InterstitialAdLoadFailed;
			HMSAdsKitManager.Instance.OnInterstitialAdLoaded += InterstitialAdLoaded;
			HMSAdsKitManager.Instance.OnInterstitialAdClosed += InterstitialAdClosed;
		}

		public override void InitRewardedAds() {
			HMSAdsKitManager.Instance.OnRewardedAdFailedToLoad += RewardAdLoadFailed;
			HMSAdsKitManager.Instance.OnRewardedAdLoaded += RewardAdLoaded;
			HMSAdsKitManager.Instance.OnRewarded += RewardAdRewarded;
			HMSAdsKitManager.Instance.OnRewardAdClosed += RewardAdClosed;
		}

		public override void InitRewardedInterstitialAds() {
		}

		public override void LoadInterstitial() {
			HMSAdsKitManager.Instance.LoadInterstitialAd();
		}

		public override void LoadRewardedAds() {
			HMSAdsKitManager.Instance.LoadRewardedAd();
		}

		public override void LoadRewardedInterstitial() {
		}

		public override void ShowBanner(BannerPosition position) {
			if (HMSAdsKitManager.Instance.IsBannerAdLoaded) {
				if (position != currentBannerPosition) {
					HMSAdsKitManager.Instance.DestroyBannerAd();
					HMSAdsKitManager.Instance.LoadBannerAd(position == BannerPosition.Bottom ? HuaweiConstants.UnityBannerAdPositionCode.UnityBannerAdPositionCodeType.POSITION_BOTTOM : HuaweiConstants.UnityBannerAdPositionCode.UnityBannerAdPositionCodeType.POSITION_TOP);
				}
				HMSAdsKitManager.Instance.ShowBannerAd();
			} else {
				HMSAdsKitManager.Instance.LoadBannerAd(position == BannerPosition.Bottom ? HuaweiConstants.UnityBannerAdPositionCode.UnityBannerAdPositionCodeType.POSITION_BOTTOM : HuaweiConstants.UnityBannerAdPositionCode.UnityBannerAdPositionCodeType.POSITION_TOP);
				HMSAdsKitManager.Instance.ShowBannerAd();
			}
			currentBannerPosition = position;
		}

		public override void ShowInterstitial(string placement) {
			HMSAdsKitManager.Instance.ShowInterstitialAd();
		}

		public override void ShowRewardedAds(string placement) {
			isShowingReward = true;
			HMSAdsKitManager.Instance.ShowRewardedAd();
		}

		public override void ShowRewardedInterstitial(string placement) {
			isShowingReward = false;
			HMSAdsKitManager.Instance.ShowRewardedAd();
		}

		private void InterstitialAdLoadFailed(int error) {
			InvokeOnInterstitialLoadFailed();
		}

		private void InterstitialAdLoaded() {
			InvokeOnInterstitialLoaded(WidogameConstants.AD_NETWORK_NAME_NONE);
		}

		private void InterstitialAdClosed() {
			InvokeOnInterstitialClosed();
		}

		private void RewardAdLoadFailed(int error) {
			InvokeOnRewardedLoadFailed();
		}

		private void RewardAdLoaded() {
			InvokeOnRewardedLoaded(WidogameConstants.AD_NETWORK_NAME_NONE);
		}

		private void RewardAdDisplayFailed() {
			if (isShowingReward) {
				InvokeOnRewardedDisplayFailed();
			} else {
				InvokeOnRewardedInterstitialDisplayFailed();
			}
		}

		private void RewardAdRewarded(Reward reward) {
			if (isShowingReward) {
				InvokeOnRewardedSuccess();
			} else {
				InvokeOnRewardedInterstitialSuccess();
			}
		}

		private void RewardAdClosed() {
			if (isShowingReward) {
				InvokeOnRewardedClosed();
			} else {
				InvokeOnRewardedInterstitialClosed();
			}
		}

		private void RewardInterstitialLoadFailed() {
			InvokeOnRewardedInterstitialLoadFailed();
		}

		private void RewardInterstitialLoaded() {
			InvokeOnRewardedInterstitialLoaded();
		}

		private void RewardInterstitialAdDisplayFailed() {
			InvokeOnRewardedInterstitialDisplayFailed();
		}

		private void RewardInterstitialAdRewarded() {
			InvokeOnRewardedInterstitialSuccess();
		}

		private void RewardInterstitialClosed() {
			InvokeOnRewardedInterstitialClosed();
		}
	}

}
#endif