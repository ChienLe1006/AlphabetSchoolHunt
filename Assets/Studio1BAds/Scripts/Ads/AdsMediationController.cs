using Studio1BAds.Ads;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class AdsMediationController : MonoBehaviour
{
    private static AdsMediationController _instance;
    private static bool isFullScreenAdsShowing;
    private static long lastFullscreenAdsShown;
    public static AdsMediationController Instance
    {
        get => _instance;
    }

    private BaseAdsMediation adsMediation;
#if MEDIATION_ADMOB_RI
    private BaseAdsMediation riAdsMediation;
#endif

    private int bannerShowingBalance;
    private UnityAction<bool> onIntersititalFinished;
    private int interstitialRetryAttempt;
    private string loadedInterstitialNetwork = Studio1BAds.Studio1BConstants.AD_NETWORK_NAME_NONE;

    private bool gotRewarded;
    private UnityAction onRewardedShowFailed;
    private UnityAction<bool> onRewardedFinished;
    private int rewardedRetryAttempt;
    private string loadedRewardedNetwork = Studio1BAds.Studio1BConstants.AD_NETWORK_NAME_NONE;
    private Action<bool> _onRewardedReady;

    private UnityAction onRewardedInterstitialShowFailed;
    private UnityAction<bool> onRewardedInterstitialFinished;
    private int rewardedInterstitialRetryAttempt;

    private readonly Queue<Action> executionQueue = new Queue<Action>();
    private Thread mainThread;

    public static bool IsFullscreenAdsShowing
    {
        get
        {
            return isFullScreenAdsShowing;
        }
    }

    public static long LastFullscreenAdsShown
    {
        get
        {
            return lastFullscreenAdsShown;
        }
    }

    public event Action<bool> OnRewarededReady
    {
        add
        {
            _onRewardedReady += value;
        }
        remove
        {
            _onRewardedReady -= value;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void LoadAppOpenAd()
    {
        AppOpenAdManager.Instance.LoadAd(ScreenOrientation.Portrait);
    }

    private void Awake()
    {
        _instance = this;
        _instance.Init();
        InitInterstitial();
        InitRewardedAds();
        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        mainThread = Thread.CurrentThread;
    }

    private void Update()
    {
        lock (executionQueue)
        {
            while (executionQueue.Count > 0)
            {
                executionQueue.Dequeue().Invoke();
            }
        }
    }
#if MEDIATION_IRONSOURCE
    private void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }
#endif

    public void Enqueue(Action act)
    {
        Enqueue(ActionWrapper(act));
    }

    public void Enqueue(IEnumerator action)
    {
        lock (executionQueue)
        {
            executionQueue.Enqueue(() => {
                StartCoroutine(action);
            });
        }
    }

    private IEnumerator ActionWrapper(Action act)
    {
        act();
        yield return null;
    }

    private void Init()
    {
#if MEDIATION_IRONSOURCE
        adsMediation = new IronSourceAdsMediation();
#elif MEDIATION_MAX
        adsMediation = new MaxAdsMediation();
#elif MEDIATION_ADMOB
        adsMediation = new AdmobAdsMediation();
#elif MEDIATION_HUAWEI
        adsMediation = new HuaweiAdsMediation();
#endif
        adsMediation.Init();
        adsMediation.OnInterstitialClosed += AdsMediation_OnInterstitialClosed;
        adsMediation.OnInterstitialDisplayFailed += AdsMediation_OnInterstitialDisplayFailed;
        adsMediation.OnInterstitialLoadFailed += AdsMediation_OnInterstitialLoadFailed;
        adsMediation.OnInterstitialLoaded += AdsMediation_OnInterstitialLoaded;

        adsMediation.OnRewardedClosed += AdsMediation_OnRewardedClosed;
        adsMediation.OnRewardedDisplayFailed += AdsMediation_OnRewardedDisplayFailed;
        adsMediation.OnRewardedLoaded += AdsMediation_OnRewardedLoaded;
        adsMediation.OnRewardedLoadFailed += AdsMediation_OnRewardedLoadFailed;
        adsMediation.OnRewardedSuccess += AdsMediation_OnRewardedSuccess;

#if MEDIATION_ADMOB_RI
        riAdsMediation = new AdmobAdsMediation();
        riAdsMediation.Init();
        riAdsMediation.OnRewardedInterstitialClosed += AdsMediation_OnRewardedInterstitialClosed;
		riAdsMediation.OnRewardedInterstitialDisplayFailed += AdsMediation_OnRewardedInterstitialDisplayFailed;
		riAdsMediation.OnRewardedInterstitialLoaded += AdsMediation_OnRewardedInterstitialLoaded;
		riAdsMediation.OnRewardedInterstitialLoadFailed += AdsMediation_OnRewardedInterstitialLoadFailed;
		riAdsMediation.OnRewardedInterstitialSuccess += AdsMediation_OnRewardedInterstitialSuccess;
#else
        adsMediation.OnRewardedInterstitialClosed += AdsMediation_OnRewardedInterstitialClosed;
        adsMediation.OnRewardedInterstitialDisplayFailed += AdsMediation_OnRewardedInterstitialDisplayFailed;
        adsMediation.OnRewardedInterstitialLoaded += AdsMediation_OnRewardedInterstitialLoaded;
        adsMediation.OnRewardedInterstitialLoadFailed += AdsMediation_OnRewardedInterstitialLoadFailed;
        adsMediation.OnRewardedInterstitialSuccess += AdsMediation_OnRewardedInterstitialSuccess;
#endif
    }

    private void AdsMediation_OnRewardedInterstitialClosed()
    {
        MarkLastFullscreenAdsShown();
        LoadRewardedInterstitialAds();
        if (mainThread != Thread.CurrentThread)
        {
            Enqueue(InvokeRewardedInterstitialClosedWithDelay());
        }
        else
        {
            StartCoroutine(InvokeRewardedInterstitialClosedWithDelay());
        }
    }

    private void AdsMediation_OnRewardedInterstitialDisplayFailed()
    {
        MarkLastFullscreenAdsShown();
        if (mainThread != Thread.CurrentThread)
        {
            Enqueue(() => onRewardedInterstitialShowFailed?.Invoke());
        }
        else
        {
            onRewardedInterstitialShowFailed?.Invoke();
        }
        isFullScreenAdsShowing = false;
    }

    private void AdsMediation_OnRewardedInterstitialLoaded()
    {
        rewardedInterstitialRetryAttempt = 0;
    }

    private void AdsMediation_OnRewardedInterstitialLoadFailed()
    {
        rewardedInterstitialRetryAttempt++;
        int retryDelay = GetRetryDelayForCurrentAttempt(rewardedInterstitialRetryAttempt);
        if (mainThread != Thread.CurrentThread)
        {
            Enqueue(LoadRewardedInterstitialWithDelay(retryDelay));
        }
        else
        {
            StartCoroutine(LoadRewardedInterstitialWithDelay(retryDelay));
        }
    }

    private void AdsMediation_OnRewardedInterstitialSuccess()
    {
        gotRewarded = true;
    }

    private IEnumerator InvokeRewardedInterstitialClosedWithDelay()
    {
        yield return null;
        onRewardedInterstitialFinished?.Invoke(gotRewarded);
        isFullScreenAdsShowing = false;
    }

    private void AdsMediation_OnRewardedSuccess()
    {
        gotRewarded = true;
    }

    private void AdsMediation_OnRewardedLoadFailed()
    {
        rewardedRetryAttempt++;
        int retryDelay = GetRetryDelayForCurrentAttempt(rewardedRetryAttempt);
        if (mainThread != Thread.CurrentThread)
        {
            Enqueue(IELoadRewardedAdsDelay(retryDelay));
        }
        else
        {
            StartCoroutine(IELoadRewardedAdsDelay(retryDelay));
        }
    }

    private void AdsMediation_OnRewardedLoaded(string networkName)
    {
        rewardedRetryAttempt = 0;
        _onRewardedReady?.Invoke(true);
    }

    private void AdsMediation_OnRewardedDisplayFailed()
    {
        MarkLastFullscreenAdsShown();
        loadedRewardedNetwork = Studio1BAds.Studio1BConstants.AD_NETWORK_NAME_NONE;
        LoadRewardedAds();
        if (mainThread != Thread.CurrentThread)
        {
            Enqueue(() => onRewardedShowFailed?.Invoke());
        }
        else
        {
            onRewardedShowFailed?.Invoke();
        }
        isFullScreenAdsShowing = false;
    }

    private void AdsMediation_OnRewardedClosed()
    {
        MarkLastFullscreenAdsShown();
        loadedRewardedNetwork = Studio1BAds.Studio1BConstants.AD_NETWORK_NAME_NONE;
        LoadRewardedAds();
        if (mainThread != Thread.CurrentThread)
        {
            Enqueue(InvokeRewardedClosedWithDelay());
        }
        else
        {
            StartCoroutine(InvokeRewardedClosedWithDelay());
        }
    }

    private IEnumerator InvokeRewardedClosedWithDelay()
    {
        yield return null;
        onRewardedFinished?.Invoke(gotRewarded);
        gotRewarded = false;
        isFullScreenAdsShowing = false;
    }

    private void AdsMediation_OnInterstitialLoaded(string networkName)
    {
        loadedInterstitialNetwork = networkName;
        interstitialRetryAttempt = 0;
    }

    private void AdsMediation_OnInterstitialLoadFailed()
    {
        interstitialRetryAttempt++;
        int retryDelay = GetRetryDelayForCurrentAttempt(interstitialRetryAttempt);
        if (mainThread != Thread.CurrentThread)
        {
            Enqueue(IELoadInterstitialDelay(retryDelay));
        }
        else
        {
            StartCoroutine(IELoadInterstitialDelay(retryDelay));
        }
    }

    private void AdsMediation_OnInterstitialDisplayFailed()
    {
        MarkLastFullscreenAdsShown();
        loadedInterstitialNetwork = Studio1BAds.Studio1BConstants.AD_NETWORK_NAME_NONE;
        LoadInterstitial();
        if (mainThread != Thread.CurrentThread)
        {
            Enqueue(() => onIntersititalFinished?.Invoke(false));
        }
        else
        {
            onIntersititalFinished?.Invoke(false);
        }
        isFullScreenAdsShowing = false;
    }

    private void AdsMediation_OnInterstitialClosed()
    {
        MarkLastFullscreenAdsShown();
        loadedInterstitialNetwork = Studio1BAds.Studio1BConstants.AD_NETWORK_NAME_NONE;
        LoadInterstitial();
        if (mainThread != Thread.CurrentThread)
        {
            Enqueue(() => onIntersititalFinished?.Invoke(true));
        }
        else
        {
            onIntersititalFinished?.Invoke(true);
        }
        isFullScreenAdsShowing = false;
    }

    public void ShowBanner(BannerPosition position)
    {
        bannerShowingBalance++;
        if (bannerShowingBalance > 0)
        {
            adsMediation.ShowBanner(position);
        }
    }

    public void HideBanner()
    {
        bannerShowingBalance--;
        adsMediation.HideBanner();
    }

    public bool IsInterstitialAvailable
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
            return adsMediation.IsInterstitialAvailable;
#endif
        }
    }

    public void InitInterstitial()
    {
        adsMediation.InitInterstitial();
        LoadInterstitial();
    }

    public void LoadInterstitial()
    {
        adsMediation.LoadInterstitial();
    }

    private IEnumerator IELoadInterstitialDelay(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        LoadInterstitial();
    }

    public void ShowInterstitial(string placement, UnityAction<bool> onFinished)
    {
        onIntersititalFinished = onFinished;
        if (IsInterstitialAvailable)
        {
            isFullScreenAdsShowing = true;
        }
        adsMediation.ShowInterstitial(placement);
    }

    public bool IsRewardedAvailable
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
            return adsMediation.IsRewardedAdsAvailable;
