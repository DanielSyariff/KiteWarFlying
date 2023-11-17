using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsController : MonoBehaviour
{
    public bool developerMode;
    public AdsManager adsManager;
    public Button showBanner, showInterstitial, showRewarded;

    private void Awake()
    {
        adsManager = FindObjectOfType<AdsManager>();
        adsManager.removeAds = PlayerPrefs.GetInt("RemoveAdsEnabled", 0);
    }
    private void Start()
    {
        CallBannerAds();
    }
    public void BuyRemoveAds()
    {
        PlayerPrefs.SetInt("RemoveAdsEnabled", 1);
        adsManager.removeAds = PlayerPrefs.GetInt("RemoveAdsEnabled");
    }

    private void Update()
    {
        if (developerMode)
        {
            if (adsManager.adsReady)
            {
                showBanner.gameObject.SetActive(true);
                showInterstitial.gameObject.SetActive(true);
                showRewarded.gameObject.SetActive(true);
            }
            else
            {
                showBanner.gameObject.SetActive(false);
                showInterstitial.gameObject.SetActive(false);
                showRewarded.gameObject.SetActive(false);
            }
        }
    }

    public void CallBannerAds()
    {
        adsManager.ShowBannerAds();
    }
    public void CallInterstitialAds()
    {
        adsManager.ShowInterstitialAds();
    }
    public void RewardedPlayGames()
    {
        adsManager.ShowRewardedAds_PlayGame();
    }
    public void RewardedExitGames()
    {
        adsManager.ShowRewardedAds_ExitGame();
    }
    public void RewardedPlayAgain()
    {
        adsManager.ShowRewardedAds_PlayAgain();
    }
}
