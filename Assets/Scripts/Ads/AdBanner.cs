using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdBanner : MonoBehaviour
{
    private const string BannerId = "ca-app-pub-4436861112083738/2048354312";
    
    [SerializeField] private float _showAfterSeconds = 3f;
    private BannerView _bannerView;

    private void OnEnable()
    {
        _bannerView = CreateBannerView();
        StartCoroutine(ShowBannerAfterSeconds(_bannerView, _showAfterSeconds));
    }

    private BannerView CreateBannerView()
    {
        BannerView bannerView = new BannerView(BannerId, AdSize.Banner, AdPosition.Bottom);
        AdRequest adRequest = new AdRequest.Builder().Build();
        bannerView.LoadAd(adRequest);
        bannerView.Hide();
        return bannerView;
    }

    private IEnumerator ShowBannerAfterSeconds(BannerView bannerView, float time)
    {
        yield return new WaitForSeconds(time);
        bannerView.Show();
    }
}
