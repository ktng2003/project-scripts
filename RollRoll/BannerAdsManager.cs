using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System.Linq;

public class BannerAdsManager : MonoBehaviour
{
    string adUnitId;

    public BannerView bannerView;

    private static BannerAdsManager instance;
    public static BannerAdsManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<BannerAdsManager>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<BannerAdsManager>();
                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<BannerAdsManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void InitAds()
    {
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/6300978111";
#elif UNITY_IOS
        adUnitId = "ca-app-pub-3940256099942544/2934735716";
#else
        adUnitId = "unexpected_platform";
#endif

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        AdRequest request = new AdRequest();
        bannerView.LoadAd(request);
    }

    public void HideBanner()
    {
        bannerView?.Hide();
    }
}