using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour
{
    public int removeAds;
    public bool adsReady;
    public MainMenuManager mainMenuManager;
    public ScoreAndResultManager scoreAndResultManager;

    void Start()
    {
        Advertisements.Instance.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Advertisements.initialized)
        {
            adsReady = true;
        }
        else
        {
            adsReady = false;
        }
    }

    public void ShowBannerAds()
    {
        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
    }

    public void ShowInterstitialAds()
    {
        if (removeAds == 0)
        {
            Advertisements.Instance.ShowInterstitial(/* Can be used for show Event After Ads*/);
        }
        else
        {
            Debug.Log("Remove Ads Enabled");
        }
    }

    public void ShowRewardedAds_PlayGame()
    {
        mainMenuManager = FindObjectOfType<MainMenuManager>();
        Advertisements.Instance.ShowRewardedVideo(GetRewardAds_PlayGame);
    }

    private void GetRewardAds_PlayGame(bool success)
    {
        if (success)
        {
            //Get Reward
            Debug.Log("SUCCES REWARDED ADS");
            mainMenuManager.SuccessAds_PlayGame();
        }
        else
        {
            mainMenuManager.SuccessAds_PlayGame();
            //Show Failed
            Debug.Log("FAILED REWARDED ADS");
#if UNITY_EDITOR
            mainMenuManager.SuccessAds_PlayGame();
#endif
        }
    }

    public void ShowRewardedAds_ExitGame()
    {
        scoreAndResultManager = FindObjectOfType<ScoreAndResultManager>();
        Advertisements.Instance.ShowRewardedVideo(GetRewardAds_ExitGame);
    }

    private void GetRewardAds_ExitGame(bool success)
    {
        if (success)
        {
            //Get Reward
            Debug.Log("SUCCES REWARDED ADS");
            scoreAndResultManager.SuccessAds_ExitGame();
        }
        else
        {
            //Show Failed
            Debug.Log("FAILED REWARDED ADS");
#if UNITY_EDITOR
            scoreAndResultManager.SuccessAds_ExitGame();
#endif
        }
    }

    public void ShowRewardedAds_PlayAgain()
    {
        scoreAndResultManager = FindObjectOfType<ScoreAndResultManager>();
        Advertisements.Instance.ShowRewardedVideo(GetRewardAds_PlayAgain);
    }

    private void GetRewardAds_PlayAgain(bool success)
    {
        if (success)
        {
            //Get Reward
            Debug.Log("SUCCES REWARDED ADS");
            scoreAndResultManager.SuccessAds_PlayAgain();
        }
        else
        {
            //Show Failed
            Debug.Log("FAILED REWARDED ADS");
#if UNITY_EDITOR
            scoreAndResultManager.SuccessAds_PlayAgain();
#endif
        }
    }
}
