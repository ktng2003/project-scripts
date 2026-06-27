using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardAdsManager : MonoBehaviour
{
    private RewardedAd rewardedAd;

    private static RewardAdsManager instance;
    public static RewardAdsManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<RewardAdsManager>();

                if (obj != null)
                    instance = obj;
                else
                {
                    var newObj = new GameObject().AddComponent<RewardAdsManager>();

                    instance = newObj;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        var objs = FindObjectsOfType<RewardAdsManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
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
   
    public void ShowAdsForPlanetChange()
    {
        if (GameManager.Instance.isRemoveAllAds == true)
            PlanetManager.Instance.ChangeSelectedPlanetRandomly();
        else if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(OnUserEarnedRewardForPlanetChange);

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

    public void OnUserEarnedRewardForPlanetChange(Reward reward)
    {
        PlanetManager.Instance.ChangeSelectedPlanetRandomly();
    }

    public void ShowAdsForRestartGame()
    {    
        if (GameManager.Instance.isRemoveAllAds == true)
           GameManager.Instance.RestartGame();
        else if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(OnUserEarnedRewardForRestartGame);

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
  
    public void OnUserEarnedRewardForRestartGame(Reward reward)
    {
        GameManager.Instance.RestartGame();
    }
}