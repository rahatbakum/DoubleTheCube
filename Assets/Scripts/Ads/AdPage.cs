using UnityEngine;
using System;
using GoogleMobileAds.Api;

public class AdPage : MonoBehaviour
{
    private const string PageId = "ca-app-pub-4436861112083738/5193392401";
    private InterstitialAd _interstitialAd;

    public void Show()
    {
        if(_interstitialAd.IsLoaded())
            _interstitialAd.Show();
        _interstitialAd = CreateIntersitialAd();
    }

    private void OnEnable()
    {
        _interstitialAd = CreateIntersitialAd();
    }

    private void LoadNewAd(ref InterstitialAd interstitialAd)
    {
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(adRequest);
    }

    private InterstitialAd CreateIntersitialAd()
    {
        InterstitialAd interstitialAd = new InterstitialAd(PageId);
        interstitialAd.OnAdOpening += OnAdOpening;
        interstitialAd.OnAdClosed += OnAdClosed;
        LoadNewAd(ref interstitialAd);
        return interstitialAd;
    }

    private void OnAdOpening(object sender, EventArgs eventArgs)
    {
        GameManager.Instance.Pause();
    }

    private void OnAdClosed(object sender, EventArgs eventArgs)
    {
        GameManager.Instance.Unpause();
    }
}