#endif
        }
    }

    public void InitRewardedAds()
    {
        adsMediation.InitRewardedAds();
        LoadRewardedAds();
    }

    public void LoadRewardedAds()
    {
        adsMediation.LoadRewardedAds();
    }

    private IEnumerator IELoadRewardedAdsDelay(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        LoadRewardedAds();
    }

    public void ShowRewardedAds(string placement, UnityAction onRewardedShowFailed, UnityAction<bool> onFinished)
    {
        gotRewarded = false;
        this.onRewardedShowFailed = onRewardedShowFailed;
        onRewardedFinished = onFinished;
        if (IsRewardedAvailable)
        {
            isFullScreenAdsShowing = true;
        }
        adsMediation.ShowRewardedAds(placement);
    }

    public string LoadedInterstitialNetwork
    {
        get
        {
            return loadedInterstitialNetwork;
        }
    }

    public string LoadedRewardedNetwork
    {
        get
        {
            return loadedRewardedNetwork;
        }
    }

    public void InitRewardedInterstitialAds()
    {
#if MEDIATION_ADMOB_RI
        riAdsMediation.InitRewardedInterstitialAds();
#else
        adsMediation.InitRewardedInterstitialAds();
#endif
        LoadRewardedInterstitialAds();
    }

    public void LoadRewardedInterstitialAds()
    {
#if MEDIATION_ADMOB_RI
        riAdsMediation.LoadRewardedInterstitial();
#else
        adsMediation.LoadRewardedInterstitial();
#endif
    }

    public bool IsRewardedInterstitialAvailable
    {
        get
        {
#if MEDIATION_ADMOB_RI
            return riAdsMediation.IsRewardedInterstitialAvailable;
#else
            return adsMediation.IsRewardedInterstitialAvailable;
#endif
        }
    }

    public void ShowRewardedInterstitial(string placement, UnityAction onShowFailed, UnityAction<bool> onFinished)
    {
        gotRewarded = false;
        this.onRewardedInterstitialShowFailed = onShowFailed;
        this.onRewardedInterstitialFinished = onFinished;
        if (IsRewardedInterstitialAvailable)
        {
            isFullScreenAdsShowing = true;
        }
#if MEDIATION_ADMOB_RI
        riAdsMediation.ShowRewardedInterstitial(placement);
#else
        adsMediation.ShowRewardedInterstitial(placement);
#endif
    }

    private IEnumerator LoadRewardedInterstitialWithDelay(int seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);

#if MEDIATION_ADMOB_RI
        riAdsMediation.LoadRewardedInterstitial();
#else
        adsMediation.LoadRewardedInterstitial();
#endif
    }

    private void MarkLastFullscreenAdsShown()
    {
        lastFullscreenAdsShown = DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    private int GetRetryDelayForCurrentAttempt(int attempt)
    {
        int retryDelay = Power(2, Math.Min(5, attempt));
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            retryDelay = 10;
        }
        return retryDelay;
    }

    private int Power(int baseNumber, int exponent)
    {
        int ret = 1;
        for (int i = 0; i < exponent; i++)
        {
            ret *= baseNumber;
        }
        return ret;
    }

}
