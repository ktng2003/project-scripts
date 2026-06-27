using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class RewardAdsManager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    public void Start()
    {
        InitAds();
    }

    public void InitAds()
    {
        string adUnitId;

#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        RewardedAd.Load(adUnitId, new AdRequest(), LoadCallback);
    }

    public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
    {
        if (rewardedAd != null)
            this.rewardedAd = rewardedAd;
        else
            Debug.Log(loadAdError.GetMessage());
    }

    public void ShowAds()
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
            rewardedAd.Show(OnUserEarnedReward);
        else if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(OnUserEarnedReward);

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("광고 닫힘 -> 새 광고 로드");
                InitAds();
            };

            rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.Log("광고 도중 오류: " + error);
                InitAds();
            };
        }
        else
        {
            Debug.Log("광고 재생 실패");
            InitAds();
        }
    }

    public void OnUserEarnedReward(Reward reward)
    {
        GameManager.Instance.countPlay += 5;
        GameManager.Instance.GameCountPlay();
        Shop.Instance.btAd.interactable = false;
        GameManager.Instance.Save();
    }
}