using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;


public class RewardAd : MonoBehaviour
{
    private RewardedAd rewardedAd;
    private string adUnitId = "ca-app-pub-3940256099942544/5224354917";

    void Start()
    {
        InitAd();
    }

    public void WatchAd()
    {
        if(rewardedAd.IsLoaded() == true)
        {
            rewardedAd.Show();
        }
        else
        {
            InitAd();
        }
    }

    public void InitAd()
    {
        rewardedAd = new RewardedAd(adUnitId);

        rewardedAd.OnAdLoaded += RewardedAd_OnAdLoaded;
        rewardedAd.OnAdFailedToLoad += RewardedAd_OnAdFailedToLoad;
        rewardedAd.OnAdOpening += RewardedAd_OnAdOpening;
        rewardedAd.OnAdFailedToShow += RewardedAd_OnAdFailedToShow;
        rewardedAd.OnUserEarnedReward += RewardedAd_OnUserEarnedReward;
        rewardedAd.OnAdClosed += RewardedAd_OnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(request);

    }

    #region 광고 객체 이벤트 핸들러

    private void RewardedAd_OnAdLoaded(object sender, System.EventArgs e)
    {
        print("광고가 로드되었습니다.");
    }

    private void RewardedAd_OnAdFailedToLoad(object sender, AdErrorEventArgs e)
    {
        print("광고 로드에 실패하였습니다 : " + e.ToString());
    }

    private void RewardedAd_OnAdOpening(object sender, System.EventArgs e)
    {
        print("광고를 실행합니다.");
        GameObject.Find("Canvas").SetActive(false);
    }

    private void RewardedAd_OnAdFailedToShow(object sender, AdErrorEventArgs e)
    {
        print("광고 실행이 실패하였습니다 : " + e.ToString());
    }

    private void RewardedAd_OnUserEarnedReward(object sender, Reward e)
    {
        string type = "gold";
        double amount = Main.instance.gold * ((e.Amount + 5) * 0.1f);
        GameManager.Singleton.gold += System.Convert.ToInt32(amount);
        print("광고 시청 보상을 지급하였습니다 : " + amount.ToString() + " " + type);
    }

    private void RewardedAd_OnAdClosed(object sender, System.EventArgs e)
    {
        print("광고 창을 닫습니다.");
        InitAd();
        
        if (Main.instance.score > GameManager.Singleton.bestScore)
        {
            GameManager.Singleton.bestScore = Main.instance.score;
        }
        SaveManager.Singleton.SavePlayerData();

        GameManager.Singleton.restart = true;
        GameManager.Singleton.LoadNextScene("Main");
        Main.instance.isGame = false;
    }

    #endregion
}
